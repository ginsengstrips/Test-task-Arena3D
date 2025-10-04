using UnityEngine;

public interface IDamageable
{
    int damage { get; }
    int health { get; }
    void TakeDamage(int amount);
    void Death();
}
