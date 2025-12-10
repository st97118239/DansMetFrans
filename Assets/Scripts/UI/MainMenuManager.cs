using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private SongSelectManager songSelectManager;
    [SerializeField] private SettingsMenuManager settingsMenuManager;

    private void Awake()
    {
        MeshRenderer[] handControllers = { GameObject.Find("Right Hand Controller").GetComponent<MeshRenderer>(), GameObject.Find("Left Hand Controller").GetComponent<MeshRenderer>(), };

        foreach (MeshRenderer hand in handControllers) 
            hand.enabled = false;

        GameObject.Find("Right Ray Interactor").SetActive(true);
        GameObject.Find("Left Ray Interactor").SetActive(true);

        settingsMenuManager.Load();
        Settings.LoadVolume();
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
//#elif UNITY_WEBPLAYER
//        Application.OpenURL(webplayerQuitURL);
#else
        Application.Quit();
#endif
    }
}
