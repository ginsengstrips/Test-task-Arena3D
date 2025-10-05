using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private EventManager _eventManager;
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private TrailObjectPool _trailPool;
    [SerializeField] private int _playerDamage;
    [SerializeField] private float _coolDownShoot = 0.5f;
    private bool _canShoot = true;
    private PlayerInput _playerInput;

    [SerializeField] private TrailRenderer _bulletTrail; 
    [SerializeField] private float _trailTime = 0.1f; 
    [SerializeField] private Transform _bulletSpawnPoint;

    [SerializeField] private Animator _pistolAnimator;
    private float _coolDownReload=1.5f;
    private int _amountBullets =7;
    private int _maxBulletOnClip =7;
    private bool _isReloading;

    private bool _isAim;
    private float _normalFOV=60f;
    private float _aimFOV = 40f;
    private void Awake()
    {
        _playerInput = GameInputManager.instance.PlayerInput;
    }
    private void OnEnable()
    {
        _playerInput.Player.Reload.performed += Reload;
        _playerInput.Player.Aim.performed += Aim;
    }
    private void OnDisable()
    {
        _playerInput.Player.Reload.performed -= Reload;
        _playerInput.Player.Aim.performed -= Aim;
    }
    private void Update()
    {
        if (_playerInput.Player.Shoot.ReadValue<float>() > 0.1f)
        {
            if (_canShoot && _amountBullets >0)
            {
                Shoot();
                Ray();
            }
            else if(_amountBullets <= 0)
            {
                if (!_isReloading)
                    StartCoroutine(ReloadCoroutine());
            }
        }
    }
    private void Ray()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Vector3 targetPoint = ray.origin + ray.direction * 105f;
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            targetPoint = hit.point;
            IDamageable interactable = hit.collider.GetComponent<IDamageable>();
            if (interactable != null)
            {
                interactable.TakeDamage(_playerDamage);
            }
        }
        StartCoroutine(SpawnTrail(targetPoint));
    }
    private IEnumerator ShootCoolDown()
    {
        _canShoot = false;
        yield return new WaitForSeconds(_coolDownShoot);
        _canShoot = true;
    }
    private IEnumerator SpawnTrail(Vector3 hitPoint)
    {
        if (_trailPool == null) yield break;

        TrailRenderer trail = _trailPool.GetTrail(_bulletSpawnPoint.position);
        yield return null;
        float time = 0f;
        Vector3 startPosition = trail.transform.position;
        while (time < 1f)
        {
            if (trail == null) 
                yield break;
            trail.transform.position = Vector3.Lerp(startPosition, hitPoint, time);
            time += Time.deltaTime / _trailTime;
            yield return null;
        }
        trail.transform.position = hitPoint;
        if (trail != null)
        {
            trail.transform.position = hitPoint;
            yield return new WaitForSeconds(trail.time);
            _trailPool.ReturnTrail(trail);
        }
    }
    private void Shoot()
    {
        _amountBullets--;
        _pistolAnimator.SetTrigger("Shoot");
        StartCoroutine(ShootCoolDown());
        _soundManager.ShotSound();
        _eventManager.ChangePlayerAmmo(_amountBullets, _maxBulletOnClip);
    }
    private void Reload(InputAction.CallbackContext obj)
    {
        if(!_isReloading && _amountBullets != _maxBulletOnClip)
            StartCoroutine(ReloadCoroutine());
    }
    private void Aim(InputAction.CallbackContext obj)
    {
        _isAim = !_isAim;
        Camera.main.fieldOfView = _isAim ? _normalFOV : _aimFOV;
    }
    private IEnumerator ReloadCoroutine()
    {
        _isReloading = true;
        _pistolAnimator.SetBool("Reload", _isReloading);
        _amountBullets = 0;
        _eventManager.Reload(_coolDownReload);
        yield return new WaitForSeconds(_coolDownReload);
        _amountBullets = _maxBulletOnClip;
        _eventManager.ChangePlayerAmmo(_amountBullets, _maxBulletOnClip);
        _isReloading = false;
        _pistolAnimator.SetBool("Reload", _isReloading);
        _canShoot = true;
    }
}
