using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Time Settings")]
    [Range(0, 24)] public float timeOfDay = 12f;   // Giờ hiện tại
    public float dayLengthInMinutes = 2f;          // 1 ngày trong game dài bao nhiêu phút
    private float timeSpeed;                       // tốc độ chạy thời gian

    [Header("Lighting")]
    public Light sunLight;
    public Light moonLight;
    public Material skyboxBlendMat;

    [Header("Environment Colors")]
    public Color dayAmbient = new Color(1f, 0.95f, 0.85f);
    public Color nightAmbient = new Color(0.05f, 0.1f, 0.2f);

    void Start()
    {
        // Tốc độ chạy: 24h chia cho tổng số giây 1 ngày
        timeSpeed = 24f / (dayLengthInMinutes * 60f);

        if (skyboxBlendMat != null)
            RenderSettings.skybox = skyboxBlendMat;
    }

    void Update()
    {
        // Cập nhật thời gian
        timeOfDay += Time.deltaTime * timeSpeed;
        if (timeOfDay >= 24f) timeOfDay = 0f;

        UpdateLighting();
    }

    void UpdateLighting()
    {
        // blend từ 0-1
        float t = Mathf.InverseLerp(6f, 18f, timeOfDay); // ngày từ 6h-18h

        // Xoay mặt trời & mặt trăng
        float sunRot = (timeOfDay / 24f) * 360f - 90f;
        sunLight.transform.rotation = Quaternion.Euler(sunRot, 170f, 0f);
        moonLight.transform.rotation = Quaternion.Euler(sunRot + 180f, 170f, 0f);

        // Cường độ sáng
        sunLight.intensity = Mathf.Lerp(0f, 1.2f, t);
        moonLight.intensity = Mathf.Lerp(0.5f, 0f, t);

        // Ánh sáng môi trường
        RenderSettings.ambientLight = Color.Lerp(nightAmbient, dayAmbient, t);

        // Skybox blend
        if (skyboxBlendMat != null)
        {
            skyboxBlendMat.SetFloat("_Blend", t);
        }

        // Fog
        if (RenderSettings.fog)
        {
            RenderSettings.fogColor = Color.Lerp(nightAmbient, dayAmbient, t);
            RenderSettings.fogDensity = Mathf.Lerp(0.02f, 0.004f, t);
        }
    }
}
