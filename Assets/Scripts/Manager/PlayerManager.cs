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
    private BoxCollider2D coll;
    private Vector2 collSourceSize;
    private Vector2 collSourceOffset;
    private Transform tf;
    private SpriteRenderer sprite;

    public AudioSource jumpAudio, hurtAudio;
    // 移动速度,跳跃力
    public float speed = 5, jumpForce = 5;
    // 地面监测点,地面Layer 用来检测是否踩在地面上
    public LayerMask ground;
    private Vector2[] collAllPoints;
    // 状态
    [SerializeField] public bool isGround, isJump, isHurt, isRun, isCrouch, isFall;

    // 判断是否按了跳跃键
    private bool jumpPressed, crouchPressed;
    // 记录可跳跃次数
    public int jumpCountReset = 2;
    private int jumpCount;
    private float horizontalmove = 0;
    public float hurtIntervalTime = 0.5f;
    // ------------- handler start------------
    void Start () {
        init ();
    }
    // Update 根据终端频率执行,用于响应事件
    // FixUpdate 固定刷新频率50hz,用于执行动画
    void Update () {
        inputListen ();
    }
    void FixedUpdate () {
        updateState ();
        playerAction ();
        switchAnim1 ();
        // switchAnim ();
        switchAudio ();
    }
    // 接触处理
    private void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "DeathArea") {
            observer.dispatch (EventEnum.GameOver, gameObject);
        }
    }
    // 碰撞处理
    private void OnCollisionEnter2D (Collision2D other) {

        GameObject otherObj = other.gameObject;
        ContactPoint2D contact = other.contacts[0];

        if (otherObj.tag == "Enemy") { // 接触敌人
            Debug.Log ("p:" + contact.point);
            if (Utils.isBeTrampleOn (contact.point, otherObj)) { //判断敌人是否被踩
                // rb.velocity = new Vector2 (rb.velocity.x, jumpForce * 0.5f); //踩完弹起
            } else if (!isHurt) { // 碰到敌人,受伤后弹
                // FIXME 踩敌人还会触发受伤问题
                isHurt = true;
                Invoke ("hurtIntervalEnd", hurtIntervalTime);
                TargetEventArgs e = new TargetEventArgs (other.gameObject);
                observer.dispatch (EventEnum.Hurt, gameObject, e);
                Vector2 back = (Vector2) tf.position - contact.point;
                // rb.velocity = new Vector2 (back.x * 5, back.y);
            }
        }
    }
    void OnGUI () {
        String[] infoList = {
            "x: " + tf.position.x,
            "y: " + tf.position.y,
            "velocity.x: " + rb.velocity.x,
            "velocity.y: " + rb.velocity.y
        };
        GuiUtils.printInfoInGUI (tf, infoList);
    }
    // ------------- handler end------------

    void init () {
        // 赋值自己组件
        coll = GetComponent<BoxCollider2D> ();
        collSourceSize = coll.size;
        collSourceOffset = coll.offset;
        rb = GetComponent<Rigidbody2D> ();
        anim = GetComponent<Animator> ();
        tf = GetComponent<Transform> ();
        sprite = GetComponent<SpriteRenderer> ();
        observer = GetComponent<Observer> ();
    }
    // 按键监听
    void inputListen () {
        jumpPressed = Input.GetButtonDown ("Jump");
        crouchPressed = Input.GetButton ("Crouch");
        horizontalmove = Input.GetAxis ("Horizontal");
    }
    void updateState () {
        // 获取角色碰撞体矩形顶点
        collAllPoints = Utils.GetBoxPoints (coll);
        // 获取是否触地
        isGround = Physics2D.OverlapCircle (collAllPoints[2], 0.1f, ground) || Physics2D.OverlapCircle (collAllPoints[3], 0.1f, ground);
        if (rb.velocity.y <= -1f && !isGround) {
            isFall = true;
            isJump = false;
        } else {
            isFall = false;
        }
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
    void switchAnim1 () {
        AnimEnum animName = AnimEnum.Idle;
        if (isHurt) {
            animName = AnimEnum.Hurt;
        } else {
            if (isGround) {
                if (isCrouch) {
                    animName = AnimEnum.Crouch;
                } else if (isRun) {
                    animName = AnimEnum.Run;
                }
            } else {
                if (isFall) {
                    animName = AnimEnum.Fall;
                } else if (isJump) {
                    animName = AnimEnum.Jump;
                }
            }
        }
        anim.Play (animName.ToString ());
    }
    void switchAnim () {
        anim.SetBool ("isIdle", false);
        anim.SetBool ("isRun", isRun);
        anim.SetBool ("isJump", isJump);
        anim.SetBool ("isCrouch", isCrouch);
        if (isHurt) { // 受伤
            // Debug.Log (anim.GetBool ("isHurt"));
            anim.SetBool ("isHurt", true);
            // if (Mathf.Abs (rb.velocity.x) < 0.1f) { //弹开后不再运动才停止受伤动画
            //     isHurt = false;
            //     // anim.SetBool ("isHurt", false);
            // }
        } else {
            if (rb.velocity.y <= -1f && !isGround) { // 下降
                anim.SetBool ("isFall", true);
            } else {
                anim.SetBool ("isFall", false);
            }

            if (isGround) { // 触地
                anim.SetBool ("isIdle", true);
            }
        }

    }
    void switchAudio () {
        // if (jumpPressed && isJump) {
        //     jumpAudio.Play ();
        // }
        if (isHurt && !hurtAudio.isPlaying) {
            // hurtAudio.Play ();
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
        // float horizontalmove = Input.GetAxis ("Horizontal");
        if (horizontalmove != 0) {
            rb.velocity = new Vector2 (horizontalmove * speed * (!isCrouch? 1f : 0.5f), rb.velocity.y);
            // 动画翻转
            sprite.flipX = horizontalmove < 0 ? true : false;
            isRun = true;
        } else {
            isRun = false;
        }

        // 动画翻转
        // 方向 [-1,0,1] 反向,无动作,正向
        // float faceDirection = Input.GetAxisRaw ("Horizontal");
        // if (faceDirection != 0) {
        //     // transform.localScale = new Vector3 (faceDirection, 1, 1);
        //     sprite.flipX = faceDirection < 0 ? true : false;
        // }
    }
    // 下蹲
    void Crouch () {
        if ((coll.size.y != collSourceSize.y) && (Physics2D.OverlapCircle (collAllPoints[0], 0.3f, ground) || Physics2D.OverlapCircle (collAllPoints[1], 0.3f, ground))) {
            isCrouch = true;
        } else {
            isCrouch = crouchPressed;
        }
        if (isCrouch) {
            float dis = collSourceSize.y * 0.4f;
            coll.size = new Vector2 (collSourceSize.x, collSourceSize.y - dis);
            coll.offset = new Vector2 (collSourceOffset.x, collSourceOffset.y - dis * 0.5f);
        } else {
            coll.size = collSourceSize;
            coll.offset = collSourceOffset;
        }
    }
    void hurtIntervalEnd () {
        isHurt = false;
    }
}