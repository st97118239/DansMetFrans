using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreScreenManager : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private Image rankImage;

    [SerializeField] private int[] rankPoints;

    [SerializeField] private int mainMenuSceneIdx;

    public void Show(int points, bool isNewHighScore)
    {
        pointsText.text = points.ToString();

        int maxRankIdx = rankPoints.Length - 1;
        int rankIdx = 0;

        for (int i = 0; i < maxRankIdx; i++)
        {
            if (points > rankPoints[i])
                continue;

            rankIdx = i - 1;
            break;
        }

        string rank = rankIdx switch
        {
            -1 or 0 => "D",
            1 => "C",
            2 => "B",
            3 => "A",
            4 => "S",
            5 => "SS",
            _ => string.Empty
        };

        if (isNewHighScore) 
            PlayerPrefs.SetString("rank" + SongReader.Songs[SongReader.selectedSongIdx].songName, rank);

        rankImage.sprite = Resources.Load<Sprite>("Sprites/Ranks/" + rank);

        gameObject.SetActive(true);
    }

    public void ContinueButton()
    {
        SceneManager.LoadScene(mainMenuSceneIdx);
    }
}
