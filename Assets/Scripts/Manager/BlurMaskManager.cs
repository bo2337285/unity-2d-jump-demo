using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlurMaskManager : MonoBehaviour {
    public Transform tf;
    void Start () {
        init ();
    }
    void Update () {
        Utils.followCamera (tf);
    }
    void init () {
        tf = GetComponent<Transform> ();
        Utils.fullScreen (tf);
    }
}