using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreScreenManager : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private TMP_Text rankText;

    [SerializeField] private int[] rankPoints;

    [SerializeField] private int mainMenuSceneIdx;

    public void Show(int points)
    {
        pointsText.text = points.ToString();

        int maxRankIdx = rankPoints.Length - 1;
        int rankIdx = 0;

        for (int i = 0; i < maxRankIdx; i++)
        {
            if (points > rankPoints[i])
                continue;
            else
            {
                rankIdx = i;
                break;
            }
        }

        switch (rankIdx)
        {
            case 0:
                rankText.text = "D";
                break;
            case 1:
                rankText.text = "C";
                break;
            case 2:
                rankText.text = "B";
                break;
            case 3:
                rankText.text = "A";
                break;
            case 4:
                rankText.text = "S";
                break;
            case 5:
                rankText.text = "SS";
                break;
        }

        gameObject.SetActive(true);
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene(mainMenuSceneIdx);
    }
}
