using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private SongManager songManager;

    [SerializeField] private GameObject lightPrefab;

    [SerializeField] private Light[] lights;
    [SerializeField] private DataLight[] lightData;

    private async void Awake()
    {
        if (SongReader.Songs.Count == 0)
            await SongReader.GetSongs();

        SongReader.selectedSongIdx++;

        LoadLights();
    }

    private void LoadLights()
    {
        Debug.Log(SongReader.Songs[SongReader.selectedSongIdx].songName);

        if (SongReader.Songs[SongReader.selectedSongIdx].lights == null)
        {
            Debug.LogWarning("No lights were found on " + SongReader.Songs[SongReader.selectedSongIdx].songName);
            return;
        }

        int lightCount = SongReader.Songs[SongReader.selectedSongIdx].lights.Length;

        lights = new Light[lightCount];

        var songLightData = SongReader.Songs[SongReader.selectedSongIdx].lights;

        lightData = new DataLight[songLightData.Length];

        for (int i = 0; i < lightData.Length; i++)
        {
            lightData[i] = new DataLight
            {
                songLightData = new SongLights[songLightData[i].lightData.Length]
            };
            Debug.Log("a");
        }

        for (int i = 0; i < songLightData.Length; i++)
        {
            for (int j = 0; j < songLightData[i].lightData.Length; j++)
            {
                lightData[i].songLightData[j] = new SongLights
                {
                    beat = songLightData[i].lightData[j].beat,
                    position = songLightData[i].lightData[j].pos,
                    rotation = songLightData[i].lightData[j].rot,
                    intensity = songLightData[i].lightData[j].intensity,
                    color = new(songLightData[i].lightData[j].color[0], songLightData[i].lightData[j].color[1], songLightData[i].lightData[j].color[2])
                };
            }
        }

        for (int i = 0; i < lightCount; i++)
        {
            Light newLight = Instantiate(lightPrefab, lightData[i].songLightData[0].position, Quaternion.Euler(lightData[i].songLightData[0].rotation)).GetComponent<Light>();
            lights[i] = newLight;
            newLight.color = lightData[i].songLightData[0].color;
            newLight.intensity = lightData[i].songLightData[0].intensity;
        }
    }
}
