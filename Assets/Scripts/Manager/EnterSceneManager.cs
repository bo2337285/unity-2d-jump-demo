using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterSceneManager : MonoBehaviour {

    public GameObject dialog;
    private Boolean dialogFlag = false;
    private Observer observer;
    public string sceneName;

    void Start () {
        init ();
    }
    void Update () {
        dialog.SetActive (dialogFlag);
        if (dialogFlag && Input.GetKeyDown (KeyCode.E)) {
            saveGameObject ();
            enterNextScene ();
        }
    }

    void saveGameObject () {
        DontDestroyOnLoad (GameObject.Find ("UI"));
        DontDestroyOnLoad (GameObject.Find ("GamePlay"));
        DontDestroyOnLoad (GameObject.Find ("Menu"));
        DontDestroyOnLoad (GameObject.Find ("InputControl"));
    }
    void enterNextScene () {
        TargetEventArgs e = new TargetEventArgs (gameObject);
        e.args = new object[] { sceneName };
        observer.dispatch (EventEnum.EnterScence, gameObject, e);
        SceneManager.LoadScene (sceneName);
    }

    private void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "Player") {
            dialogFlag = true;
        }
    }

    private void OnTriggerExit2D (Collider2D other) {
        if (other.tag == "Player") {
            dialogFlag = false;
        }
    }

    void init () {
        observer = GetComponent<Observer> ();
    }
}