using UnityEngine;
using UnityEngine.SceneManagement; // cần cho LoadScene

public class MainMenu : MonoBehaviour
{
    // Gọi khi bấm nút New Game
    public void NewGame()
    {
        SceneManager.LoadScene("MAP3"); // thay "GameScene" bằng tên scene chơi game của bạn
    }

    // Gọi khi bấm nút Quit
    public void QuitGame()
    {
        Debug.Log("Thoát game..."); // để test trong Editor
        Application.Quit(); // chỉ thoát khi build ra .exe/.apk
    }
}
