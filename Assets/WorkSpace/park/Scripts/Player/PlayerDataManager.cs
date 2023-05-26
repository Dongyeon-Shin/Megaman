using UnityEngine;
using UnityEngine.Events;

public class PlayerDataManager : MonoBehaviour
{
    [SerializeField] int life, mode;
    [SerializeField] UnityEvent OnDieEvent, HPModifedEvent;

    void Start()
    {
        life = GameManager.Data.Life;
        mode = GameManager.Data.Mode;
    }

    public void HitDataProcess(int damage)
    {
        GameManager.Data.Life -= damage;
        HPModifedEvent?.Invoke();

        if (GameManager.Data.Life <= 0)
        {
            OnDieEvent?.Invoke();
            // 게임 오버 관련 어쩌구
        }
    }

    public void HealDataProcess(int heal)
    {
        GameManager.Data.Life += heal;
        HPModifedEvent?.Invoke();
    }
}
