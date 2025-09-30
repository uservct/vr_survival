using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SurvivalManager : MonoBehaviour
{
    public static SurvivalManager instance;

    [Header("Tham chiếu tới người chơi")]
    public PlayerStats playerStats; // 👉 Kéo GameObject có script PlayerStats vào đây

    [Header("UI Thông báo")]
    public GameObject gameOverUI;
    public GameObject victoryUI;
    public TextMeshProUGUI dayText;

    [Header("Chiến thắng sau số ngày")]
    public int targetDaysToWin = 3;
    private int currentDay = 1;

    [Header("Chu kỳ 1 ngày (giây)")]
    public float dayDuration = 120f; // ví dụ: 120 giây = 1 ngày
    private float dayTimer = 0f;

    private bool isGameEnded = false;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        Time.timeScale = 1f; // đảm bảo game chạy bình thường
    }

    void Start()
    {
        dayTimer = dayDuration;
        UpdateDayText();

        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (victoryUI != null) victoryUI.SetActive(false);
    }

    void Update()
    {
        if (isGameEnded) return;

        // 🩸 Kiểm tra máu người chơi
        if (playerStats != null && playerStats.health <= 0f)
        {
            GameOver();
            return;
        }

        // ⏱️ Đếm thời gian để tăng ngày
        dayTimer -= Time.deltaTime;
        if (dayTimer <= 0f)
        {
            NextDay();
            dayTimer = dayDuration;
        }
    }

    // 📅 Sang ngày mới
    public void NextDay()
    {
        if (isGameEnded) return;

        currentDay++;
        UpdateDayText();

        Debug.Log($"🌅 Sang ngày {currentDay}");

        if (currentDay > targetDaysToWin)
        {
            Victory();
        }
    }

    // 💀 Khi người chơi chết
    public void GameOver()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        Debug.Log("💀 Game Over - Người chơi đã chết!");
        if (gameOverUI != null) gameOverUI.SetActive(true);

        Time.timeScale = 0f;
    }

    // 🏆 Khi sống đủ ngày
    public void Victory()
    {
        if (isGameEnded) return;
        isGameEnded = true;

        Debug.Log("🏆 Chiến thắng!");
        if (victoryUI != null) victoryUI.SetActive(true);

        Time.timeScale = 0f;
    }

    // 🔙 Gọi khi bấm nút quay về menu
    public void ReturnToMenuButton()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("menu");
    }

    // 🕒 Cập nhật text hiển thị ngày
    private void UpdateDayText()
    {
        if (dayText != null)
            dayText.text = $"Ngày: {currentDay}/{targetDaysToWin}";
    }
}
