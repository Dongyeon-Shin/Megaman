using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeUpItem : MonoBehaviour
{
    [SerializeField] int heal;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<PlayerDataManager>().HealDataProcess(heal);
            Destroy(gameObject);
        }
    }
}
