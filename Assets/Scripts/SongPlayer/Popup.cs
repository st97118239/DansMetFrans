using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private SongManager songManager;
    [SerializeField] private RectTransform trans;
    [SerializeField] private Image image;

    public void Load(PopupData popupData)
    {
        image.sprite = songManager.popupSprites[Random.Range(0, songManager.popupSprites.Length - 1)];
        trans.position = popupData.posV; 
        trans.localRotation = Quaternion.Euler(popupData.rotV);
        trans.localScale = popupData.scaleV;

        gameObject.SetActive(true);
    }
}
