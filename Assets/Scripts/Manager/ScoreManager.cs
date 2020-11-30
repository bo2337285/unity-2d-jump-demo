using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour {
    public Text CherryText;
    private Observer observer;
    void Start () {
        CherryText = GetComponent<Text> ();
        observer = GetComponent<Observer> ();
        observer.listen (EventEnum.ScoreAdd, onScoreAdd);
    }
    private void onScoreAdd (object sender, System.EventArgs e) {
        // CherryText.text = gamePlay.score;
    }
}