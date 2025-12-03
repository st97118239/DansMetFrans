using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField] private MainMenuManager mainMenuManager;
    [SerializeField] private Transform camTrans;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Awake()
    {
        CalcHeight();
        Settings.LoadVolume();
        musicSlider.value = Settings.musicVolume;
        sfxSlider.value = Settings.sfxVolume;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void BackButton()
    {
        gameObject.SetActive(false);
        Settings.SaveVolume();
        mainMenuManager.Show();
    }

    public void CalcHeight()
    {
        Settings.SetHeight(camTrans.localPosition.y + 1);
        Debug.Log(Settings.height);
    }

    public void UpdateMusicVolume()
    {
        Settings.musicVolume = musicSlider.value;
    }

    public void UpdateSfxVolume()
    {
        Settings.sfxVolume = sfxSlider.value;
    }

    public void ResetData()
    {
        PlayerPrefs.DeleteAll();
        Settings.ResetSettings();
        SongReader.ResetData();
        SceneManager.LoadScene(0);
    }
}
