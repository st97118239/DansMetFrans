using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SongManager : MonoBehaviour
{
    [SerializeField] private BoxCollider headHitCollider;
    [SerializeField] private BoxCollider leftHandHitCollider;
    [SerializeField] private BoxCollider rightHandHitCollider;

    [SerializeField] private GameObject headPrev;
    [SerializeField] private GameObject leftHandPrev;
    [SerializeField] private GameObject rightHandPrev;

    [SerializeField] private BoxCollider headCollider;
    [SerializeField] private BoxCollider leftHandCollider;
    [SerializeField] private BoxCollider rightHandCollider;

    [SerializeField] private float maxHitDistance;

    private float beatStep;
    [SerializeField] private float hitTime;
    private int beat;
    private int beatLoopIdx;
    [SerializeField] private int previewBeats;
    private int totalBeats;

    private int score;

    private List<ChartData> chart;
    private readonly List<int> beats = new();

    private bool isPlaying;
    private bool shouldCheckForHit;
    private bool hasPreview;

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
            beats.Add(chartBeat.beat);

        beatStep = 60 / SongReader.Songs[SongReader.selectedSongIdx].bpm;

        StartCoroutine(BeatLoop());
    }

    private IEnumerator BeatLoop()
    {
        yield return new WaitForSeconds(SongReader.Songs[SongReader.selectedSongIdx].startDelay / 1000);

        WaitForSeconds wait1Beat = new(beatStep);

        for (beatLoopIdx = 0; beatLoopIdx < beats[^1]; beatLoopIdx++)
        {
            if (beats[beat] == beatLoopIdx + 1)
            {
                SetColliders();
                beat++;
            }

            if (!hasPreview && beat <= beats.Count - 1)
            {
                float beatsTillHit = beatLoopIdx + 1 + previewBeats - beats[beat];
                if (beats.Count >= beat + 1 && beatsTillHit <= previewBeats && beatsTillHit > 0)
                    SetPreview();
            }

            yield return wait1Beat;
        }

        for (int i = 0; i < 5; i++)
            yield return wait1Beat;

        Debug.Log("Finished song.");

        if (score > PlayerPrefs.GetInt("hs" + SongReader.Songs[SongReader.selectedSongIdx].songName))
            PlayerPrefs.SetInt("hs" + SongReader.Songs[SongReader.selectedSongIdx].songName, score);

        SwitchScene(0);
    }

    private void SetColliders()
    {
        if (resetCollidersCoroutine != null)
            StopCoroutine(resetCollidersCoroutine);
        if (hasPreview)
            ResetPreview();
        headHitCollider.transform.position = chart[beat].headPosV;
        leftHandHitCollider.transform.position = chart[beat].leftHandPosV;
        rightHandHitCollider.transform.position = chart[beat].rightHandPosV;
        shouldCheckForHit = true;
        resetCollidersCoroutine = StartCoroutine(ResetColliders());
    }

    private void SetPreview()
    {
        hasPreview = true;
        headPrev.transform.position = chart[beat].headPosV;
        leftHandPrev.transform.position = chart[beat].leftHandPosV;
        rightHandPrev.transform.position = chart[beat].rightHandPosV; 
    }

    private IEnumerator ResetColliders()
    {
        yield return new WaitForSeconds(hitTime);
        shouldCheckForHit = false;
        headHitCollider.transform.position = Vector3.down;
        leftHandHitCollider.transform.position = Vector3.down;
        rightHandHitCollider.transform.position = Vector3.down;
    }

    private void ResetPreview()
    {
        hasPreview = false;
        headPrev.transform.position = Vector3.down;
        leftHandPrev.transform.position = Vector3.down;
        rightHandPrev.transform.position = Vector3.down;
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
