using System.Collections.Generic;

[System.Serializable]
public class SongData
{
    public string songName;
    public string artist;
    public float bpm;
    public List<ChartData> chart;
}