using System.Collections;
using UnityEngine;

public class BulletReturn : BulletDefault
{
    [SerializeField] float returnTime;

    public override void Shot(Vector2 shotDir, float shotPower)
    {
        rb.AddForce(shotDir * shotPower * 0.8f, ForceMode2D.Impulse);
        StartCoroutine(Return());
    }

    IEnumerator Return()
    {
        yield return new WaitForSeconds(returnTime);
        rb.velocity = new Vector2(-rb.velocity.x, 0);
    }
}
