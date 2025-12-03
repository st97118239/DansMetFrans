using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicPlayer;

    public void Load()
    {
        musicPlayer.volume = Settings.musicVolume;

        musicPlayer.clip = SongReader.Songs[SongReader.selectedSongIdx].audio;

        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        yield return new WaitForSeconds(SongReader.Songs[SongReader.selectedSongIdx].chartStartDelay);

        musicPlayer.Play();
    }
}
