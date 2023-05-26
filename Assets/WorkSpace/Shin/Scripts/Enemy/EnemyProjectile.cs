using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float range;
    private float time;
    [SerializeField]
    private int damage;
    private void OnEnable()
    {
        // TODO: enemy와 enemyProjectile 레이어 충돌감지 안함
        time = 0;
        StartCoroutine("LaunchRoutine");     
    }
    private void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);
    }
    IEnumerator LaunchRoutine()
    {
        while (time < range)
        {
            yield return new WaitForSeconds(1f);
            time++;
        }
        gameObject.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if(collision.collider.gameObject.CompareTag("Player"))
        {
            // TODO: 플레이어 데미지
        }
        gameObject.SetActive(false);
    }
}
