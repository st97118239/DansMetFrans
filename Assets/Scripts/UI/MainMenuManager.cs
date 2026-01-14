using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private SongSelectManager songSelectManager;
    [SerializeField] private SettingsMenuManager settingsMenuManager;

    private void Awake()
    {
        GameObject.Find("Right Ray Interactor").SetActive(true);
        GameObject.Find("Left Ray Interactor").SetActive(true);

        settingsMenuManager.Load();
        Settings.LoadSettings();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void ButtonStart()
    {
        gameObject.SetActive(false);
        songSelectManager.Show();
    }

    public void ButtonSettings()
    {
        gameObject.SetActive(false);
        settingsMenuManager.Show();
    }

    public void ButtonQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
