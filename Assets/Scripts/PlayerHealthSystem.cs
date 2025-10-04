using UnityEngine;

public class PlayerHealthSystem : MonoBehaviour
{
    [SerializeField] private int _health = 100;

    [SerializeField] private EventManager _eventManager;
    private void OnEnable()
    {
        _eventManager.OnPlayerDamaged += GetDamage;
    }
    private void OnDisable()
    {
        _eventManager.OnPlayerDamaged -= GetDamage;
    }
    public void GetDamage(int damage)
    {
        _health -= damage;
        _eventManager.ChangePlayerHealth(_health);
        Debug.Log("_health " + _health);
        if(_health <= 0)
        {
            Death();
        }
    }
    private void Death()
    {
        _eventManager.PlayerDeath();
    }
    
}
