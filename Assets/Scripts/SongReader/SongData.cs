using System.Collections.Generic;

[System.Serializable]
public class SongData
{
    public string songName;
    public float bpm;
    public float startDelay;
    public List<ChartData> chart;
}