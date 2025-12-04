using UnityEngine;

public class BreakableBox : MonoBehaviour
{
    [Header("박스를 깼을 때 나올 코인 프리팹")]
    public GameObject coinPrefab;   // FruitCoin 프리팹을 넣어줄 자리
    public int coinCount = 1;       // 한 번에 몇 개 줄지 (기본 1개)

    public void Break()
    {
        // 코인 생성
        if (coinPrefab != null)
        {
            for (int i = 0; i < coinCount; i++)
            {
                Vector3 spawnPos = transform.position + Vector3.up * 0.5f;
                Instantiate(coinPrefab, spawnPos, Quaternion.identity);
            }
        }

        // TODO: 나중에 깨지는 사운드 / 파편 이펙트 추가 가능

        // 박스 제거
        Destroy(gameObject);
    }
}
