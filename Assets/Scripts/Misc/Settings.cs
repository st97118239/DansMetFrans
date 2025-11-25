using UnityEngine;

public static class Settings
{
    public static float height;
    public static float defaultHeight = 1.8f;
    public static float heightDiff;

    public static void SetHeight(float givenHeight)
    {
        height = givenHeight;
        heightDiff = defaultHeight - height;
    }
}
