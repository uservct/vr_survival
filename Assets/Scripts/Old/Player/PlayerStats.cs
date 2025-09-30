using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    [Header("UI Bars")]
    public Slider healthBar;
    public Slider hungerBar;
    public Slider thirstBar;

    [Header("Values")]
    public float health = 100f;
    public float hunger = 100f;
    public float thirst = 100f;

    [Header("Rates")]
    public float hungerDecreaseRate = 10f; // mỗi 10 giây giảm 1
    public float thirstDecreaseRate = 5f;  // mỗi 5 giây giảm 1
    public float healthDecreaseRate = 1f;  // máu giảm khi đói/khát

    [Header("Rain Effect")]
    public float rainHealthDecreaseRate = 2f; // máu mất mỗi giây khi trời mưa
    public Weather weatherSystem;             // tham chiếu script Weather

    private float hungerTimer;
    private float thirstTimer;

    void Start()
    {
        if (healthBar) healthBar.maxValue = 100;
        if (hungerBar) hungerBar.maxValue = 100;
        if (thirstBar) thirstBar.maxValue = 100;

        UpdateUI();
    }

    void Update()
    {
        // Giảm dần Hunger
        hungerTimer += Time.deltaTime;
        if (hungerTimer >= hungerDecreaseRate)
        {
            hunger = Mathf.Max(0, hunger - 1);
            hungerTimer = 0f;
        }

        // Giảm dần Thirst
        thirstTimer += Time.deltaTime;
        if (thirstTimer >= thirstDecreaseRate)
        {
            thirst = Mathf.Max(0, thirst - 1);
            thirstTimer = 0f;
        }

        // Nếu hunger hoặc thirst = 0 → giảm máu
        if (hunger <= 0 || thirst <= 0)
        {
            TakeDamage(healthDecreaseRate * Time.deltaTime);
        }

        // Nếu trời mưa → giảm máu thêm
        if (weatherSystem != null && weatherSystem.currentWeather == WeatherState.Rain)
        {
            TakeDamage(rainHealthDecreaseRate * Time.deltaTime);
        }

        UpdateUI();
    }

    void UpdateUI()
    {
        if (healthBar) healthBar.value = health;
        if (hungerBar) hungerBar.value = hunger;
        if (thirstBar) thirstBar.value = thirst;
    }

    public void AddHunger(float amount) => hunger = Mathf.Min(100, hunger + amount);
    public void AddThirst(float amount) => thirst = Mathf.Min(100, thirst + amount);
    public void Heal(float amount) => health = Mathf.Min(100, health + amount);

    public void TakeDamage(float amount)
    {
        health = Mathf.Max(0, health - amount);
        if (health <= 0)
        {
            Debug.Log("💀 Người chơi đã chết!");
        }
    }
}
