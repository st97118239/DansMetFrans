using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Temp : MonoBehaviour
{
    public Image image;
    public TMP_Text text;

    private void Start()
    {
        ReloadSongs();
    }

    public async void ReloadSongs()
    {
        await SongReader.GetSongs();

        foreach (Song song in SongReader.Songs)
        {
            Debug.Log("Found song " + song.songName);
        }

        if (SongReader.Songs[0] == null) return;

        image.sprite = SongReader.Songs[0].coverArt;
        text.text = SongReader.Songs[0].songName;
    }
}
