using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SongManager : MonoBehaviour
{
    [SerializeField] private BoxCollider headHitCollider;
    [SerializeField] private BoxCollider leftHandHitCollider;
    [SerializeField] private BoxCollider rightHandHitCollider;

    [SerializeField] private BoxCollider headCollider;
    [SerializeField] private BoxCollider leftHandCollider;
    [SerializeField] private BoxCollider rightHandCollider;

    [SerializeField] private float maxHitDistance;

    private float beat;
    private float beatStep;
    [SerializeField] private float hitTime;
    private int beatIdx;
    private int totalBeats;

    private int score;

    private List<ChartData> chart;
    private List<int> beats;

    private bool isPlaying;
    private bool shouldCheckForHit;

    private Coroutine resetCollidersCoroutine;

    private void Start()
    {
        ReloadSongs();
    }

    private void Update()
    {
        if (!shouldCheckForHit) return;

        float headDist = Vector3.Distance(headHitCollider.transform.position, headCollider.transform.position);

        if (!(headDist <= maxHitDistance)) return;
        float lHandDist = Vector3.Distance(leftHandHitCollider.transform.position, leftHandCollider.transform.position);

        if (!(lHandDist <= maxHitDistance)) return;
        float rHandDist = Vector3.Distance(rightHandHitCollider.transform.position, rightHandCollider.transform.position);

        if (!(rHandDist <= maxHitDistance)) return;

        float headPoints = (1 - headDist) * 100;
        float lHandPoints = (1 - lHandDist) * 100;
        float rHandPoints = (1 - rHandDist) * 100;

        float points = headPoints + lHandPoints + rHandPoints;

        AddPoints(Mathf.RoundToInt(points));

        shouldCheckForHit = false;
    }

    public async void ReloadSongs()
    {
        if (SongReader.Songs.Count == 0)
            await SongReader.GetSongs();

        StartSong();
    }

    private void StartSong()
    {
        chart = SongReader.Songs[SongReader.selectedSongIdx].chart;

        foreach (ChartData chartBeat in chart)
        {
            beats.Add(chartBeat.beat);
        }

        beatStep = 60 / SongReader.Songs[SongReader.selectedSongIdx].bpm;

        StartCoroutine(BeatLoop());
    }

    private IEnumerator BeatLoop()
    {
        yield return new WaitForSeconds((SongReader.Songs[SongReader.selectedSongIdx].startDelay) / 1000);

        WaitForSeconds wait1Beat = new(beatStep);

        for (int i = 0; i < beats[^1]; i++)
        {
            if (beats[beatIdx] == i + 1)
            {
                SetColliders();
                beatIdx++;
            }
            beat++;
            yield return wait1Beat;
        }

        Debug.Log("Finished song.");
        PlayerPrefs.SetString("hs" + SongReader.Songs[SongReader.selectedSongIdx].songName, score.ToString());

        SwitchScene(0);
    }

    private void SetColliders()
    {
        if (resetCollidersCoroutine != null)
            StopCoroutine(resetCollidersCoroutine);
        headHitCollider.transform.position = chart[beatIdx].headPosV;
        leftHandHitCollider.transform.position = chart[beatIdx].leftHandPosV;
        rightHandHitCollider.transform.position = chart[beatIdx].rightHandPosV;
        shouldCheckForHit = true;
        resetCollidersCoroutine = StartCoroutine(ResetColliders());
    }

    private IEnumerator ResetColliders()
    {
        yield return new WaitForSeconds(hitTime);
        shouldCheckForHit = false;
        headHitCollider.transform.position = Vector3.zero;
        leftHandHitCollider.transform.position = Vector3.zero;
        rightHandHitCollider.transform.position = Vector3.zero;
    }

    private void AddPoints(int pointAmt)
    {
        score += pointAmt;
    }

    public void SwitchScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
