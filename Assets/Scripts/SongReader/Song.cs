using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Song
{
    public string songName;
    public int idx;
    public Sprite coverArt;
    public Sprite titleArt;
    public AudioClip audio;
    public float bpm;
    public float chartStartDelay;
    public float audioStartDelay;
    public List<ChartData> chart;
    public DataLight[] lights;
}