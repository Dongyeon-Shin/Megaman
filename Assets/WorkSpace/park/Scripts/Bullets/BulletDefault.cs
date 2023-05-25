using System.Collections;
using UnityEngine;

public abstract class BulletDefault : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] int damage;
    public int Damage { get { return damage; } }
    [SerializeField] float coolTime;
    public float CoolTime { get { return coolTime; } }

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    protected void Start()
    {
        StartCoroutine(SelfDestroyer());
    }

    public abstract void Shot(Vector2 shotDir, float shotPower);

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (DestroyCondition(collision))
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (DestroyCondition(collision.collider))
        {
            Destroy(gameObject);
        }
    }

    protected virtual bool DestroyCondition(Collider2D collision)
    {
        if(collision.tag == "Player" || collision.tag == "Bullet")
            return false;
        return true;
    }

    IEnumerator SelfDestroyer()
    {
        yield return new WaitForSeconds(10f);
        Destroy(gameObject);
    }
}