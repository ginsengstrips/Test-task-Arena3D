using System;
using UnityEngine;
public class Enemy : MonoBehaviour, IDamageable
{
    public event Action<GameObject> OnEnemyDeath;

    [SerializeField] private int _enemyDamage = 2;
    [SerializeField] private int _enemyHealth = 100;
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Animator _enemyAnimator;
    private float _coolDownDisable=2f;
    private EventManager _eventManager;
    private int _startHealth;

    private Collision _collision;
    private Transform _playerTransform;
    private bool _isDeath;
    private bool _isAttack;
    public int damage => _enemyDamage;
    public int health => _enemyHealth;
    private void Start()
    {
        _startHealth = _enemyHealth;
    }
    private void Update()
    {
        if (_isDeath || _playerTransform == null || _isAttack)
            return;
        EnemyMove();
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _collision = collision;
            _isAttack = true;
            _enemyAnimator.SetBool("Attack", _isAttack);
            
        }
            
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _collision = null;
            _isAttack = false;
            _enemyAnimator.SetBool("Attack", _isAttack);
        }
    }
    public void TakeDamage(int amount)
    {
        if (_isDeath)
            return;
        _enemyHealth -= amount;
        if(_enemyHealth <= 0)
        {
            Death();
        }
    }
    public void Death()
    {
        _isDeath = true;
        _enemyAnimator.SetTrigger("Death");
        _eventManager.DeathEnemy();
        Invoke(nameof(DisableModel), _coolDownDisable);
    }
    public void EnemyMove()
    {
        Vector3 direction = (_playerTransform.position - transform.position).normalized;
        direction.y = 0f;
        transform.position += direction * _moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.LookRotation(direction);
    }
    public void EnemyAttack()
    {
        if (_collision == null)
            return;
        _eventManager.PlayerDamage(damage);
    }
    public void SetSettings(Transform targetPos, EventManager eventManager)
    {
        _playerTransform = targetPos;
        _eventManager = eventManager;
    }
    public void SetParametrsOnWave(int numberWave)
    {
        if (numberWave <= 2)
        {
            _moveSpeed = 3.5f;
            _enemyAnimator.SetBool("Walk", true);
        }
          
        else if (numberWave <= 3)
        {
            _moveSpeed = 6.5f;
            _enemyHealth = 150;
            _enemyAnimator.SetBool("Run", true);
            _enemyDamage *= 2;
        }

        else
        {
            _moveSpeed = 8f;
            _enemyAnimator.SetBool("Run", true);
        }
        
    }
    private void DisableModel()
    {
        ResetParametrs();
        OnEnemyDeath?.Invoke(gameObject);
    }
    private void ResetParametrs()
    {
        _enemyHealth = _startHealth;
        _isDeath = false;
        _isAttack = false;
        _enemyAnimator.SetBool("Run", false);
        _enemyAnimator.SetBool("Walk", false);
        _enemyAnimator.SetTrigger("Idle");
    }
}
