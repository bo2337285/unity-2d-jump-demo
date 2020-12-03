using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
    public Text CherryText;
    private GamePlay gamePlay;
    void Start () {
        init ();
    }
    void Update () {
        updateScore ();
    }
    void init () {
        gamePlay = FindObjectOfType<GamePlay> ();
        CherryText = GetComponent<Text> ();
    }
    private void updateScore () {
        CherryText.text = gamePlay.score.ToString ();
    }
}