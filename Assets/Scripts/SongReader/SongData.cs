using System.Collections.Generic;

[System.Serializable]
public class SongData
{
    public string songName;
    public int idx;
    public float bpm;
    public float chartStartDelay;
    public float audioStartDelay;
    public List<ChartData> chart;
}