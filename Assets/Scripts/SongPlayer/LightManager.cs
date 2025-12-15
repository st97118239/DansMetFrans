using System.Threading.Tasks;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    [SerializeField] private GameObject lightPrefab;

    [SerializeField] private Light[] lights;
    [SerializeField] private DataLight[] lightData;

    public void LoadLights()
    {
        Debug.Log(SongReader.Songs[SongReader.selectedSongIdx].songName);

        if (SongReader.Songs[SongReader.selectedSongIdx].lights == null)
        {
            Debug.LogWarning("No lights were found on " + SongReader.Songs[SongReader.selectedSongIdx].songName);
            return;
        }

        lightData = SongReader.Songs[SongReader.selectedSongIdx].lights;

        int lightCount = SongReader.Songs[SongReader.selectedSongIdx].lights.Length;

        lights = new Light[lightCount];
        
        for (int i = 0; i < lightCount; i++)
        {
            Light newLight = Instantiate(lightPrefab, lightData[i].songLightData[0].position, Quaternion.Euler(lightData[i].songLightData[0].rotation)).GetComponent<Light>();
            lights[i] = newLight;
            newLight.color = lightData[i].songLightData[0].color;
            newLight.intensity = lightData[i].songLightData[0].intensity;
        }
    }
}
