using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {
    public void playGame () {
        SceneManager.LoadScene ("Scenes/Scene0");
    }
    public void quitGame () {
        Application.Quit ();
    }
}