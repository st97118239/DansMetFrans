using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Song
{
    public string songName;
    public string artist;
    public Sprite coverArt;
    public AudioClip audio;
    public float bpm;
    public List<ChartData> chart;
}