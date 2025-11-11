using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Application = UnityEngine.Application;

public class SongReaderManager : MonoBehaviour
{
    public List<Song> songs;

    public Image image;
    public AudioSource audioSource;

    public Image poseImage;

    private void Start()
    {
        StartCoroutine(LoadFile());
    }

    private IEnumerator LoadFile()
    {
        int idx = 0;
        string jsonData = "";

        var path = new DirectoryInfo(Application.streamingAssetsPath);

        // Find every folder in StreamingAssets
        DirectoryInfo[] paths = path.GetDirectories();
        foreach (DirectoryInfo dirInfo in paths)
        {
            bool hasSongData = false;
            bool hasCoverArt = false;
            bool hasSongAudio = false;
            bool hasChart = false;
            Song song = new();

            // Find every file in the folder and check if it has songData
            FileInfo[] fileInfo = dirInfo.GetFiles();
            foreach (FileInfo file in fileInfo)
            {
                if (file.Name.EndsWith(".meta")) continue;

                string filePath = file.DirectoryName + "\\" + file.Name;

                Sprite sprite = null;
                switch (file.Name)
                {
                    // songData handler
                    case "songData.json":
                    {
                        if (filePath.StartsWith("jar") || filePath.StartsWith("http"))
                        {
                            UnityWebRequest request = UnityWebRequest.Get(filePath);
                            yield return request.SendWebRequest();

                            if (request.result == UnityWebRequest.Result.Success)
                                jsonData = request.downloadHandler.text;
                        }
                        else
                            jsonData = File.ReadAllText(filePath);

                        hasSongData = true;
                        SongData songData = JsonUtility.FromJson<SongData>(jsonData);

                        song.songName = songData.songName;
                        song.artist = songData.artist;
                        song.bpm = songData.bpm;
                        if (songData.chart.Count > 0)
                        {
                            song.chart = songData.chart;
                            hasChart = true;
                        }

                        break;
                    }
                    // cover handler
                    case "cover.png":
                    {
                        using UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(filePath);
                        yield return uwr.SendWebRequest();

                        if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
                            Debug.Log(uwr.error);
                        else
                        {
                            Texture2D loadedTexture = DownloadHandlerTexture.GetContent(uwr);

                            sprite = Sprite.Create(loadedTexture,
                                new Rect(0, 0, loadedTexture.width, loadedTexture.height), Vector2.zero);

                            hasCoverArt = true;
                        }

                        if (hasCoverArt)
                            song.coverArt = sprite;
                        break;
                        }
                    // song handler
                    case "song.mp3":
                    {
                        using UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.MPEG);
                        yield return uwr.SendWebRequest();

                        if (uwr.result == UnityWebRequest.Result.ConnectionError || uwr.result == UnityWebRequest.Result.ProtocolError)
                            Debug.Log(uwr.error);
                        else
                        {
                            AudioClip songClip = DownloadHandlerAudioClip.GetContent(uwr);
                            song.audio = songClip;
                            hasSongAudio = true;
                        }

                        break;
                    }
                }

                if (!hasSongData) continue;

                songs.Add(song);

                if (hasCoverArt)
                    image.sprite = song.coverArt;
                else
                    Debug.LogWarning("No cover art found for " + song.songName);

                if (hasSongAudio)
                {
                    if (audioSource.clip) 
                        audioSource.Stop();

                    audioSource.clip = song.audio;
                    audioSource.Play();
                }
                else
                    Debug.LogWarning("No song audio found for " + song.songName);

                Debug.Log("Loaded song " + song.songName);

                if (hasChart)
                    poseImage.sprite = Resources.Load<Sprite>(song.chart[0].spriteDir);

                idx++;
            }
        }
    }
}
