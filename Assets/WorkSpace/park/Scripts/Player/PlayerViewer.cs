using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerViewer : MonoBehaviour
{
    [SerializeField] GameObject[] lifes;

    public void OnLifeModified()
    {
        for(int i = 0; i < lifes.Length; i++)
        {
            if(i < 30 - GameManager.Data.Life)
            {
                lifes[i].SetActive(false);
            }
            else
            {
                lifes[i].SetActive(true);
            }
        }
    }
}
