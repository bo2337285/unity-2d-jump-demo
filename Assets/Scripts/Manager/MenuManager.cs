using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {

    public GameObject pauseMenu, settingMenu;
    private Boolean pauseFlag = false;
    public void pause () {
        Time.timeScale = 0f;
        pauseFlag = true;
        pauseMenu.SetActive (true);
    }
    public void resume () {
        Time.timeScale = 1f;
        pauseFlag = false;
        pauseMenu.SetActive (false);
        settingMenu.SetActive (false);
    }
    public void setting () {
        settingMenu.SetActive (true);
        pauseMenu.SetActive (false);
    }
    public void toMainMenu () {
        resume ();
        SceneManager.LoadScene ("Scenes/MainMenu");
    }
    void Start () {
        init ();
    }
    void Update () {
        inputListen ();
    }

    void init () { }
    void inputListen () {
        if (Input.GetKeyDown (KeyCode.Escape)) {
            if (!pauseFlag) {
                pause ();
            } else {
                resume ();
            }
        }
    }
}