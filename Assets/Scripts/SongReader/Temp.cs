using UnityEngine;
using UnityEngine.SceneManagement;

public class Temp : MonoBehaviour
{
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
    }

    public void SwitchScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
