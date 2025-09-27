using UnityEngine;

public enum WeatherType { Clear, Rain }

public class WeatherManager : MonoBehaviour
{
    [Header("Weather Effects")]
    public GameObject rainEffect;
    public AudioSource rainSound;

    [Header("Settings")]
    public WeatherType currentWeather = WeatherType.Clear;

    void Start()
    {
        UpdateWeather(currentWeather);
    }

    void Update()
    {
        // Debug test: nhấn phím R để bật/tắt mưa
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentWeather == WeatherType.Clear)
                SetWeather(WeatherType.Rain);
            else
                SetWeather(WeatherType.Clear);
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
                rainEffect.SetActive(false);
                rainSound.Stop();
                RenderSettings.fog = false; // tắt sương mù
                break;

            case WeatherType.Rain:
                rainEffect.SetActive(true);
                if (!rainSound.isPlaying) rainSound.Play();
                RenderSettings.fog = true;
                RenderSettings.fogColor = new Color(0.4f, 0.4f, 0.45f);
                RenderSettings.fogDensity = 0.01f;
                break;
        }
    }
}
