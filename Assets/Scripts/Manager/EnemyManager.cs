using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Animator anim;
    [SerializeField] protected Transform tf;
    [SerializeField] protected AudioSource deathAudio;
    protected Transform player;
    protected Boolean isMoving;
    float distance, horizontalDirection, verticalDirection;
    public float searchDistance = 10f;
    protected SpriteRenderer sprite;

    public float speed = 0.5f;
    void Start () {
        rb = GetComponent<Rigidbody2D> ();
        anim = GetComponent<Animator> ();
        tf = GetComponent<Transform> ();
        sprite = GetComponent<SpriteRenderer> ();
        deathAudio = GetComponent<AudioSource> ();
        player = GameObject.Find ("Player").transform;
    }
    protected void FixedUpdate () {
        Moving ();
    }
    protected void Update () {
        AnimSwitch ();
    }

    public virtual void Death () {
        anim.SetTrigger ("Death");
        deathAudio.Play();
    }
    public virtual void OnDeathAnimEnd () {
        Destroy (gameObject);
    }

    protected virtual void Moving () {
        distance = Utils.getDistance (player.position, tf.position);
        // 判断方向 Direction.x : 1 右, -1 左 , Direction.y : 1 上, -1 下
        Vector2 direction = Utils.getDirection (tf, player);
        horizontalDirection = direction.x;
        verticalDirection = direction.y;

        // TODO 索敌,寻路
        //当与player距离大于10,则朝主角移动
        if (distance > searchDistance) {
            if (rb.gravityScale > 0.1f) { // 地面敌人
                rb.velocity = new Vector2 (horizontalDirection * speed, rb.velocity.y);
            } else { //飞行敌人
                rb.velocity = new Vector2 (
                    horizontalDirection * speed, -1 * verticalDirection * speed
                );
            }
            isMoving = true;
        } else {
            // 免得兄弟刹不住车
            rb.velocity = Vector2.zero;
            isMoving = false;
        }
        // 动画翻转
        if (horizontalDirection != 0) {
            // transform.localScale = new Vector3 (-1 * horizontalDirection, 1, 1);
            sprite.flipX = horizontalDirection > 0 ? true : false;
        }

    }

    protected void OnGUI () {
        String[] infoList = {
            "distance: " + distance,
            "verticalDirection: " + verticalDirection,
            "horizontalDirection: " + horizontalDirection,
            "x: " + tf.position.x,
            "y: " + tf.position.y
        };
        GuiUtils.printInfoInGUI (tf, infoList);
    }

    void AnimSwitch () {
        anim.SetBool ("isMoving", isMoving);
        // if (isDeath){
        //     anim.SetTrigger ("isDeath");
        // }
    }

}