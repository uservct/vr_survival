using UnityEngine;
using System.Collections;

public enum WeatherState { Clear, Rain }

public class Weather : MonoBehaviour
{
    [Header("Weather Effects")]
    [Tooltip("Prefab hiệu ứng mưa (ParticleSystem) có AudioSource mưa gắn sẵn")]
    public GameObject rainEffect;

    [Header("Ambient Sounds")]
    [Tooltip("Âm thanh môi trường khi trời quang (chim, gió nhẹ)")]
    public AudioSource ambientClear;

    [Tooltip("Âm thanh môi trường khi trời mưa (gió, ếch, nước nhỏ giọt)")]
    public AudioSource ambientRain;

    [Header("Weather Settings")]
    public WeatherState currentWeather = WeatherState.Clear;

    [Header("Random Settings")]
    [Tooltip("Thời gian tối thiểu giữa các lần kiểm tra thời tiết")]
    public float minInterval = 30f;

    [Tooltip("Thời gian tối đa giữa các lần kiểm tra thời tiết")]
    public float maxInterval = 60f;

    [Tooltip("Khoảng thời gian mưa kéo dài (min, max)")]
    public Vector2 rainDurationRange = new Vector2(20f, 40f);

    [Tooltip("Tỉ lệ xác suất mưa (0 = không bao giờ, 1 = luôn mưa)")]
    [Range(0f, 1f)] public float rainProbability = 0.4f;

    private AudioSource rainAudio;

    void Start()
    {
        // Lấy AudioSource từ object mưa nếu có
        if (rainEffect != null)
            rainAudio = rainEffect.GetComponent<AudioSource>();

        // Cập nhật thời tiết ban đầu
        UpdateWeather(currentWeather);

        // Bắt đầu vòng lặp thay đổi thời tiết tự động
        StartCoroutine(AutoWeatherCycle());
    }

    IEnumerator AutoWeatherCycle()
    {
        while (true)
        {
            // ⏳ Chờ 1 khoảng ngẫu nhiên
            float waitTime = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(waitTime);

            // 🎲 Xác định có mưa hay không
            bool shouldRain = Random.value < rainProbability;

            if (shouldRain && currentWeather == WeatherState.Clear)
            {
                SetWeather(WeatherState.Rain);

                float rainTime = Random.Range(rainDurationRange.x, rainDurationRange.y);
                yield return new WaitForSeconds(rainTime);

                SetWeather(WeatherState.Clear);
            }
        }
    }

    public void SetWeather(WeatherState newWeather)
    {
        currentWeather = newWeather;
        UpdateWeather(newWeather);
    }

    void UpdateWeather(WeatherState weather)
    {
        switch (weather)
        {
            case WeatherState.Clear:
                // ☀️ Trời quang
                if (rainEffect) rainEffect.SetActive(false);
                if (rainAudio && rainAudio.isPlaying) rainAudio.Stop();

                if (ambientClear && !ambientClear.isPlaying) ambientClear.Play();
                if (ambientRain && ambientRain.isPlaying) ambientRain.Stop();

                // Sương mù nhẹ khi trời quang
                RenderSettings.fog = true;
                // Sương mù xanh nhẹ
                RenderSettings.fogColor = new Color(0.6f, 0.75f, 0.9f);              
                RenderSettings.fogDensity = 0.002f; // mờ nhẹ

                Debug.Log("🌤️ Thời tiết: Trời quang");
                break;

            case WeatherState.Rain:
                // 🌧️ Trời mưa
                if (rainEffect) rainEffect.SetActive(true);
                if (rainAudio && !rainAudio.isPlaying) rainAudio.Play();

                if (ambientRain && !ambientRain.isPlaying) ambientRain.Play();
                if (ambientClear && ambientClear.isPlaying) ambientClear.Stop();

                // Sương mù dày khi trời mưa
                RenderSettings.fog = true;
                RenderSettings.fogColor = new Color(0.4f, 0.4f, 0.45f); // màu tối xám
                RenderSettings.fogDensity = 0.01f; // dày đặc hơn

                Debug.Log("🌧️ Thời tiết: Mưa");
                break;
        }
    }

}
