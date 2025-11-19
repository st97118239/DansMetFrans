using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private SongSelectManager songSelectManager;

    public void ButtonStart()
    {
        gameObject.SetActive(false);
        songSelectManager.Show();
    }

    public void ButtonSettings()
    {
        Debug.Log("Settings");
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
