using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiUtils {
    public static Boolean isMouseInScreen () {
        Vector3 mousePosition = Input.mousePosition;
        return (mousePosition.x >= 0 && mousePosition.x <= Screen.width) && (mousePosition.y >= 0 && mousePosition.y <= Screen.height);
    }
    public static void printInfoInGUI (Transform tf, String[] infoList, float height = 10f, float margin = 2f) {
        GameObject obj = tf.gameObject;
        Vector3 localScale = tf.localScale;
        Vector3 size = obj.GetComponent<SpriteRenderer> ().bounds.size;
        // 屏幕坐标的y与世界坐标相反
        // FIXME y方向总是高一点,先暂时-2f靠近点得了
        Vector3 screenPos = Camera.main.WorldToScreenPoint (
            new Vector3 (tf.position.x - (localScale.x * size.x) / 2, (tf.position.y - 2f - (localScale.y * size.y) / 2) * -1, tf.position.z));
        for (var i = 0; i < infoList.Length; i++) {
            infoList[i] = "<color=#0000ff>" + infoList[i] + "</color>";
            GUI.Label (new Rect (screenPos.x, screenPos.y + (height * i) + margin, 200, 20), infoList[i]);
        }
    }
}