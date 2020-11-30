using System;
using UnityEngine;

public class DebugGamePlay : MonoBehaviour {
    private Vector3 mousePos = Vector3.zero;

    private void FixedUpdate () {
        mousePos = getMousePos ();
        mouseEvent ();
    }
    private void OnGUI () {
        // if (GUILayout.Button ("Add Enemy")) {
        //     Transform player = GameObject.Find ("Player").transform;
        //     Vector3 distance = new Vector3 (player.localScale.x * (player.forward.x + UnityEngine.Random.Range (0f, 30f)), 6, 0);
        //     addEnemy (player.position + distance);
        // }
        printGuiInfo ();
    }

    void printGuiInfo () {
        GUI.Label (new Rect (Screen.width - 200, Screen.height - 50, 200, 40), "x:" + mousePos.x + ", y:" + mousePos.y);
        GUI.Label (new Rect (Screen.width - 200, Screen.height - 100, 200, 40), "Input.mousePosition.x:" + Input.mousePosition.x +
            ",  Input.mousePosition.y:" + Input.mousePosition.y);
        GUI.Label (new Rect (Screen.width - 200, Screen.height - 150, 200, 40), "isMouseInScreen:" + GuiUtils.isMouseInScreen ().ToString ());
    }

    Vector3 getMousePos () {
        Vector3 viewPoint = Input.mousePosition;
        viewPoint.z -= Camera.main.transform.position.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint (viewPoint);
        return worldPos;
    }
    // 处理点击事件
    // FIXME 在OnGUI中灰触发两次
    void mouseEvent () {
        // 左键加敌人
        if (Input.GetMouseButtonUp (0) && GuiUtils.isMouseInScreen ()) {
            addEnemy ((Vector2) mousePos);
        }
        // 中键清除敌人
        if (Input.GetMouseButtonUp (2) && GuiUtils.isMouseInScreen ()) {
            clearEnemy ();
        }
    }

    void addEnemy (Vector2 pos) {
        GameObject enemy = GameObject.Find ("Enemy");
        if (enemy == null) {
            enemy = new GameObject ();
            enemy.name = "Enemy";
        }

        String filePath = "Prefabs/Enemy";
        GameObject prefabs = (GameObject) Resources.Load (filePath);
        Transform enemyTransform = prefabs.transform.GetChild (UnityEngine.Random.Range (0, prefabs.transform.childCount));
        Instantiate (enemyTransform.gameObject, pos, Quaternion.identity, enemy.transform);
    }
    void clearEnemy () {
        Destroy (GameObject.Find ("Enemy"));
    }
}