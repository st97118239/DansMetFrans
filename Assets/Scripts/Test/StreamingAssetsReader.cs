using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class StreamingAssetsReader : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadFile());
    }

    private IEnumerator LoadFile()
    {
        string jsonData = "";
        string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "text.json");

        if (filePath.StartsWith("jar") || filePath.StartsWith("http"))
        {
            // Special case to access StreamingAsset content on Android and Web
            UnityWebRequest request = UnityWebRequest.Get(filePath);
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                jsonData = request.downloadHandler.text;
            }
        }
        else
        {
            // This is a regular file path on most platforms and in Play mode of the editor
            jsonData = System.IO.File.ReadAllText(filePath);
        }

        Debug.Log("Loaded JSON Data: " + jsonData);
    }
}


[System.Serializable]
public class SaveData
{
    public string text;
}