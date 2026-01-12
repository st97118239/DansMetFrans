using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoBehaviour
{
    [SerializeField] private SongManager songManager;
    [SerializeField] private RectTransform trans;
    [SerializeField] private Image image;
    public bool isInUse;

    [SerializeField] private float moveMaxDistance;
    [SerializeField] private float hiddenPosY = -500;

    private Vector3 posToMoveTo;
    private Vector3 hiddenPos;

    public void Load(PopupData popupData)
    {
        image.sprite = songManager.popupSprites[popupData.spriteIdx];
        posToMoveTo = popupData.posV;
        hiddenPos = posToMoveTo;
        hiddenPos.y = hiddenPosY;
        trans.anchoredPosition = hiddenPos;
        trans.localRotation = Quaternion.Euler(popupData.rotV);
        trans.localScale = popupData.scaleV;

        gameObject.SetActive(true);
        StartCoroutine(TurnOn(popupData.time));
    }

    private IEnumerator TurnOn(float time)
    {
        while (transform.localPosition != posToMoveTo)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, posToMoveTo, moveMaxDistance * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(time);

        while (transform.localPosition != hiddenPos)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, hiddenPos, moveMaxDistance * Time.deltaTime);
            yield return null;
        }
    }
}
