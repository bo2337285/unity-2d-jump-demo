using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils {
    public static Boolean isBeTrampleOn (Vector2 contactPoint, GameObject beTrampler) {
        Vector2[] allPoints = GetBoxPoints (beTrampler.GetComponent<Collider2D> ());
        // player-敌人向量与x轴夹角
        float angle = getAngle (contactPoint, beTrampler.transform.position);
        // 敌人碰撞体的左上角/右上角-中心与x轴的夹角
        float angleMin = getAngle (allPoints[0], beTrampler.transform.position);
        float angleMax = getAngle (allPoints[1], beTrampler.transform.position);
        return (angle >= angleMin && angle <= angleMax);
    }
    // 判断目标对于当前对象的方向, Direction.x : 1 右, -1 左 , Direction.y : 1 上, -1 下
    public static Vector2 getDirection (Transform self, Transform target) {
        Vector3 direction = Vector3.Cross (
            self.TransformDirection (self.forward), // 将 transform.forward转为世界坐标向量
            target.position - self.position);
        return new Vector2 (direction.y > 0 ? 1 : -1, direction.x > 0 ? 1 : -1);
    }
    // 获取距离
    public static float getDistance (Vector2 from, Vector2 to) {
        return (from - to).sqrMagnitude;
    }

    // 获取两点连线与x轴夹角
    public static float getAngle (Vector2 from, Vector2 to) {
        Vector2 line = from - to;
        return Vector2.SignedAngle (line, Vector2.left);
    }

    // 获取2D碰撞体,4个边缘点坐标集
    public static Vector2[] GetBoxPoints (Collider2D collider) {

        Vector2[] allPoints = new Vector2[4];

        Quaternion quaternion = collider.transform.rotation;

        // BoxCollider2D boxCollider = null;
        // try {
        //     boxCollider = (BoxCollider2D) collider;
        // } catch {
        //     return new Vector2[0];
        // }
        // float sx = boxCollider.size.x;
        // float sy = boxCollider.size.y;
        Vector3 c = collider.bounds.center;
        float sx = collider.bounds.size.x;
        float sy = collider.bounds.size.y;
        Vector2 size = new Vector2 (
            sx * collider.transform.lossyScale.x,
            sy * collider.transform.lossyScale.y);
        float rx = size.x / 2f;
        float ry = size.y / 2f;

        // 依次是 左上,右上,左下,右下
        allPoints[0] = c + quaternion * new Vector2 (-rx, ry);
        allPoints[1] = c + quaternion * new Vector2 (rx, ry);
        allPoints[2] = c + quaternion * new Vector2 (-rx, -ry);
        allPoints[3] = c + quaternion * new Vector2 (rx, -ry);
        return allPoints;
    }

    // 满屏
    public static void fullScreen (Transform tf) {
        Camera cam = Camera.main;
        float pos = (cam.nearClipPlane + 0.01f);
        float h = (Mathf.Tan (cam.fieldOfView * Mathf.Deg2Rad * 0.5f) * pos * 2f) / 10.0f;
        tf.localScale = new Vector3 (h * 100 * 3.2f, 1.0f, h * cam.aspect * 100);
    }

    // 跟随相机
    public static void followCamera (Transform tf) {
        Camera mainCamera = Camera.main;
        tf.position = new Vector3 (mainCamera.transform.position.x, mainCamera.transform.position.y, tf.position.z);
    }
}