using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public GameObject rainEffect;   // Particle mưa
    public AudioSource rainSound;   // Âm thanh mưa
    public Light sunLight;          // Nguồn sáng chính (mặt trời)
    public float weatherDuration = 30f; // Thời gian 1 kiểu thời tiết (giây)

    private float timer;
    private enum WeatherType { Sunny, Rainy, Foggy }
    private WeatherType currentWeather;

    void Start()
    {
        SetWeather(WeatherType.Sunny); // Bắt đầu với trời nắng
        timer = weatherDuration;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            // Chuyển sang thời tiết khác ngẫu nhiên
            WeatherType newWeather = (WeatherType)Random.Range(0, 3);
            SetWeather(newWeather);

            timer = weatherDuration;
        }
    }

    void SetWeather(WeatherType type)
    {
        currentWeather = type;

        switch (type)
        {
            case WeatherType.Sunny:
                rainEffect.SetActive(false);
                rainSound.Stop();
                RenderSettings.fog = false;
                sunLight.intensity = 1f;
                break;

            case WeatherType.Rainy:
                rainEffect.SetActive(true);
                rainSound.Play();
                RenderSettings.fog = true;
                RenderSettings.fogDensity = 0.02f;
                sunLight.intensity = 0.5f;
                break;

            case WeatherType.Foggy:
                rainEffect.SetActive(false);
                rainSound.Stop();
                RenderSettings.fog = true;
                RenderSettings.fogDensity = 0.05f;
                sunLight.intensity = 0.6f;
                break;
        }
    }
}
