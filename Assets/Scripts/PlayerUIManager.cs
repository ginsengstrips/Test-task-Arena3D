using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class PlayerUIManager : MonoBehaviour
{
    [SerializeField] private EventManager _eventManager;
    [SerializeField] private Image _healthBar;
    [SerializeField] private Image _reloadIcon;
    [SerializeField] private GameObject _loseCanvas, playerCanvas;
    [SerializeField] private TextMeshProUGUI _ammoText, _waveNumberText, _aliveEnemyText, _nextWaveTimer;
    private void OnEnable()
    {
        _eventManager.OnChangeWaveUI += ChangeWaveNumber;
        _eventManager.OnPlayerHealthChanged += ChangeHealth;
        _eventManager.OnChangeEnemyUI += ChangeEnemyAmount;
        _eventManager.OnPlayerDeath += LoseUI;
        _eventManager.OnPlayerAmmoChanged += ChangeAmmoUI;
        _eventManager.OnReload += Reload;
    }
    private void OnDisable()
    {
        _eventManager.OnChangeWaveUI -= ChangeWaveNumber;
        _eventManager.OnPlayerHealthChanged -= ChangeHealth;
        _eventManager.OnChangeEnemyUI -= ChangeEnemyAmount;
        _eventManager.OnPlayerDeath -= LoseUI;
        _eventManager.OnPlayerAmmoChanged -= ChangeAmmoUI;
        _eventManager.OnReload -= Reload;
    }
    private void ChangeHealth(int heatlhAmount)
    {
        _healthBar.fillAmount = heatlhAmount / 100f;
    }
    private void ChangeAmmoUI(int ammoAmount, int maxAmmo)
    {
        _ammoText.text = $"Патроны: {ammoAmount}/{maxAmmo}";
    }
    private void ChangeWaveNumber(int waveNumber, float waveTimer)
    {
        _waveNumberText.text = $"Волна: {waveNumber}";
        NextWaveTimer(waveTimer);
    }
    private void NextWaveTimer(float time)
    {
        StartCoroutine(NextWaveTimerText(time)); 
    }
    private void ChangeEnemyAmount(int aliveEnemyAmount, int enemyOnWaveAmount)
    {
        _aliveEnemyText.text = $"Противников: {aliveEnemyAmount} /{enemyOnWaveAmount}";
    }
    private void Reload(float reloadTime)
    {
        StartCoroutine(ReloadCoroutine(reloadTime));
    }
    private IEnumerator NextWaveTimerText(float waveTime)
    {
        _nextWaveTimer.gameObject.SetActive(true);
        float time = waveTime;
        while (time > 0)
        {
            time -= Time.deltaTime;
            _nextWaveTimer.text = $" Следующая волна через:{Mathf.CeilToInt(time).ToString()}";
            yield return null;
        }
        _nextWaveTimer.gameObject.SetActive(false);
    }
    private IEnumerator ReloadCoroutine(float reloadTime)
    {
        _reloadIcon.gameObject.SetActive(true);
        float time = reloadTime;

        while (time > 0)
        {
            time -= Time.deltaTime;
            _reloadIcon.fillAmount = time / reloadTime;
            yield return null;
        }

        _reloadIcon.fillAmount = 1f;
        _reloadIcon.gameObject.SetActive(false);
    }
    private void LoseUI()
    {
        playerCanvas.SetActive(false);
        _loseCanvas.SetActive(true);
    }
}
