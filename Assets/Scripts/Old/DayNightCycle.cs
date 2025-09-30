using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Skybox Settings")]
    public Material skyboxBlendMat; // Material sử dụng shader blend
    [Range(0, 24)] public float timeOfDay = 12f;
    public float dayLengthInMinutes = 1f;

    [Header("Lighting Settings")]
    public Light sunLight;
    public Light moonLight;

    private float timeSpeed;

    void Start()
    {
        timeSpeed = 24f / (dayLengthInMinutes * 60f);
        RenderSettings.skybox = skyboxBlendMat;
    }

    void Update()
    {
        // tăng thời gian
        timeOfDay += Time.deltaTime * timeSpeed;
        if (timeOfDay >= 24f) timeOfDay = 0f;

        UpdateLighting();
    }

    void UpdateLighting()
    {
        // blend dao động (0 = đêm, 1 = ngày)
        float blend = (1f - Mathf.Cos((timeOfDay / 24f) * Mathf.PI * 2f)) * 0.5f;

        // Gán blend cho skybox
        skyboxBlendMat.SetFloat("_Blend", blend);

        // Sun mạnh ban ngày, Moon mạnh ban đêm
        sunLight.intensity = Mathf.Lerp(0f, 1.2f, blend);  // ban ngày >1 cho sáng rõ
        moonLight.intensity = Mathf.Lerp(0f, 0.5f, 1f - blend);

        // Xoay mặt trời & mặt trăng
        float sunRotation = (timeOfDay / 24f) * 360f;
        sunLight.transform.rotation = Quaternion.Euler(new Vector3(sunRotation - 90f, 170f, 0));
        moonLight.transform.rotation = Quaternion.Euler(new Vector3(sunRotation - 270f, 170f, 0));

        // Ambient light
        RenderSettings.ambientLight = Color.Lerp(
            new Color(0.05f, 0.1f, 0.2f), // đêm xanh tối
            new Color(1f, 0.95f, 0.8f),   // ngày vàng sáng
            blend
        );

        // Nếu fog bật thì chỉnh lại cho hợp lý
        if (RenderSettings.fog)
        {
            RenderSettings.fogDensity = Mathf.Lerp(0.01f, 0.002f, blend);
        }
    }


}
