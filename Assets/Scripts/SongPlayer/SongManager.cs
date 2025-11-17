using System.Collections;
using System.Collections.Generic;
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

    public int score;

    public List<ChartData> chart;
    public List<float> beats;

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

        score += Mathf.RoundToInt(points);

        shouldCheckForHit = false;
    }

    public async void ReloadSongs()
    {
        await SongReader.GetSongs();

        chart = SongReader.Songs[0].chart;

        foreach (ChartData chartBeat in chart)
        {
            beats.Add(chartBeat.beat);
        }

        StartCoroutine(BeatLoop());
    }

    private IEnumerator BeatLoop()
    {
        WaitForSeconds wait1Beat = new(beatStep);

        foreach (float t in beats)
        {
            for (; beat - beatStep < t; beat += beatStep)
            {
                if (Mathf.Approximately(beat, t))
                {
                    SetColliders();
                }

                yield return wait1Beat;
            }

            beatIdx++;
        }

        Debug.Log("Finished loop.");
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
}
