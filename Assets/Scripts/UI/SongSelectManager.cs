using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SongSelectManager : MonoBehaviour
{
    [SerializeField] private MainMenuManager mainMenuManager;

    [SerializeField] private Image coverImage;
    [SerializeField] private Image titleImage;
    [SerializeField] private TMP_Text highScoreText;

    [SerializeField] private int songPlayerSceneIdx;

    private int idx;
    private int maxIdx;
    
    public void Show()
    {
        LoadSongList();
    }

    public void BackButton()
    {
        gameObject.SetActive(false);
        mainMenuManager.Show();

    }

    public void ButtonNext()
    {
        idx++;
        if (idx > maxIdx) idx = 0;
        LoadSong();
    }

    public void ButtonPrev()
    {
        idx--;
        if (idx < 0) idx = maxIdx;
        LoadSong();
    }

    private void LoadSong()
    {
        coverImage.sprite = SongReader.Songs[idx].coverArt;
        titleImage.sprite = SongReader.Songs[idx].titleArt;

        string highScore = PlayerPrefs.GetInt("hs" + SongReader.Songs[idx].songName).ToString();

        if (highScore == string.Empty) 
            highScore = "Not yet played";

        highScoreText.text = highScore;
    }

    public void PlaySong()
    {
        SongReader.selectedSongIdx = idx;
        SceneManager.LoadScene(songPlayerSceneIdx);
    }

    public async void LoadSongList()
    {
        if (SongReader.Songs.Count == 0)
            await SongReader.GetSongs();

        foreach (Song song in SongReader.Songs)
        {
            Debug.Log("Found song " + song.songName);
        }

        maxIdx = SongReader.Songs.Count - 1;

        gameObject.SetActive(true);
        LoadSong();
    }
}
