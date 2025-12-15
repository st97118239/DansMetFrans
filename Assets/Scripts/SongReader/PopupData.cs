using UnityEngine;

[System.Serializable]
public class PopupData
{
    public int beat;
    public int idx;
    public float[] pos;
    public float[] rot;
    public float[] scale;

    public Vector3 posV => pos.ToVector3();
    public Vector3 rotV => rot.ToVector3();
    public Vector3 scaleV => scale.ToVector3();
}