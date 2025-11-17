using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Song
{
    public string songName;
    public Sprite coverArt;
    public AudioClip audio;
    public float bpm;
    public float startDelay;
    public List<ChartData> chart;
}