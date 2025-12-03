using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using Application = UnityEngine.Application;

public static class SongReader
{
    public static IReadOnlyList<Song> Songs => songs;

    public static int selectedSongIdx = 0;

    private static List<Song> songs = new();

    public static async Task GetSongs()
    {
        string jsonData = "";

        var path = Application.streamingAssetsPath;

        string filePath = path + "/SongList.json";

        if (path.StartsWith("jar") || path.StartsWith("http"))
        {
            UnityWebRequest request = UnityWebRequest.Get(filePath);
            await request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
                jsonData = request.downloadHandler.text;
        }
        else
            jsonData = await File.ReadAllTextAsync(filePath);

        SongList songList = JsonUtility.FromJson<SongList>(jsonData);

        foreach (string songListPath in songList.songs)
        {
            jsonData = "";

            Song song = new();

            // songData handler
            string songPath = Application.streamingAssetsPath + songListPath + "/songData.json";

            if (songPath.StartsWith("jar") || songPath.StartsWith("http"))
            {
                UnityWebRequest request = UnityWebRequest.Get(songPath);
                await request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    jsonData = request.downloadHandler.text;
                }
            }
            else
                jsonData = await File.ReadAllTextAsync(songPath);

            SongData songData = JsonUtility.FromJson<SongData>(jsonData);

            if (songData.songName == string.Empty)
            {
                Debug.LogError("Song found with no name at " + songPath);
            }

            song.songName = songData.songName;
            song.bpm = songData.bpm;
            song.chartStartDelay = songData.chartStartDelay;
            song.audioStartDelay = songData.audioStartDelay;
            song.idx = songData.idx;
            if (songData.chart.Count > 0)
                song.chart = songData.chart;
            else
            {
                Debug.LogError("No chart found in song " + song.songName);
                continue;
            }

            // cover art handler
            songPath = path + songListPath + "/cover.png";
            Sprite sprite = null;

            using UnityWebRequest coverUwr = UnityWebRequestTexture.GetTexture(songPath);
            await coverUwr.SendWebRequest();

            if (coverUwr.result == UnityWebRequest.Result.ConnectionError || coverUwr.result == UnityWebRequest.Result.ProtocolError)
                Debug.Log(coverUwr.error);
            else
            {
                Texture2D loadedTexture = DownloadHandlerTexture.GetContent(coverUwr);

                sprite = Sprite.Create(loadedTexture,
                    new Rect(0, 0, loadedTexture.width, loadedTexture.height), Vector2.zero);
            }

            song.coverArt = sprite;

            // title art handler
            songPath = path + songListPath + "/title.png";
            sprite = null;

            using UnityWebRequest titleUwr = UnityWebRequestTexture.GetTexture(songPath);
            await titleUwr.SendWebRequest();

            if (titleUwr.result == UnityWebRequest.Result.ConnectionError || titleUwr.result == UnityWebRequest.Result.ProtocolError)
                Debug.Log(titleUwr.error);
            else
            {
                Texture2D loadedTexture = DownloadHandlerTexture.GetContent(titleUwr);

                sprite = Sprite.Create(loadedTexture,
                    new Rect(0, 0, loadedTexture.width, loadedTexture.height), Vector2.zero);
            }

            song.titleArt = sprite;

            // song audio handler
            songPath = path + songListPath + "/song.mp3";

            using UnityWebRequest audioUwr = UnityWebRequestMultimedia.GetAudioClip(songPath, AudioType.MPEG);
            await audioUwr.SendWebRequest();

            if (audioUwr.result == UnityWebRequest.Result.ConnectionError || audioUwr.result == UnityWebRequest.Result.ProtocolError)
                Debug.Log(audioUwr.error);
            else
            {
                AudioClip songClip = DownloadHandlerAudioClip.GetContent(audioUwr);
                song.audio = songClip;
            }

            songs.Add(song);
            Debug.Log("Loaded song " + song.songName);
        }

        songs = new List<Song>(songs.OrderBy(song => song.idx));
    }

    public static void ResetData()
    {
        songs.Clear();
        selectedSongIdx = 0;
    }
}
