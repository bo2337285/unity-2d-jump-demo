using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GamePlay : MonoBehaviour {
    private GameObject player;
    public int score = 0;
    private Observer observer;
    void Start () {
        init ();
    }
    private void onScoreAdd (object sender, System.EventArgs e) {
        score++;
    }
    private void onGameOver (object sender, System.EventArgs e) {
        Invoke ("restart", 0.5f);
    }

    void init () {
        player = GameObject.Find ("Player");
        observer = GetComponent<Observer> ();
        observer.listen (EventEnum.ScoreAdd, onScoreAdd);
        observer.listen (EventEnum.GameOver, onGameOver);
    }

    void restart () {
        SceneManager.LoadScene (SceneManager.GetActiveScene ().name);
    }
}