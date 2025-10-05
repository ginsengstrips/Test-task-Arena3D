using System.Collections.Generic;
using UnityEngine;

public class TrailObjectPool : MonoBehaviour
{
    [SerializeField] private TrailRenderer _bulletTrailPrefab;
    [SerializeField] private int _poolSize = 4;
    private Queue<TrailRenderer> _trailPool = new Queue<TrailRenderer>();
    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            CreateNewTrail();
        }
    }

    private TrailRenderer CreateNewTrail()
    {
        TrailRenderer trail = Instantiate(_bulletTrailPrefab);
        trail.gameObject.SetActive(false);
        _trailPool.Enqueue(trail);
        return trail;
    }

    public TrailRenderer GetTrail(Vector3 position)
    {
        TrailRenderer trail;

        if (_trailPool.Count > 0)
        {
            trail = _trailPool.Dequeue();
        }
        else
        {
            trail = CreateNewTrail();
        }

        trail.transform.position = position;
        trail.gameObject.SetActive(true);
        trail.Clear();
        return trail;
    }

    public void ReturnTrail(TrailRenderer trail)
    {
        trail.gameObject.SetActive(false);
        _trailPool.Enqueue(trail);
    }
}
