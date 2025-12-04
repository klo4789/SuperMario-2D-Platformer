using UnityEngine;

public class FruitCoin : MonoBehaviour
{
    [Header("Score")]
    public int scoreValue = 5;          // 코인 1개당 점수

    [Header("Motion (마리오식 둥둥 코인)")]
    public float floatAmplitude = 0.1f; // 위아래 이동량
    public float floatFrequency = 2f;   // 위아래 움직이는 속도
    public float rotateSpeed = 0f;      // Y축 회전 속도 (필요 없으면 0)

    Vector3 startPos;

    void Start()
    {
        // 시작 위치 기억 (여기를 기준으로 위아래 움직임)
        startPos = transform.position;
    }

    void Update()
    {
        // 위아래로 둥둥
        Vector3 pos = startPos;
        pos.y += Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = pos;

        // 필요하면 살짝 회전 (2D라 안 돌리고 싶으면 rotateSpeed = 0으로 두기)
        if (rotateSpeed != 0f)
        {
            transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어만 먹을 수 있게
        if (!other.CompareTag("Player"))
            return;

        // 점수 추가
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreValue);
        }

        // 코인 오브젝트 제거
        Destroy(gameObject);
    }
}
