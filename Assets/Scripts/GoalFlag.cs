using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalFlag : MonoBehaviour
{
    [Header("설정")]
    public string playerTag = "Player";   // 플레이어 태그
    public GameObject clearUI;            // 클리어 패널(버튼 포함)
    public string titleSceneName = "Title";  // 타이틀 씬 이름(인스펙터에서 수정 가능)

    bool isCleared = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isCleared) return;
        if (!other.CompareTag(playerTag)) return;

        isCleared = true;
        Debug.Log("Stage Clear!");

        // 클리어 UI 켜기
        if (clearUI != null)
            clearUI.SetActive(true);

        // 게임 일시정지
        Time.timeScale = 0f;
    }

    // ▶ 다시하기 버튼용
    public void OnClickRetry()
    {
        Time.timeScale = 1f;   // 시간 되살리고
        Scene current = SceneManager.GetActiveScene();
        SceneManager.LoadScene(current.buildIndex);  // 현재 스테이지 다시 로드
    }

    // ▶ 타이틀로 버튼용
    public void OnClickGoTitle()
    {
        Time.timeScale = 1f;   // 시간 되살리고
        if (!string.IsNullOrEmpty(titleSceneName))
        {
            SceneManager.LoadScene(titleSceneName);
        }
        else
        {
            Debug.LogWarning("GoalFlag: titleSceneName 이 비어 있습니다.");
        }
    }
}
