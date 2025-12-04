using UnityEngine;
using TMPro;   // TextMeshPro 사용

public class ScoreManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static ScoreManager Instance { get; private set; }

    [Header("UI")]
    public TextMeshProUGUI scoreText;          // 점수 텍스트(TMP)
    public ScoreTextAnimator scoreTextAnimator; // 점수 튀는 애니메이션

    [Header("Sound")]
    public AudioSource audioSource;            // 효과음 재생용
    public AudioClip coinClip;                 // 코인 먹는 소리

    int score = 0;

    private void Awake()
    {
        // 싱글톤 설정
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        // 씬 시작 시 UI 초기화
        UpdateScoreUI();
    }

    /// <summary>
    /// 점수 추가 (코인 먹을 때 호출)
    /// </summary>
    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
        PlayCoinSfx();

        // 점수 텍스트 통통 튀는 애니메이션
        if (scoreTextAnimator != null)
        {
            scoreTextAnimator.Play();
        }
    }

    /// <summary>
    /// 점수 텍스트 UI 갱신
    /// </summary>
    void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = $"Score : {score}";
        }
    }
    void PlayCoinSfx()
    {
        if (audioSource != null && coinClip != null)
        {
            audioSource.PlayOneShot(coinClip);
        }
    }

    public int CurrentScore => score;
}
