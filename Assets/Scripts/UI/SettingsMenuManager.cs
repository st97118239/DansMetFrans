using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField] private MainMenuManager mainMenuManager;
    [SerializeField] private PauseScreenManager pauseScreenManager;
    [SerializeField] private SongManager songManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Transform camTrans;

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Toggle showPrevsToggle;

    public void Load()
    {
        camTrans = GameObject.Find("Camera Offset").transform;
        if (mainMenuManager != null)
        {
            //camTrans.localPosition += Vector3.up; 
            CalcHeight();
        }
        else
        {
            Debug.Log(Settings.height);
            //camTrans.localPosition = new Vector3(camTrans.localPosition.z, Settings.height, camTrans.localPosition.z);
        }

        Settings.LoadSettings();
        musicSlider.value = Settings.musicVolume;
        sfxSlider.value = Settings.sfxVolume;
        showPrevsToggle.isOn = Settings.showPreviews;
        if (songManager)
            songManager.OnUpdateSettings();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void MainMenuBackButton()
    {
        gameObject.SetActive(false);
        Settings.SaveSettings();
        mainMenuManager.Show();
    }

    public void SongPlayerBackButton()
    {
        Settings.SaveSettings();
        pauseScreenManager.OnSettings();
        gameObject.SetActive(false);
    }

    public void CalcHeight()
    {
        Settings.SetHeight(camTrans.localPosition.y);
    }

    public void ResetHeight()
    {
        CalcHeight();
        songManager.ReloadHeight();
    }

    public void UpdateShowPrevs()
    {
        Settings.showPreviews = showPrevsToggle.isOn;
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
