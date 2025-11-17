using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    public BoxCollider headHitCollider;
    public BoxCollider leftHandHitCollider;
    public BoxCollider rightHandHitCollider;

    public BoxCollider headCollider;
    public BoxCollider leftHandCollider;
    public BoxCollider rightHandCollider;

    [SerializeField] private float maxHitDistance;

    public float beat;
    [SerializeField] private float beatStep;
    [SerializeField] private float hitTime;
    private int beatIdx;
    private int totalBeats;

    public int score;

    public List<ChartData> chart;
    public List<int> beats;

    public bool isPlaying;
    public bool shouldCheckForHit;

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
}
