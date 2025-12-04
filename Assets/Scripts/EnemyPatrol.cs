using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    [Header("이동 경로")]
    public Transform leftPoint;     // 왼쪽 끝 지점
    public Transform rightPoint;    // 오른쪽 끝 지점

    [Header("이동 속도")]
    public float moveSpeed = 2f;

    [Header("초기 바라보는 방향")]
    public bool faceRightWhenMovingRight = false;
    // true  : 오른쪽으로 갈 때 오른쪽을 바라봄
    // false : 오른쪽으로 갈 때 왼쪽을 바라봄

    bool movingRight = true;

    Rigidbody2D rb;
    SpriteRenderer sprite;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {

    }

    void Update()
    {
        Patrol();
    }

    void Patrol()
    {
        if (leftPoint == null || rightPoint == null) return;

        // 현재 이동 방향
        float dir = movingRight ? 1f : -1f;

        // 이동
        rb.linearVelocity = new Vector2(dir * moveSpeed, rb.linearVelocity.y);

        // 방향 전환 (왼/오 끝 지점 도달 시)
        if (movingRight && transform.position.x >= rightPoint.position.x)
        {
            movingRight = false;
        }
        else if (!movingRight && transform.position.x <= leftPoint.position.x)
        {
            movingRight = true;
        }

        if (movingRight)
        {
            sprite.flipX = !faceRightWhenMovingRight;
        }
        else
        {
            sprite.flipX = faceRightWhenMovingRight;
        }
    }

    // 플레이어가 밟았을 때 호출되는 함수
    public void Die()
    {
        // 간단하게 Destroy
        Destroy(gameObject);
    }
}
