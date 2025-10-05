using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    [SerializeField] private EventManager _eventManager;
    [SerializeField] private Transform _spawnArea;
    [SerializeField] private GameObject[] _bonusPrefabs;
    private Queue<GameObject> _bonusQueue = new Queue<GameObject>();
    private int _poolSize = 4;
    private void Awake()
    {
        InitializePool();
    }
    private void OnEnable()
    {
        _eventManager.OnSpawnedBonus += SpawnRandomBonus;
    }
    private void OnDisable()
    {
        _eventManager.OnSpawnedBonus -= SpawnRandomBonus;
    }
    private void InitializePool()
    {
        for (int i = 0; i < _bonusPrefabs.Length; i++)
        {
            GameObject bonus = Instantiate(_bonusPrefabs[i]);
            bonus.GetComponent<BonusCollectible>().SetManagers(_eventManager, this);
            bonus.SetActive(false);
            _bonusQueue.Enqueue(bonus);
        }
        int remainingPoolSize = _poolSize - _bonusPrefabs.Length;
        for (int i = 0; i < remainingPoolSize; i++)
        {
            int randomIndex = Random.Range(0, _bonusPrefabs.Length);
            GameObject bonus = Instantiate(_bonusPrefabs[randomIndex]);
            bonus.GetComponent<BonusCollectible>().SetManagers(_eventManager, this);
            bonus.SetActive(false);
            _bonusQueue.Enqueue(bonus);
        }
    }
    private void SpawnRandomBonus()
    {
        if (_bonusQueue.Count == 0)
        {
            ExpandPool(1);
            return;
        }

        GameObject bonus = _bonusQueue.Dequeue();
        Vector3 randomPosition = GetRandomPositionInArea();
        bonus.transform.position = randomPosition;
        bonus.SetActive(true);
    }
    private Vector3 GetRandomPositionInArea()
    {
        Vector3 areaPosition = _spawnArea.position;
        float randomX = Random.Range(-areaPosition.x , areaPosition.x );
        float randomZ = Random.Range(-areaPosition.y, areaPosition.y);

        return new Vector3(
            areaPosition.x + randomX,
            areaPosition.y,
            areaPosition.z + randomZ
        );
    }
    public void ReturnBonusToPool(GameObject bonus)
    {
        if (bonus != null && !_bonusQueue.Contains(bonus))
        {
            bonus.SetActive(false);
            _bonusQueue.Enqueue(bonus);
        }
    }
    private void ExpandPool(int additionalSize)
    {
        for (int i = 0; i < additionalSize; i++)
        {
            int randomIndex = Random.Range(0, _bonusPrefabs.Length);
            GameObject bonus = Instantiate(_bonusPrefabs[randomIndex]);
            bonus.GetComponent<BonusCollectible>().SetManagers(_eventManager, this);
            bonus.SetActive(false);
            _bonusQueue.Enqueue(bonus);
        }
        _poolSize += additionalSize;
    }
}
