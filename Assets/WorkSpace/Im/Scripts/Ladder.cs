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
    private bool xAxis;
    private bool CheckX(Collider2D collision)
    {
        if(collision.transform.position.x >= transform.position.x + 0.3f || collision.transform.position.x <= transform.position.x - 0.3f)
            return true;
        else return false;
    }
    private bool CheckY(Collider2D collision)
    {
        if (collision.transform.position.y > upPoint.transform.position.y || collision.transform.position.y < downPoint.transform.position.y)
            return true;
        else return false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player") // && 플레이어의 OnLadder가 트루이면
        {
            if(!xAxis)
            {
                collision.transform.position = new Vector2(transform.position.x, collision.transform.position.y);
                xAxis = true;
            } 
            else
            {
                if (CheckY(collision) || CheckX(collision))
                {
                    // 플레이어의 OnLadder을 False로
                    xAxis = false;
                }
            }
        } 
    }
}
