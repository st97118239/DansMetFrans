using UnityEngine;

public static class Settings
{
    public static float height;
    public static float defaultHeight = 1.8f;
    public static float heightDiff;

    public static float musicVolume = 1;
    public static float sfxVolume = 1;

    public static void SetHeight(float givenHeight)
    {
        height = givenHeight;
        heightDiff = defaultHeight - height;
        Debug.Log(height + ", " + heightDiff);
    }

    public static void LoadVolume()
    {
        musicVolume = PlayerPrefs.GetFloat("musicVolume", 1);
        sfxVolume = PlayerPrefs.GetFloat("sfxVolume", 1);
    }

    public static void SaveVolume()
    {
        PlayerPrefs.SetFloat("musicVolume", musicVolume);
        PlayerPrefs.SetFloat("sfxVolume", sfxVolume);
    }

    public static void ResetSettings()
    {
        height = 0;
        heightDiff = 0;
        musicVolume = 1;
        sfxVolume = 1;
    }
}
