using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Tên Scene chơi chính")]
    public string gameSceneName = "MAP3";

    [Header("Nút Quit")]
    public Button quitButton; // Kéo Button Quit vào đây
    public float delayEnable = 2f; // thời gian khóa 2 giây đầu

    void Start()
    {
        if (quitButton != null)
            quitButton.interactable = false; // khóa click

        Invoke(nameof(EnableQuitButton), delayEnable);
    }

    void EnableQuitButton()
    {
        if (quitButton != null)
            quitButton.interactable = true;
    }

    public void NewGame()
    {
        SceneManager.LoadScene(gameSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("❌ Thoát game!");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
