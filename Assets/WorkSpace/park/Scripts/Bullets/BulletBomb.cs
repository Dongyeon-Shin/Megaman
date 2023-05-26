using System.Collections;
using UnityEngine;

public class BulletBomb : BulletDefault
{
    [SerializeField] Animator animator;
    [SerializeField] bool bombed;

    new void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
    }

    public override void Shot(Vector2 shotDir, float shotPower)
    {
        rb.AddForce((new Vector2(0, 0.5f) + shotDir) * shotPower, ForceMode2D.Impulse);
        StartCoroutine(Bomb());
    }

    protected override bool DestroyCondition(Collider2D collision)
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (bombed)
            Destroy(GetComponent<Collider2D>());

        return false;
    }

    IEnumerator Bomb()
    {
        yield return new WaitForSeconds(2f);
        animator.SetTrigger("Bomb");
        bombed = true;
        yield return new WaitForSeconds(.2f);
        Destroy(gameObject);
    }
}