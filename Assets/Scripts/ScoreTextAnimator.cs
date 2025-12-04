using System.Collections;
using UnityEngine;

public class ScoreTextAnimator : MonoBehaviour
{
    public float punchScale = 1.2f;   // 얼마나 크게 튈지
    public float duration = 0.15f;    // 커졌다 줄어드는 시간

    RectTransform rect;
    Coroutine routine;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    public void Play()
    {
        // 이미 애니메이션 중이면 멈추고 다시 시작
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(AnimRoutine());
    }

    IEnumerator AnimRoutine()
    {
        Vector3 original = Vector3.one;
        Vector3 big = Vector3.one * punchScale;

        float t = 0f;

        // 원래 → 크게
        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;
            rect.localScale = Vector3.Lerp(original, big, lerp);
            yield return null;
        }

        t = 0f;
        // 크게 → 원래
        while (t < duration)
        {
            t += Time.deltaTime;
            float lerp = t / duration;
            rect.localScale = Vector3.Lerp(big, original, lerp);
            yield return null;
        }

        rect.localScale = original;
        routine = null;
    }
}
