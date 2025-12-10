using UnityEngine;

public class PauseScreenManager : MonoBehaviour
{
    [SerializeField] private SongManager songManager;
    [SerializeField] private SettingsMenuManager settingsMenuManager;

    public void OnPause()
    {
        bool shouldPause = !gameObject.activeSelf;

        gameObject.SetActive(shouldPause);

        songManager.ShowIndicators(shouldPause);

        Time.timeScale = shouldPause ? 0 : 1;
    }

    public void OnSettings()
    {
        bool shouldOpenSettings = !settingsMenuManager.gameObject.activeSelf;

        gameObject.SetActive(!shouldOpenSettings);
        settingsMenuManager.Show();
    }

    public void OnQuit()
    {
        OnPause();
        songManager.OpenScoreScreen();
    }
}
