using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreScreenManager : MonoBehaviour
{
    [SerializeField] private TMP_Text pointsText;
    [SerializeField] private Image rankImage;

    [SerializeField] private int[] rankPoints;

    [SerializeField] private int rankAmt = 6;

    [SerializeField] private int mainMenuSceneIdx;

    public void Show(int points, bool isNewHighScore, int poseAmt, int hitAmt)
    {
        pointsText.text = points.ToString();

        int rankUpAmt = poseAmt / rankAmt;
        Debug.Log(rankUpAmt);

        int rankIdx = 0;
        while (hitAmt > rankUpAmt)
        {
            rankIdx++;
            hitAmt = -rankUpAmt;
        }

        Debug.Log(rankIdx);

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
