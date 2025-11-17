using System;
using UnityEngine;

[System.Serializable]
public class ChartData
{
    public int beat;
    public float[] headPos;
    public float[] leftHandPos;
    public float[] rightHandPos;

    public Vector3 headPosV => headPos.ToVector3(beat);
    public Vector3 leftHandPosV => leftHandPos.ToVector3(beat);
    public Vector3 rightHandPosV => rightHandPos.ToVector3(beat);
}

public static class VectorExtensions
{
    public static Vector3 ToVector3(this float[] floatValues, float beat)
    {
        if (floatValues.Length == 3)
            return new Vector3(floatValues[0], floatValues[1], floatValues[2]);

        throw new ArgumentException("Invalid vector at beat " + beat);
    }
}