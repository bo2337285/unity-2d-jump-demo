using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
    private Transform mainCam;
    public float moveRateX, moveRateY;
    private Vector2 startPos;
    public bool isLockY = true;
    void Start () {
        startPos = transform.position;
        mainCam = Camera.main.transform;
    }

    void Update () {
        transform.position = new Vector2 (startPos.x + mainCam.position.x * moveRateX, isLockY ? transform.position.y : startPos.y + transform.position.y * moveRateY);
    }
}