using UnityEngine;

public class BulletNormal : BulletDefault
{
    public override void Shot(Vector2 shotDir, float shotPower)
    {
        rb.AddForce(shotDir * shotPower, ForceMode2D.Impulse);
    }
}