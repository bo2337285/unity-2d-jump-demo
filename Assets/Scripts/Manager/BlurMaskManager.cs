using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurMaskManager : MonoBehaviour {
    public Transform tf;
    public GameObject mainCamera;
    // Update is called once per frame
    void Start () {
        mainCamera = GameObject.FindGameObjectsWithTag ("MainCamera") [0];
        tf = GetComponent<Transform> ();
        fixScreen ();
    }
    void Update () {
        tf.position = new Vector3 (mainCamera.transform.position.x, mainCamera.transform.position.y, tf.position.z);
    }

    // TODO 同步遮罩层与相机大小一致,并且在player运动时再显示
    void fixScreen () {
        Camera cam = Camera.main;
        float pos = (cam.nearClipPlane + 0.01f);

        // tf.position = cam.transform.position + cam.transform.forward * pos;
        // tf.LookAt (cam.transform);
        // tf.Rotate (90.0f, 180.0f, 0.0f);

        float h = (Mathf.Tan (cam.fieldOfView * Mathf.Deg2Rad * 0.5f) * pos * 2f) / 10.0f;

        tf.localScale = new Vector3 (h * 100 * 3.2f, 1.0f, h * cam.aspect * 100);
    }
}