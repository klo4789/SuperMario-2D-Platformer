using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleUI : MonoBehaviour
{
    [Header("설정")]
    public string gameSceneName = "MainScene";  // 실제 게임 씬 이름

    // "게임 시작" 버튼에서 호출
    public void OnClickStart()
    {
        Time.timeScale = 1f; // 혹시 멈춰 있을 수도 있으니 복구
        SceneManager.LoadScene(gameSceneName);
    }

    // ▶ "게임 종료" 버튼에서 호출
    public void OnClickQuit()
    {
#if UNITY_EDITOR
        // 에디터에서 테스트 할 때는 게임 중지
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 빌드된 실행파일에서는 앱 종료
        Application.Quit();
#endif
    }
}
