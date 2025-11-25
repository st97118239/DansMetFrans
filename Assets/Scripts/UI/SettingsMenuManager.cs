using Unity.VisualScripting;
using UnityEngine;

public class SettingsMenuManager : MonoBehaviour
{
    [SerializeField] private Transform camTrans;

    private void Awake()
    {
        CalcHeight();
    }

    public void CalcHeight()
    {
        Settings.SetHeight(camTrans.localPosition.y + 1); // Adding + 1 due to the camTrans being from Camera Offset, which doesn't include the 1 meter from XR Origin
        Debug.Log(Settings.height);
    }
}
