using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EventManager _eventManager;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform[] _spawnPos;
    [SerializeField] private Transform _playerTransform;
    private Queue<GameObject> _enemyPool = new Queue<GameObject>();
    private int _poolSize = 15;
    private void Awake()
    {
        InitializePool();
    }
    private void OnEnable()
    {
        _eventManager.OnEnemySpawn += Spawn;
    }
    private void OnDisable()
    {
        _eventManager.OnEnemySpawn -= Spawn;
    }
    private void InitializePool()
    {
        for(int i=0;i< _poolSize; i++)
        {
            GameObject enemy = Instantiate(_enemyPrefab);
            enemy.SetActive(false);
            _enemyPool.Enqueue(enemy);
        }
    }
    private void Spawn(int amountEnemies, float coolDownSpawn, int numberWave)
    {
        StartCoroutine(Spawner(amountEnemies,coolDownSpawn, numberWave));
    }
    private IEnumerator Spawner(int amountEnemies, float coolDownSpawn, int numberWave)
    {
        WaitForSeconds wait = new WaitForSeconds(coolDownSpawn);
        for (int i = 0; i < amountEnemies; i++)
        {
            if (_enemyPool.Count == 0)
            {
                ExpandPool(5);
            }
            int randomIndexSpawnPos = Random.Range(0, _spawnPos.Length);
            GameObject enemy = GetEnemyFromPool();
            if (enemy != null)
            {
                SetupEnemy(enemy, _spawnPos[randomIndexSpawnPos].position, numberWave);
            }

            yield return wait;
        }
    }
    private GameObject GetEnemyFromPool()
    {
        if (_enemyPool.Count > 0)
        {
            GameObject enemy = _enemyPool.Dequeue();
            return enemy;
        }
        return null;
    }

    public void ReturnEnemyToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        _enemyPool.Enqueue(enemy);
    }

    private void SetupEnemy(GameObject enemy, Vector3 spawnPosition, int numberWave)
    {
        enemy.transform.position = spawnPosition;
        enemy.SetActive(true);

        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        enemyComponent.SetSettings(_playerTransform, _eventManager);
        enemyComponent.SetParametrsOnWave(numberWave);

        enemyComponent.OnEnemyDeath += HandleEnemyDeath;
    }
    private void HandleEnemyDeath(GameObject enemy)
    {
        Enemy enemyComponent = enemy.GetComponent<Enemy>();
        enemyComponent.OnEnemyDeath -= HandleEnemyDeath; 
        ReturnEnemyToPool(enemy);
    }

    private void ExpandPool(int additionalSize)
    {
        for (int i = 0; i < additionalSize; i++)
        {
            GameObject enemy = Instantiate(_enemyPrefab);
            enemy.SetActive(false);
            _enemyPool.Enqueue(enemy);
        }
    }
}
