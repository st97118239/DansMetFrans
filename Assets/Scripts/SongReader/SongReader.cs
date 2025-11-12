using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Application = UnityEngine.Application;

public static class SongReader
{
    public static IReadOnlyList<Song> Songs => songs;

    private static List<Song> songs = new();

    public static async Task GetSongs()
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
                                await request.SendWebRequest();

                                if (request.result == UnityWebRequest.Result.Success)
                                    jsonData = request.downloadHandler.text;
                            }
                            else
                                jsonData = await File.ReadAllTextAsync(filePath);

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
                            await uwr.SendWebRequest();

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
                    // song audio handler
                    case "song.mp3":
                        {
                            using UnityWebRequest uwr = UnityWebRequestMultimedia.GetAudioClip(filePath, AudioType.MPEG);
                            await uwr.SendWebRequest();

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

                if (!hasChart)
                {
                    Debug.LogError("No chart found for " + song.songName);
                    continue;
                }

                if (!hasCoverArt)
                    Debug.LogWarning("No cover art found for " + song.songName);

                if (!hasSongAudio)
                    Debug.LogWarning("No song audio found for " + song.songName);

                songs.Add(song);
                Debug.Log("Loaded song " + song.songName);

                idx++;
            }
        }

    }
}
