using UnityEngine;

public class myVector3 {
    public float x;
    public float y;
    public float z;

    public myVector3() {}

    public myVector3(float x, float y, float z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public myVector3(Vector3 vector3) {
        this.x = vector3.x;
        this.y = vector3.y;
        this.z = vector3.z;
    }

    public float getX() {
        return x;
    }

    public float getY() {
        return y;
    }

    public float getZ() {
        return z;
    }

    public void setX(float x) {
        this.x = x;
    }

    public void setY(float y) {
        this.y = y;
    }

    public void setZ(float z) {
        this.z = z;
    }

    // 其他可能的操作，例如向量加法、向量减法、点积、叉积等
    public string toJson() {
        return "{\"x\":" + x + ",\"y\":" + y + ",\"z\":" + z + "}";
    }
}

