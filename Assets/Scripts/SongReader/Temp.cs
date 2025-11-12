using UnityEngine;

public class Temp : MonoBehaviour
{
    public async void ReloadSongs()
    {
        await SongReader.GetSongs();

        foreach (Song song in SongReader.Songs)
        {
            Debug.Log("Found song " + song.songName);
        }
    }
}
