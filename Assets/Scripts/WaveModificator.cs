using UnityEngine;

public class WaveModificator : MonoBehaviour
{
    [SerializeField] private EventManager _eventManager;
    [SerializeField] private float _waveAmountModifier = 1.2f;
    [SerializeField] private float _coolDownSpawn =3f;
    [SerializeField] private float _waitTimeBeforeNewWave = 2f;
    [SerializeField] private int _amountSpawnEnemy = 1;

    private int _numberWave = 0;
    private int _currentEnemyRemains = 0;
    private void OnEnable()
    {
       // _eventManager.OnEnemySpawned += OnEnemySpawned;
        _eventManager.OnEnemyDeath += OnKillEnemies;
    }
    private void OnDisable()
    {
        //_eventManager.OnEnemySpawned -= OnEnemySpawned;
        _eventManager.OnEnemyDeath -= OnKillEnemies;
    }
    private void Start()
    {
        NextWave();
    }
    private void SpawnEnemies()
    {
        _eventManager.SpawnEnemy(_amountSpawnEnemy, _coolDownSpawn,_numberWave);
    }
    public void OnEnemySpawned()
    {
        //_currentEnemyAmount++;
    }
    private void OnKillEnemies()
    {
        _currentEnemyRemains--;
        _eventManager.ChangeEnemyUI(_currentEnemyRemains, _amountSpawnEnemy);
        if (_currentEnemyRemains == 0)
        {
            NextWave();
        }
    }
    private void NextWave()
    {
        _numberWave++;
        _amountSpawnEnemy = Mathf.CeilToInt(_numberWave * _waveAmountModifier);
        _currentEnemyRemains = _amountSpawnEnemy;
        Invoke(nameof(SpawnEnemies), _waitTimeBeforeNewWave);
        _eventManager.NumberWaveChanged(_numberWave,_amountSpawnEnemy);
        _eventManager.ChangeWaveUI(_numberWave, _waitTimeBeforeNewWave);
        _eventManager.ChangeEnemyUI(_currentEnemyRemains, _amountSpawnEnemy);
    }

}
