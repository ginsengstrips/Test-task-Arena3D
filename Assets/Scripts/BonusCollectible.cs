using UnityEngine;

public class BonusCollectible : MonoBehaviour
{
    [SerializeField] private int _refillHeatlh;
    private EventManager _eventManager;
    private BonusManager _bonusManager;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _eventManager.RefillPlayerHealth(_refillHeatlh);
        }
        _bonusManager.ReturnBonusToPool(gameObject);
    }
    public void SetManagers(EventManager eventManager, BonusManager bonusManager)
    {
        _eventManager = eventManager;
        _bonusManager = bonusManager;
    }
}
