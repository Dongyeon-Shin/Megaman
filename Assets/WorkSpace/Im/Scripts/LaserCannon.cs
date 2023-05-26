using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCannon : MonoBehaviour
{
    [SerializeField] private GameObject laser;
    [SerializeField] private float rapid;

    private void Start()
    {
        laserCannon = StartCoroutine(Cannon());
    }

    Coroutine laserCannon;
    IEnumerator Cannon()
    {
        while (true)
        {
            yield return new WaitForSeconds(rapid);
            Instantiate(laser, transform.position, transform.rotation);
        }
    }
}
