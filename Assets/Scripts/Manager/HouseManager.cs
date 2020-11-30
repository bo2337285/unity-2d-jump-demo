using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseManager : MonoBehaviour {

    public GameObject dialog;
    private void OnTriggerEnter2D (Collider2D other) {
        if (other.tag == "Player") {
            dialog.SetActive (true);
        }
    }

    private void OnTriggerExit2D (Collider2D other) {
        if (other.tag == "Player") {
            dialog.SetActive (false);
        }
    }
}