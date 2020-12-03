using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {
    private Observer observer;
    private AudioSource destoryAudio;
    private Boolean isDestory = false;

    private void Start () {
        init ();
    }
    void init () {
        observer = GetComponent<Observer> ();
        destoryAudio = GetComponent<AudioSource> ();
    }
    // 接触处理
    private void OnTriggerEnter2D (Collider2D other) {
        // 碰了道具
        if (other.tag == "Player") {
            onTouchItems ();
        }
    }
    private void onTouchItems () {
        if (!isDestory) {
            isDestory = true;
            GetComponent<Collider2D> ().enabled = false;
            GetComponent<Animator> ().SetTrigger ("Destroy");
            destoryAudio.Play ();
            observer.dispatch (EventEnum.ScoreAdd, gameObject);
        }
    }

    private void destoryItem () {
        Destroy (gameObject);
    }
}