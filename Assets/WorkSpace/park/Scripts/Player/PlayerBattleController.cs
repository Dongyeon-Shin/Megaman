using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBattleController : MonoBehaviour
{

    [Header("컴포넌트")]
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] GameObject[] bulletPrefabs;

    enum BulletMode { Normal, Bomb, Return }
    [Header("데이터")]
    [SerializeField] BulletMode bulletNum;
    [SerializeField] Transform shotTransform;
    [SerializeField] float shotPower, hitCoolTime, knockBackPower;
    [SerializeField] bool coolTime, hitable;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        coolTime = true;
        hitable = true;
    }

    void OnShot()
    {
        animator.SetTrigger("DoShot");

        if (coolTime)
        {
            GameObject bullet = Instantiate(bulletPrefabs[(int)bulletNum], shotTransform.position, Quaternion.identity);
            bullet.GetComponent<BulletDefault>().Shot((shotTransform.position - transform.position), shotPower);
            StartCoroutine(CoolTime());
        }
    }

    void OnChangeMode(InputValue inputValue)
    {
        Vector2 input = inputValue.Get<Vector2>();
        if (input.x < 0)
        {
            if ((int)bulletNum > 0)
                bulletNum = (BulletMode)((int)bulletNum - 1);
            else
                bulletNum = BulletMode.Return;
        }
        else if (input.x > 0)
        {
            if ((int)bulletNum < 2)
                bulletNum = (BulletMode)((int)bulletNum + 1);
            else
                bulletNum = BulletMode.Normal;
        }
    }

    IEnumerator CoolTime()
    {
        coolTime = false;
        yield return new WaitForSeconds(bulletPrefabs[(int)bulletNum].GetComponent<BulletDefault>().CoolTime);
        coolTime = true;
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if((collision.tag == "EnemyProjectile" || collision.tag == "Trap") && hitable)
        {
            // 데미지 받아와서 발생 추가할 것
            Hit(collision.transform);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "EnemyProjectile" && hitable)
        {
            // 데미지 받아와서 발생 추가할 것
            Hit(collision.transform);
        }
    }

    void Hit(Transform attackerTransform)
    {
        rb.velocity = Vector2.zero;
        rb.AddForce(((transform.position.x < attackerTransform.position.x ? -transform.right : transform.right ) + transform.up) * knockBackPower, ForceMode2D.Impulse);
        StartCoroutine(HitCoolTime());
    }

    IEnumerator HitCoolTime()
    {
        hitable = false;
        yield return new WaitForSeconds(hitCoolTime);
        hitable = true;
    }
}
