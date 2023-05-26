using UnityEngine;
using UnityEngine.Events;

public class DataManager : MonoBehaviour
{
    static int MaxLife = 30, MaxMode = 3;
    [SerializeField] int life, mode;
    public int Life 
    { 
        get { return life; } 
        set
        {
            life = value;
            if(life > MaxLife)
                life = MaxLife;
        } 
    }
    public int Mode 
    { 
        get { return mode; } 
        set 
        { 
            mode = value;
            if(mode > MaxMode)
                mode = MaxMode;
        } 
    }

    void Awake()
    {
        Life = MaxLife;
        Mode = 0;
    }
}
