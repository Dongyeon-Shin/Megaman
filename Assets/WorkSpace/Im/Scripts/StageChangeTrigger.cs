using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageChangeTrigger : MonoBehaviour
{
    public UnityEvent stageChange;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            stageChange?.Invoke();
        }
    }
}
