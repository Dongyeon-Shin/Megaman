using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("컴포넌트")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] GameObject[] bulletPrefabs;

    [Header("이동 데이터")]
    [SerializeField] Vector2 moveDir;
    [SerializeField] float moveSpeed, jumpPower, highSpeed, slidePower, climbSpeed;
    [SerializeField] bool onJump, onSlide, onLadder, onControl;

    enum BattleMode { Normal, Return, Bomb }
    [Header("전투 데이터")]
    [SerializeField] BattleMode modeNum;
    [SerializeField] Transform shotTransform;
    [SerializeField] float shotPower, hitCoolTime, knockBackPower;
    [SerializeField] bool isCoolTime, isHitable;

    [Header("이벤트")]
    [SerializeField] UnityEvent<int> OnHitEvent;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        onControl = true;
        isCoolTime = true;
        isHitable = true;
    }

    void Update()
    {
        CheckGround();
    }

    void FixedUpdate()
    {
        Move();
        Jump();
        Silde();
    }

    // 땅 판정
    void CheckGround()
    {
        Debug.DrawRay(transform.position, -transform.up, Color.green);
        if (Physics2D.Raycast(transform.position, -transform.up, 1f, LayerMask.GetMask("Ground")))
            animator.SetBool("IsGround", true);
        else
            animator.SetBool("IsGround", false);
    }

    // 물리 동작
    void Move()
    {
        if (onLadder)
        {
            if (animator.GetBool("IsClimb"))
            {
                LadderMove();
                return;
            }
            else if (moveDir.y != 0)
            {
                rb.gravityScale = 0f;
                animator.SetBool("IsClimb", true);
                animator.SetTrigger("DoClimb");
                LadderMove();
                return;
            }
        }

        if (animator.GetBool("IsGround"))
        {
            rb.AddForce(transform.right * moveDir.x * moveSpeed, ForceMode2D.Force);
            if (moveDir.x == 0)
                rb.velocity = new Vector2(0, rb.velocity.y);
        }
        else
            rb.AddForce(transform.right * moveDir.x * moveSpeed * 0.5f, ForceMode2D.Force);

        if (rb.velocity.x * moveDir.x > highSpeed)
            rb.velocity = new Vector2(moveDir.x * highSpeed, rb.velocity.y);
    }

    void Jump()
    {
        if (onJump)
        {
            rb.AddForce(transform.up * jumpPower, ForceMode2D.Impulse);
            onJump = false;
        }
    }

    void Silde()
    {
        if (onSlide)
        {
            rb.AddForce(transform.right * moveDir.x * slidePower, ForceMode2D.Impulse);
            onSlide = false;
        }
    }

    void LadderMove()
    {
        rb.velocity = transform.up * moveDir.y * climbSpeed;
    }

    // 플레이어 입력
    void OnMove(InputValue inputValue)
    {
        if (onControl)
        {
            moveDir = inputValue.Get<Vector2>();

            animator.SetInteger("MoveDir", (int)moveDir.x);

            if (moveDir.x < 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (moveDir.x > 0)
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void OnJump()
    {
        if (onControl && animator.GetBool("IsGround") && !onJump)
        {
            onJump = true;
        }
    }

    void OnSlide()
    {
        if (onControl && animator.GetBool("IsGround") && !onSlide)
        {
            animator.SetTrigger("DoSlide");
            onSlide = true;
        }
    }

    void OnShot()
    {
        if(onControl && isCoolTime)
        {
            animator.SetTrigger("DoShot");
            GameObject bullet = Instantiate(bulletPrefabs[(int)modeNum], shotTransform.position, Quaternion.identity);
            bullet.GetComponent<BulletDefault>().Shot((shotTransform.position - transform.position), shotPower);
            StartCoroutine(CoolTime());
        }
    }

    void OnChangeMode(InputValue inputValue)
    {
        if (onControl)
        {
            Vector2 input = inputValue.Get<Vector2>();
            if (input.x < 0)
            {
                if ((int)modeNum > 0)
                {
                    animator.SetLayerWeight((int)modeNum, 0);
                    modeNum = (BattleMode)((int)modeNum - 1);
                    animator.SetLayerWeight((int)modeNum, 1);
                }
                else
                {
                    animator.SetLayerWeight((int)modeNum, 0);
                    modeNum = (BattleMode)(GameManager.Data.Mode);
                    animator.SetLayerWeight((int)modeNum, 1);
                }
            }
            else if (input.x > 0)
            {
                if ((int)modeNum < GameManager.Data.Mode)
                {
                    animator.SetLayerWeight((int)modeNum, 0);
                    modeNum = (BattleMode)((int)modeNum + 1);
                    animator.SetLayerWeight((int)modeNum, 1);
                }
                else
                {
                    animator.SetLayerWeight((int)modeNum, 0);
                    modeNum = BattleMode.Normal;
                    animator.SetLayerWeight((int)modeNum, 1);
                }
            }
            animator.SetTrigger("DoChange");
        }
    }

    // 충돌 판정
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Ladder")
        {
            onLadder = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ladder")
        {
            onLadder = false;
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if ((collision.tag == "EnemyProjectile" || collision.tag == "Trap") && isHitable)
        {
            // 데미지 받아와서 발생 추가할 것
            OnHitEvent?.Invoke(1);
            Hit(collision.transform);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "EnemyProjectile" && isHitable)
        {
            // 데미지 받아와서 발생 추가할 것
            OnHitEvent?.Invoke(1);
            Hit(collision.transform);
        }
    }

    void Hit(Transform attackerTransform)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(((transform.position.x < attackerTransform.position.x ? -transform.right : transform.right) + transform.up) * knockBackPower, ForceMode2D.Impulse);
        StartCoroutine(HitCoolTime());
    }

    // 외부 호출
    public bool OnLadder { get { return animator.GetBool("IsClimb"); } }

    public void LadderOut()
    {
        animator.SetBool("IsClimb", false);
        rb.gravityScale = 1f;
    }

    public void Die()
    {
        onControl = false;
        isHitable = false;
        animator.SetLayerWeight((int)modeNum, 0);
        modeNum = BattleMode.Normal;
        animator.SetLayerWeight((int)modeNum, 1);
        animator.SetTrigger("DoDie");
    }

    public void Revival()
    {
        animator.SetTrigger("DoRevival");
        onControl = true;
        isHitable = true;
    }

    
    // 코루틴
    IEnumerator CoolTime()
    {
        isCoolTime = false;
        yield return new WaitForSeconds(bulletPrefabs[(int)modeNum].GetComponent<BulletDefault>().CoolTime);
        isCoolTime = true;
    }

    IEnumerator HitCoolTime()
    {
        isHitable = false;
        yield return new WaitForSeconds(hitCoolTime);
        isHitable = true;
    }
}
