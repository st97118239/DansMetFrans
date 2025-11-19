using System.IO;
using UnityEngine;

public class ChartMakerData : MonoBehaviour
{
    public SongData songData;

    private static string Path => Application.persistentDataPath + "\\songData.json";

    public void SaveSong()
    {
        string json = JsonUtility.ToJson(songData, true);
        File.WriteAllText(Path, json);
    }
}
