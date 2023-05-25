using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Lava : MonoBehaviour
{
    BoxCollider2D[] col;
    UnityEvent inLava;

    private void Awake()
    {
        col = GetComponents<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            inLava?.Invoke();
        }
    }
}
