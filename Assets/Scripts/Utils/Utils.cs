using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils {
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

}