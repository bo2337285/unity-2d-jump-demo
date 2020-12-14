using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickManager : MonoBehaviour {
    public Joystick joystick;
    public Canvas joystickUI;
    private bool jumpPressed;
    public float jumpIntervalTime;
    // private float horizontalmove = 0;
    private Observer observer;
    private InputEventArgs inputEventArgs;
    void Start () {
        init ();
    }

    void Update () {
        inputManager ();
    }

    void init () {
        observer = GetComponent<Observer> ();
        inputEventArgs = new InputEventArgs ();
        jumpIntervalTime = Time.deltaTime;
        // 控制虚拟按键
        // if ("Desktop" == SystemInfo.deviceType.ToString ()) {
        //     joystickUI.enabled = false;
        // }
    }

    public void onJump () {
        if (!jumpPressed) {
            jumpPressed = true;
            Invoke ("resetJumpPress", jumpIntervalTime);
        }
    }
    void resetJumpPress () {
        jumpPressed = false;
    }

    void inputManager () {
        inputEventArgs.jumpPressed = jumpPressed || Input.GetButtonDown ("Jump");
        inputEventArgs.crouchPressed = Input.GetButton ("Crouch") || joystick.Vertical < -0.5f;
        inputEventArgs.horizontalmove = Input.GetAxis ("Horizontal") != 0 ? Input.GetAxis ("Horizontal") : joystick.Horizontal;

        observer.dispatch (EventEnum.Input, gameObject, inputEventArgs);
    }

    private void OnGUI () {
        printGuiInfo ();
    }

    void printGuiInfo () {
        GUI.Label (new Rect (Screen.width - 200, 50, 200, 40), "jumpPressed :" + inputEventArgs.jumpPressed);
        GUI.Label (new Rect (Screen.width - 200, 100, 200, 40), "crouchPressed :" + inputEventArgs.crouchPressed);
        GUI.Label (new Rect (Screen.width - 200, 150, 200, 40), "horizontalmove :" + inputEventArgs.horizontalmove);
    }
}