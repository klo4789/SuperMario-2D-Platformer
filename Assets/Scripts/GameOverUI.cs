using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    // 다시하기 버튼
    public void OnClickRetry()
    {
        Time.timeScale = 1f;
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.name);
    }

    // 타이틀로 버튼
    public void OnClickTitle()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title");   // 타이틀 씬 이름과 같게
    }
}
