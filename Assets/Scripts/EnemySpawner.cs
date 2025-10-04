using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private EventManager _eventManager;
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform[] _spawnPos;
    [SerializeField] private Transform _playerTransform;

    private void OnEnable()
    {
        _eventManager.OnEnemySpawn += Spawn;
    }
    private void OnDisable()
    {
        _eventManager.OnEnemySpawn -= Spawn;
    }
    private void Spawn(int amountEnemies, float coolDownSpawn, int numberWave)
    {
        StartCoroutine(Spawner(amountEnemies,coolDownSpawn, numberWave));
    }
    private IEnumerator Spawner(int amountEnemies, float coolDownSpawn, int numberWave)
    {
        WaitForSeconds wait = new WaitForSeconds(coolDownSpawn);
        for(int i = 0; i < amountEnemies; i++)
        {
            int randomIndexSpawnPos = Random.Range(0, _spawnPos.Length);
            GameObject enemy = Instantiate(_enemyPrefab, _spawnPos[randomIndexSpawnPos].position, Quaternion.identity);
            enemy.GetComponent<Enemy>().SetSettings(_playerTransform, _eventManager);
            enemy.GetComponent<Enemy>().SetParametrsOnWave(numberWave);
            _eventManager.SpawnedEnemy();
            yield return wait;
        }
    }
}
