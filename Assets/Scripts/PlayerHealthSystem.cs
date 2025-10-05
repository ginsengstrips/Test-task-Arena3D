using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    [SerializeField] private int _health = 100;

    [SerializeField] private EventManager _eventManager;
    private void OnEnable()
    {
        _eventManager.OnPlayerDamaged += GetDamage;
        _eventManager.OnPlayerHealthRefill += RefillHealth;
    }
    private void OnDisable()
    {
        _eventManager.OnPlayerDamaged -= GetDamage;
        _eventManager.OnPlayerHealthRefill -= RefillHealth;
    }
    private void GetDamage(int damage)
    {
        _health -= damage;
        _eventManager.ChangePlayerHealth(_health);
        if(_health <= 0)
        {
            Death();
        }
    }
    private void RefillHealth(int refillAmountHealth)
    {
        _health = Mathf.Clamp(_health + refillAmountHealth, 0, 100);
        _eventManager.ChangePlayerHealth(_health);
    }
    private void Death()
    {
        _eventManager.PlayerDeath();
    }
    
}
