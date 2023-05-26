using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Door : MonoBehaviour
{
    [SerializeField] float force;
    private float doorOpen = 0;
    public void Open()
    {
        StartCoroutine(DoorOpen());
    }
    IEnumerator DoorOpen()
    {
        yield return new WaitForSeconds(3);
        while(doorOpen < 5)
        {
            yield return new WaitForEndOfFrame();
            transform.Translate(Vector3.up * Time.deltaTime * force);
            doorOpen += Time.deltaTime;
        }
        Destroy(gameObject);
    }
}
