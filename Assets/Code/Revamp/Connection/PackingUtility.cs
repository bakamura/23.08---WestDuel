using System.Net; // I hate this
using UnityEngine;

public static class PackingUtility {

    public static Vector2 FloatArrayToVector2(float[] floatArray) {
        if (floatArray.Length != 2) {
            Debug.LogError($"Tryng to convert float[] of length {floatArray.Length} into Vector2!");
            return Vector2.zero;
        }
        return new Vector2(floatArray[0], floatArray[1]);
    }

    public static float[] Vector2ToFloatArray(Vector2 vector2) { return new float[2] { vector2[0], vector2[1] }; }

    public static Vector3 FloatArrayToVector3(float[] floatArray) {
        if (floatArray.Length != 3) {
            Debug.LogError($"Tryng to convert float[] of length {floatArray.Length} into Vector3!");
            return Vector3.zero;
        }
        return new Vector3(floatArray[0], floatArray[1], floatArray[2]);
    }

    public static float[] Vector3ToFloatArray(Vector3 vector3) { return new float[3] { vector3[0], vector3[1], vector3[2] }; }

    // I hate this
    public static IPEndPoint StringToIPEndPoint(string str) {
        string[] strSplit = str.Split(':');
        return new IPEndPoint(IPAddress.Parse(strSplit[0]), int.Parse(strSplit[1])); 
    }
    // END I hate this

}
