using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [Header("컴포넌트")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;

    [Header("데이터")]
    [SerializeField] Vector2 moveDir;
    [SerializeField] float moveSpeed, jumpPower, highSpeed, slidePower, climbSpeed;
    [SerializeField] bool onJump, onSlide, onLadder;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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
        moveDir = inputValue.Get<Vector2>();

        if (moveDir.x != 0)
            animator.SetBool("IsMove", true);
        else
            animator.SetBool("IsMove", false);

        if (moveDir.x < 0)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveDir.x > 0)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void OnJump()
    {
        if (animator.GetBool("IsGround") && !onJump)
        {
            onJump = true;
        }
    }

    void OnSlide()
    {
        if (animator.GetBool("IsGround") && !onSlide)
        {
            animator.SetTrigger("DoSlide");
            onSlide = true;
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

    // 외부 호출
    public bool OnLadder { get { return animator.GetBool("IsClimb"); } }

    public void LadderOut()
    {
        animator.SetBool("IsClimb", false);
        rb.gravityScale = 1f;
    }
}
