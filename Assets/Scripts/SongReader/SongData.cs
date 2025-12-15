using System.Collections.Generic;

[System.Serializable]
public class SongData
{
    public string songName;
    public int idx;
    public int performerIdx;
    public float bpm;
    public float chartStartDelay;
    public float audioStartDelay;
    public List<ChartData> chart;
    public DataLight[] lights;
    public PopupData[] popups;
}