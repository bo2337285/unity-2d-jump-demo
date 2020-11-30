using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour {

    // [SerializeField] 展示私有属性于界面中
    private Observer observer;
    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D circleColl, crouchDisColl;
    private Transform playerTransform;
    private SpriteRenderer sprite;
    public AudioSource jumpAudio, hurtAudio;
    // 移动速度,跳跃力
    public float speed = 5, jumpForce = 5;
    // 地面监测点,地面Layer 用来检测是否踩在地面上
    public LayerMask ground;
    Vector2[] collAllPoints;
    // 状态
    [SerializeField] public bool isGround, isJump, isHurt, isRunning, isCrouch;

    // 判断是否按了跳跃键
    bool jumpPressed, crouchPressed;
    // 记录可跳跃次数
    public int jumpCountReset = 2;
    [SerializeField] private int jumpCount;
    // ------------- handler start------------
    void Start () {
        // 赋值自己组件
        rb = GetComponent<Rigidbody2D> ();
        anim = GetComponent<Animator> ();
        playerTransform = GetComponent<Transform> ();
        sprite = GetComponent<SpriteRenderer> ();
        observer = GetComponent<Observer> ();
    }
    // Update 根据终端频率执行,用于响应事件
    // FixUpdate 固定刷新频率50hz,用于执行动画
    void Update () {
        inputListen ();
    }
    void FixedUpdate () {
        updateState ();
        playerAction ();
        switchAnim ();
        switchAudio ();
    }
    // 接触处理
    private void OnTriggerEnter2D (Collider2D other) {
        // 碰了道具
        if (other.tag == "Items") {
            TargetEventArgs e = new TargetEventArgs (other.gameObject);
            e.args = new object[] { other };
            observer.dispatch (EventEnum.TouchItems, gameObject, e);
        }
        if (other.tag == "DeathArea") {
            observer.dispatch (EventEnum.GameOver, gameObject);
        }
    }
    // 碰撞处理
    private void OnCollisionEnter2D (Collision2D other) {

        GameObject otherObj = other.gameObject;
        ContactPoint2D contact = other.contacts[0];

        if (otherObj.tag == "Enemy") { // 接触敌人
            Vector2[] allPoints = Utils.GetBoxPoints (other.collider);
            // player-敌人向量与x轴夹角
            float angle = Utils.getAngle (playerTransform.position, otherObj.transform.position);
            // 敌人碰撞体的左上角/右上角-中心与x轴的夹角
            float angleMin = Utils.getAngle (allPoints[0], otherObj.transform.position);
            float angleMax = Utils.getAngle (allPoints[1], otherObj.transform.position);
            if (angle >= angleMin && angle <= angleMax) { // 踩敌人
                // FIXME 后面改为事件通知
                EnemyManager enemyManger = other.gameObject.GetComponent<EnemyManager> ();
                enemyManger.Death ();
                // TargetEventArgs e = new TargetEventArgs (other.gameObject);
                // observer.dispatch (EventEnum.BreakEnemy, gameObject, e);
                rb.velocity = new Vector2 (rb.velocity.x, jumpForce * 0.5f); //踩完弹起
            } else { // 碰到敌人,受伤后弹
                isHurt = true;
                TargetEventArgs e = new TargetEventArgs (other.gameObject);
                observer.dispatch (EventEnum.Hurt, gameObject, e);
                Vector2 back = (Vector2) playerTransform.position - contact.point;
                rb.velocity = new Vector2 (back.x * 2, back.y * 2);
            }
        }
    }
    void OnGUI () {
        String[] infoList = {
            "x: " + playerTransform.position.x,
            "y: " + playerTransform.position.y,
            "velocity.x: " + rb.velocity.x,
            "velocity.y: " + rb.velocity.y
        };
        GuiUtils.printInfoInGUI (playerTransform, infoList);
    }
    // ------------- handler end------------

    // 按键监听
    void inputListen () {
        jumpPressed = Input.GetButtonDown ("Jump");
        crouchPressed = Input.GetButton ("Crouch");
    }
    void updateState () {
        // 获取角色球形碰撞体矩形顶点
        collAllPoints = Utils.GetBoxPoints (circleColl);
        // 获取是否触地
        isGround = Physics2D.OverlapCircle (collAllPoints[2], 0.1f, ground) || Physics2D.OverlapCircle (collAllPoints[3], 0.1f, ground);
    }
    void playerAction () {
        if (!isHurt) {
            // 水平移动
            GroundMovement ();
            // 跳
            Jump ();
            // 下蹲
            Crouch ();
        }
    }
    // 动画切换
    void switchAnim () {
        anim.SetBool ("isIdle", false);
        anim.SetBool ("isRunning", isRunning);
        anim.SetBool ("isJump", isJump);
        anim.SetBool ("isCrouch", isCrouch);

        if (isHurt) { // 受伤
            anim.SetBool ("isHurt", true);
            if (Mathf.Abs (rb.velocity.x) < 0.1f && Mathf.Abs (rb.velocity.y) < 0.1f) { //弹开后不再运动才停止受伤动画
                isHurt = false;
                anim.SetBool ("isHurt", false);
            }
        }

        if (rb.velocity.y <= -1f && !isGround) { // 下降
            anim.SetBool ("isFall", true);
        } else {
            anim.SetBool ("isFall", false);
        }

        if (isGround) { // 触地
            anim.SetBool ("isIdle", true);
        }
    }
    void switchAudio () {
        // if (jumpPressed && isJump) {
        //     jumpAudio.Play ();
        // }
        if (isHurt && !hurtAudio.isPlaying) {
            hurtAudio.Play ();
        }

    }
    // 跳跃
    void Jump () {
        if (isGround && rb.velocity.y < 0.01f) {
            // 触地时重置可跳跃次数
            jumpCount = jumpCountReset;
            isJump = false;
        }
        if (jumpPressed && jumpCount > 0) {
            jumpAudio.Play ();
            isJump = true;
            rb.velocity = new Vector2 (rb.velocity.x, jumpForce);
            jumpCount--;
            // 防止重复触发
            jumpPressed = false;
        }
    }
    // 移动
    void GroundMovement () {
        // 刚体移动
        // [-1,0] [0,1]
        float horizontalmove = Input.GetAxis ("Horizontal");
        if (horizontalmove != 0) {
            rb.velocity = new Vector2 (horizontalmove * speed * (!isCrouch? 1f : 0.5f), rb.velocity.y);
            // anim.SetFloat ("running", Mathf.Abs (horizontalmove));
            isRunning = true;
        } else {
            isRunning = false;
        }

        // 动画翻转
        // 方向 [-1,0,1] 反向,无动作,正向
        float faceDirection = Input.GetAxisRaw ("Horizontal");
        if (faceDirection != 0) {
            // transform.localScale = new Vector3 (faceDirection, 1, 1);
            sprite.flipX = faceDirection < 0 ? true : false;
        }
    }
    // 下蹲
    void Crouch () {
        if (!crouchDisColl.enabled && (Physics2D.OverlapCircle (collAllPoints[0], 0.3f, ground) || Physics2D.OverlapCircle (collAllPoints[1], 0.3f, ground))) {
            isCrouch = true;
        } else {
            isCrouch = crouchPressed;
        }
        crouchDisColl.enabled = !isCrouch;
    }
}