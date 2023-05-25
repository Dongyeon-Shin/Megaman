using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private BoxCollider2D ladder;
    [SerializeField] private GameObject upPoint;
    [SerializeField] private GameObject downPoint;

    private void Awake()
    {
        ladder = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") // && 플레이어의 OnLadder가 트루이면
        {
            collision.transform.position = new Vector2(transform.position.x, collision.transform.position.y);
            if (collision.transform.position.y >  upPoint.transform.position.y || collision.transform.position.y < downPoint.transform.position.y)
            {
                // 플레이어의 OnLadder을 False로
            }
        } 
    }
}
