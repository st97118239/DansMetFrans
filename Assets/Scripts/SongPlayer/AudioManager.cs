using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicPlayer;
    public AudioSource sfxPlayer;

    [SerializeField] private AudioClip sfxSound;

    private void Start()
    {
        UpdateVolume();
    }

    public void UpdateVolume()
    {
        if (sfxPlayer)
            sfxPlayer.volume = Settings.sfxVolume;
        if (musicPlayer)
            musicPlayer.volume = Settings.musicVolume;
    }

    public void Load()
    {
        musicPlayer.clip = SongReader.Songs[SongReader.selectedSongIdx].audio;

        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        yield return new WaitForSeconds(SongReader.Songs[SongReader.selectedSongIdx].audioStartDelay);

        musicPlayer.Play();
    }

    public void PlaySFXSound()
    {
        sfxPlayer.PlayOneShot(sfxSound);
    }
}
