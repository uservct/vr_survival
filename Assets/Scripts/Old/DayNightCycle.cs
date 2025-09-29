using UnityEngine;
using System.Collections;

public enum WeatherType { Clear, Rain }

public class WeatherManager : MonoBehaviour
{
    [Header("Weather Effects")]
    public GameObject rainEffect;
    private AudioSource rainAudio;

    [Header("Ambient Sounds")]
    public AudioSource ambientClear;
    public AudioSource ambientRain;

    [Header("Settings")]
    public WeatherType currentWeather = WeatherType.Clear;

    [Header("Random Settings")]
    public float minInterval = 30f;
    public float maxInterval = 60f;
    public Vector2 rainDurationRange = new Vector2(20f, 40f);
    [Range(0f, 1f)] public float rainProbability = 0.4f;

    void Start()
    {
        // ✅ Gộp tất cả logic vào đây
        if (rainEffect != null)
            rainAudio = rainEffect.GetComponent<AudioSource>();

        UpdateWeather(currentWeather);
        StartCoroutine(AutoWeatherCycle());
    }

    IEnumerator AutoWeatherCycle()
    {
        while (true)
        {
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            bool shouldRain = Random.value < rainProbability;

            if (shouldRain && currentWeather == WeatherType.Clear)
            {
                SetWeather(WeatherType.Rain);

                float rainTime = Random.Range(rainDurationRange.x, rainDurationRange.y);
                yield return new WaitForSeconds(rainTime);

                SetWeather(WeatherType.Clear);
            }
        }
    }

    public void SetWeather(WeatherType newWeather)
    {
        currentWeather = newWeather;
        UpdateWeather(newWeather);
    }

    void UpdateWeather(WeatherType weather)
    {
        switch (weather)
        {
            case WeatherType.Clear:
                if (rainEffect) rainEffect.SetActive(false);
                if (rainAudio && rainAudio.isPlaying) rainAudio.Stop();

                if (ambientClear && !ambientClear.isPlaying) ambientClear.Play();
                if (ambientRain && ambientRain.isPlaying) ambientRain.Stop();

                RenderSettings.fog = false;
                Debug.Log("🌤️ Thời tiết: Trời quang");
                break;

            case WeatherType.Rain:
                if (rainEffect) rainEffect.SetActive(true);
                if (rainAudio && !rainAudio.isPlaying) rainAudio.Play();

                if (ambientRain && !ambientRain.isPlaying) ambientRain.Play();
                if (ambientClear && ambientClear.isPlaying) ambientClear.Stop();

                RenderSettings.fog = true;
                RenderSettings.fogColor = new Color(0.4f, 0.4f, 0.45f);
                RenderSettings.fogDensity = 0.01f;
                Debug.Log("🌧️ Thời tiết: Mưa");
                break;
        }
    }
}
