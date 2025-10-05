using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public event Action OnPlayerDeath;
    public event Action<int> OnPlayerHealthChanged;
    public event Action<int> OnPlayerHealthRefill;
    public event Action OnRefillHealthSound;
    public event Action<int, int> OnPlayerAmmoChanged;
    public event Action<int> OnPlayerDamaged;
    public event Action<int, int> OnNumberWaveChanged;
    public event Action OnWaveChangedSound;
    public event Action OnSpawnedBonus;
    public event Action<int, float, int> OnEnemySpawn;
    public event Action OnEnemyDeath;
    public event Action<int,int> OnChangeEnemyUI;
    public event Action<int, float> OnChangeWaveUI;
    public event Action<float> OnReload;
    public event Action OnReloadSound;
    private void Start()
    {
        Time.timeScale = 1f;
    }
    public void PlayerDeath()
    {
        OnPlayerDeath?.Invoke();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void ChangePlayerHealth(int healthAmount)
    {
        OnPlayerHealthChanged?.Invoke(healthAmount);
    }
    public void ChangePlayerAmmo(int ammoAmount, int maxAmmo)
    {
        OnPlayerAmmoChanged?.Invoke(ammoAmount, maxAmmo);
    }
    public void PlayerDamage(int damage)
    {
        OnPlayerDamaged?.Invoke(damage);
    }
    public void NumberWaveChanged(int numberWave, int amountEnemies)
    {
        OnNumberWaveChanged?.Invoke(numberWave, amountEnemies);
        OnWaveChangedSound?.Invoke();
    }
    public void Reload(float reloadTime)
    {
        OnReload?.Invoke(reloadTime);
        OnReloadSound?.Invoke();
    }
    public void SpawnEnemy(int amountEnemies, float coolDownSpawn, int numberWave)
    {
        OnEnemySpawn?.Invoke(amountEnemies, coolDownSpawn, numberWave);
    }
    public void DeathEnemy()
    {
        OnEnemyDeath?.Invoke();
    }
    public void ChangeEnemyUI(int aliveEnemyAmount, int enemyOnWaveAmount)
    {
        OnChangeEnemyUI?.Invoke(aliveEnemyAmount,enemyOnWaveAmount);
    }
    public void ChangeWaveUI(int numberWave, float waveTimer)
    {
        OnChangeWaveUI?.Invoke(numberWave,waveTimer);
        if (numberWave % 2 == 0)
            OnSpawnedBonus?.Invoke();
    }
    public void RefillPlayerHealth(int refillHealthAmount)
    {
        OnPlayerHealthRefill?.Invoke(refillHealthAmount);
        OnRefillHealthSound?.Invoke();
    }
}
