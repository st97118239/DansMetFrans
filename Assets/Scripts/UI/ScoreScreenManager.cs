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

            rankIdx = i;
            break;
        }

        rankText.text = rankIdx switch
        {
            0 => "D",
            1 => "C",
            2 => "B",
            3 => "A",
            4 => "S",
            5 => "SS",
            _ => rankText.text
        };

        gameObject.SetActive(true);
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene(mainMenuSceneIdx);
    }
}
