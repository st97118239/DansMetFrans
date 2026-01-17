using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SongManager : MonoBehaviour
{
    [SerializeField] private PauseScreenManager pauseScreenManager;
    [SerializeField] private ScoreScreenManager scoreScreenManager;
    [SerializeField] private SettingsMenuManager settingsMenuManager;
    [SerializeField] private LightManager lightManager;
    [SerializeField] private AudioManager audioManager;

    [SerializeField] private Transform headHitCollider;
    [SerializeField] private Transform leftHandHitCollider;
    [SerializeField] private Transform rightHandHitCollider;

    [SerializeField] private MeshRenderer headHitRenderer;
    [SerializeField] private MeshRenderer leftHandHitRenderer;
    [SerializeField] private MeshRenderer rightHandHitRenderer;

    [SerializeField] private GameObject headPrev;
    [SerializeField] private GameObject leftHandPrev;
    [SerializeField] private GameObject rightHandPrev;

    [SerializeField] private Transform headCollider;
    [SerializeField] private Transform leftHandCollider;
    [SerializeField] private Transform rightHandCollider;

    [SerializeField] private MeshRenderer leftHandRenderer;
    [SerializeField] private MeshRenderer rightHandRenderer;

    [SerializeField] private GameObject[] handIndicators;

    [SerializeField] private Transform camTrans;
    [SerializeField] private Transform[] objectsTrans;
    private Vector3[] defaultObjectsPos;

    [SerializeField] private Popup[] popups;
    public Sprite[] popupSprites;
    [SerializeField] private TMP_Text pointText;

    [SerializeField] private float maxHitDistance;

    [SerializeField] private GameObject[] performerPrefabs;
    [SerializeField] private Transform performerPos;
    [SerializeField] private Transform oldPerformerPos;
    private GameObject performer;
    private Animator performerAnimator;

    private float beatStep;
    [SerializeField] private float hitTime;
    private int beat;
    private int popupBeat;
    [SerializeField] private int beatLoopIdx;
    [SerializeField] private int previewBeats;

    public int score;
    private int hitAmt;

    private List<ChartData> chart;
    private PopupData[] popupChart;
    private readonly List<int> beats = new();

    private bool hasPreview;
    public bool hasFinished;
    private bool showPreviews = true;

    private Coroutine resetCollidersCoroutine;

    private void Start()
    {
        headCollider = GameObject.Find("Main Camera").transform;
        leftHandCollider = GameObject.Find("Left Hand Controller").transform;
        rightHandCollider = GameObject.Find("Right Hand Controller").transform;

        handIndicators = new[] { GameObject.Find("Right Ray Interactor"), GameObject.Find("Left Ray Interactor"), };

        foreach (GameObject indicator in handIndicators)
            indicator.SetActive(false);

        leftHandRenderer.enabled = true;
        rightHandRenderer.enabled = true;

        camTrans = GameObject.Find("Camera Offset").transform;

        pointText = GameObject.Find("PointText").GetComponent<TMP_Text>();

        settingsMenuManager.Load();
        OnUpdateSettings();

        defaultObjectsPos = new Vector3[objectsTrans.Length];

        for (int i = 0; i < objectsTrans.Length; i++)
            defaultObjectsPos[i] = objectsTrans[i].localPosition;

        ReloadSongs();
    }

    private async void ReloadSongs()
    {
        if (SongReader.Songs.Count == 0)
        {
            await SongReader.GetSongs();
        }

        //SongReader.selectedSongIdx = 2;

        lightManager.LoadLights();
        LoadPerformer();
        LoadPopups();
        StartSong();
    }

    private void LoadPerformer()
    {
        Vector3 pos = SongReader.Songs[SongReader.selectedSongIdx].useOldPerformerPos ? oldPerformerPos.position : performerPos.position;
        performer = Instantiate(performerPrefabs[SongReader.Songs[SongReader.selectedSongIdx].performerIdx],
                pos, performerPos.rotation);
        performerAnimator = performer.GetComponent<Animator>();
        performer.SetActive(true);
    }

    private void LoadPopups()
    {
        popupChart = SongReader.Songs[SongReader.selectedSongIdx].popups;

        if (popupChart == null) return;

        for (int t = 0; t < popupSprites.Length; t++)
        {
            Sprite tmp = popupSprites[t];
            int r = Random.Range(t, popupSprites.Length);
            popupSprites[t] = popupSprites[r];
            popupSprites[r] = tmp;
        }

        foreach (PopupData popup in popupChart)
            popups[popup.idx].Load(popup);
    }

    private void StartSong()
    {
        performer.SetActive(true);
        chart = SongReader.Songs[SongReader.selectedSongIdx].chart;

        foreach (ChartData chartBeat in chart)
            beats.Add(chartBeat.beat);

        beatStep = 60 / SongReader.Songs[SongReader.selectedSongIdx].bpm;

        StartCoroutine(BeatLoop());
        audioManager.Load();
    }

    private IEnumerator BeatLoop()
    {
        yield return new WaitForSeconds(SongReader.Songs[SongReader.selectedSongIdx].chartStartDelay);

        WaitForSeconds wait1Beat = new(beatStep);

        performerAnimator.SetTrigger("StartDance");

        for (beatLoopIdx = 0; beatLoopIdx < beats[^1]; beatLoopIdx++)
        {
            if (beats[beat] == beatLoopIdx + 1)
            {
                SetColliders();
                beat++;
            }

            if (showPreviews && !hasPreview && beat <= beats.Count - 1)
            {
                float beatsTillHit = beatLoopIdx + 1 + previewBeats - beats[beat];
                if (beats.Count >= beat + 1 && beatsTillHit <= previewBeats && beatsTillHit > 0)
                    SetPreview();
            }

            //if (popupBeats.Count > popupBeat && popupBeats[popupBeat] == beatLoopIdx + 1)
            //{
            //    popups[popupChart[popupBeat].idx].Load(popupChart[popupBeat]);
            //    popupBeat++;
            //}

            yield return wait1Beat;
        }

        for (int i = 0; i < 5; i++)
            yield return wait1Beat;

        hasFinished = true;

        OpenScoreScreen();
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

        // For chart testing
        //headCollider.transform.position = chart[beat].headPosV;
        //leftHandCollider.transform.position = chart[beat].leftHandPosV;
        //rightHandCollider.transform.position = chart[beat].rightHandPosV;

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
        CalculatePoints();
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

    private void CalculatePoints()
    {
        float headDist = Vector3.Distance(headHitCollider.position, headCollider.position);

        if (headDist <= maxHitDistance)
        {
            hitAmt++;
            float headPoints = (1 - headDist) * 100;
            AddPoints(Mathf.RoundToInt(headPoints));
        }

        float lHandDist = Vector3.Distance(leftHandHitCollider.position, leftHandCollider.position);

        if (lHandDist <= maxHitDistance)
        {
            hitAmt++;
            float lHandPoints = (1 - lHandDist) * 100;
            AddPoints(Mathf.RoundToInt(lHandPoints));
        }

        float rHandDist = Vector3.Distance(rightHandHitCollider.position, rightHandCollider.position);

        if (rHandDist <= maxHitDistance)
        {
            hitAmt++;
            float rHandPoints = (1 - rHandDist) * 100;
            AddPoints(Mathf.RoundToInt(rHandPoints));
        }

        pointText.text = score.ToString();
    }

    private void AddPoints(int pointAmt)
    {
        score += pointAmt;
    }

    public void OpenScoreScreen()
    {
        bool isNewHighScore = false;

        if (score > PlayerPrefs.GetInt("hs" + SongReader.Songs[SongReader.selectedSongIdx].songName))
        {
            isNewHighScore = true;
            PlayerPrefs.SetInt("hs" + SongReader.Songs[SongReader.selectedSongIdx].songName, score);
        }

        scoreScreenManager.Show(score, isNewHighScore, beats.Count * 3, hitAmt);

        ShowIndicators(true);
    }

    public void ShowIndicators(bool state)
    {
        foreach (GameObject obj in handIndicators)
        {
            obj.SetActive(state);
        }
    }

    private void OnPause()
    {
        if (!hasFinished)
            pauseScreenManager.OnPause();
    }

    public void ReloadHeight()
    {
        if (Settings.height == 0)
            Settings.SetHeight(camTrans.localPosition.y + 1);

        camTrans.localPosition = new Vector3(0, Settings.height, 0);

        for (int i = 0; i < objectsTrans.Length; i++)
        {
            Transform obj = objectsTrans[i];
            Vector3 defPos = defaultObjectsPos[i];
            obj.localPosition = new Vector3(defPos.x, defPos.y + Settings.heightDiff, defPos.z);
        }

        //foreach (Transform trans in objectsTrans)
        //    trans.localPosition = new Vector3(trans.localPosition.x, trans.localPosition.y + Settings.heightDiff, trans.localPosition.z);
    }

    public void OnUpdateSettings()
    {
        showPreviews = Settings.showPreviews;
        headHitRenderer.enabled = showPreviews;
        leftHandHitRenderer.enabled = showPreviews;
        rightHandHitRenderer.enabled = showPreviews;
    }
}