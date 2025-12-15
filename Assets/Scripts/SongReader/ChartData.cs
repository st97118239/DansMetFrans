using UnityEngine;

[System.Serializable]
public class ChartData
{
    public int beat;
    public float[] headPos;
    public float[] leftHandPos;
    public float[] rightHandPos;

    public Vector3 headPosV => headPos.ToVector3();
    public Vector3 leftHandPosV => leftHandPos.ToVector3();
    public Vector3 rightHandPosV => rightHandPos.ToVector3();
}

public static class VectorExtensions
{
    public static Vector3 ToVector3(this float[] floatValues)
    {
        if (floatValues != null)
        {
            if (floatValues.Length == 3)
                return new Vector3(floatValues[0], floatValues[1], floatValues[2]);
        }

        Debug.LogError("Invalid vector found.");
        return Vector3.zero;
    }
}