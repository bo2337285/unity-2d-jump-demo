using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {
    private Observer observer;
    public AudioSource destoryAudio;

    private void Start () {
        observer = GetComponent<Observer> ();
        destoryAudio = GetComponent<AudioSource> ();
        observer.listen (EventEnum.TouchItems, onTouchItems);
    }
    private void onTouchItems (object sender, System.EventArgs e) {
        TargetEventArgs _e = (TargetEventArgs) e;
        if (_e._target == gameObject) {
            observer.dispatch (EventEnum.ScoreAdd, gameObject, e);
            // FIXME 抽到音效管理去播放
            destoryAudio.Play ();
            Destroy (gameObject);
        }
    }
}