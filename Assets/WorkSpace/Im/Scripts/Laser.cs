using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private float laserSpeed;

    private void Start()
    {
        Destroy(gameObject, 4f);
    }
    private void Update()
    {
        transform.Translate(Vector3.down * laserSpeed * Time.deltaTime);
    }

}
