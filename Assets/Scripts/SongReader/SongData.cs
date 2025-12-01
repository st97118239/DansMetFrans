using System.Collections.Generic;

[System.Serializable]
public class SongData
{
    public string songName;
    public float bpm;
    public float chartStartDelay;
    public float audioStartDelay;
    public List<ChartData> chart;
}