using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 5f;

    [Header("점프 설정")]
    public float jumpForce = 12f;

    [Header("Ground 체크")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.5f;
    public LayerMask groundLayer;

    [Header("코요테 타임")]
    public float coyoteTime = 0.1f;
    float coyoteTimeCounter;

    [Header("HP / 하트 UI")]
    public int maxHP = 3;
    public int currentHP;
    public Image heart1;
    public Image heart2;
    public Image heart3;

    [Header("피격 / 무적")]
    public float invincibleTime = 1.0f;
    public float knockbackForceX = 5f;
    public float knockbackForceY = 8f;
    public string enemyTag = "Enemy";

    [Header("함정(즉사)")]
    public string trapTag = "Trap";

    [Header("GameOver UI")]
    public GameObject gameOverPanel;

    // 내부 상태
    bool isGrounded;
    bool jumpPressed;
    float inputX;
    bool isInvincible = false;
    bool isDead = false;

    // 컴포넌트
    Rigidbody2D rb;
    Animator anim;
    SpriteRenderer sprite;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        currentHP = maxHP;
        UpdateHPUI();

        // 혹시 멈춰있을 수 있으므로 보정
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (isDead) return;

        inputX = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
            jumpPressed = true;

        // 좌우 반전
        if (inputX > 0.01f)
            sprite.flipX = false;
        else if (inputX < -0.01f)
            sprite.flipX = true;

        // 애니메이터 값 전달
        anim.SetFloat("Speed", Mathf.Abs(inputX));
        anim.SetBool("IsGrounded", isGrounded);
        anim.SetFloat("YVelocity", rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
        if (isDead) return;

        // Ground 체크
        isGrounded = Physics2D.OverlapCircle(
            groundCheck.position,
            groundCheckRadius,
            groundLayer
        );

        // 코요테 타임
        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.fixedDeltaTime;

        // 점프 처리
        if (jumpPressed && coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            coyoteTimeCounter = 0;
        }
        jumpPressed = false;

        // 좌우 이동
        rb.linearVelocity = new Vector2(inputX * moveSpeed, rb.linearVelocity.y);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //   1) Enemy 충돌 처리
        if (collision.collider.CompareTag(enemyTag))
        {
            if (isDead) return;

            ContactPoint2D contact = collision.GetContact(0);
            Transform enemyTr = collision.collider.transform;

            bool stomp =
                contact.point.y > enemyTr.position.y &&
                rb.linearVelocity.y <= 0f;

            if (stomp)
            {
                // 적 사망
                EnemyPatrol enemy = enemyTr.GetComponent<EnemyPatrol>();
                if (enemy != null) enemy.Die();

                // 자동 점프
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 0.8f);
            }
            else
            {
                // 데미지
                TakeDamage(enemyTr.position);
            }

            return;
        }

        //   2) Breakable Box 처리
        BreakableBox box = collision.collider.GetComponent<BreakableBox>();
        if (box != null)
        {
            foreach (var c in collision.contacts)
            {
                // 정상적으로 박스를 아래에서 쳤을 때
                if (c.normal.y < -0.5f)
                {
                    box.Break();
                    break;
                }
            }
        }
    }

    //   Trap 즉사 처리
    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag(trapTag))
        {
            currentHP = 0;
            UpdateHPUI();
            Die();
        }
    }

    //   데미지 처리
    public void TakeDamage(Vector3 enemyPos)
    {
        if (isInvincible || isDead) return;

        currentHP--;
        if (currentHP < 0) currentHP = 0;

        UpdateHPUI();

        if (currentHP <= 0)
        {
            Die();
            return;
        }

        StartCoroutine(InvincibleCoroutine());

        // 넉백
        float dir = (transform.position.x - enemyPos.x >= 0) ? 1 : -1;
        rb.linearVelocity = new Vector2(dir * knockbackForceX, knockbackForceY);
    }

    IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;
        float elapsed = 0f;
        float blinkInterval = 0.1f;

        while (elapsed < invincibleTime)
        {
            sprite.enabled = !sprite.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        sprite.enabled = true;
        isInvincible = false;
    }
    //     HP UI 관리
    void UpdateHPUI()
    {
        if (heart1 != null) heart1.enabled = currentHP >= 1;
        if (heart2 != null) heart2.enabled = currentHP >= 2;
        if (heart3 != null) heart3.enabled = currentHP >= 3;
    }

    //     사망 처리
    void Die()
    {
        if (isDead) return;
        isDead = true;

        rb.linearVelocity = Vector2.zero;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // 시간 정지 — UI는 동작 가능
        Time.timeScale = 0f;
    }
}
