using Unity.VisualScripting;
using UnityEngine;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField] private MainMenuManager mainMenuManager;
    [SerializeField] private Transform camTrans;

    private void Awake()
    {
        CalcHeight();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void BackButton()
    {
        gameObject.SetActive(false);
        mainMenuManager.Show();
    }

    public void CalcHeight()
    {
        Settings.SetHeight(camTrans.localPosition.y + 1);
        Debug.Log(Settings.height);
    }
}
