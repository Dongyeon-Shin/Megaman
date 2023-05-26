using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int hp;
    public int Hp { get { return hp; } }
    [SerializeField]
    protected float detectRange;
    [SerializeField]
    protected float attackRange;
    protected Rigidbody2D rigidbody;
    protected SpriteRenderer spriteRenderer;
    protected bool isInvincible;
    // 플레이어 인식

    protected virtual void Awake()
    {

    }


    public void Spawn(Vector3 spawnPosiiton)
    {
        gameObject.transform.position = spawnPosiiton;
        gameObject.SetActive(true);
    }
    public void DeSpawn()
    {
        gameObject.SetActive(false);
    }
    public void GetDamage(int damage)
    {
        if (isInvincible)
        {
            return;
        }
        hp -= damage;
        //TODO: 피격 효과
        if ( hp <= 0)
        {
            Die();
        }

    }
    private void Die()
    {
        DeSpawn();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}
