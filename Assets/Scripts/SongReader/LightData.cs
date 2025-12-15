using UnityEngine;

[System.Serializable]
public class LightData
{
    public int beat;
    public float[] position;
    public float[] rotation;
    public float[] color;
    public float intensity;

    public Vector3 pos => position.ToVector3();
    public Vector3 rot => rotation.ToVector3();
}
