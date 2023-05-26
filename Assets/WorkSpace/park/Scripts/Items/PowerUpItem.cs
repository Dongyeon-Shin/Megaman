using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpItem : MonoBehaviour
{
    [SerializeField][Range(1, 2)] int level;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if(GameManager.Data.Mode < level)
            {
                GameManager.Data.Mode = level;
            }
            Destroy(gameObject);
        }
    }
}
