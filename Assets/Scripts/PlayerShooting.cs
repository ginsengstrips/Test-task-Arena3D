using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private EventManager _eventManager;
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
    private void Awake()
    {
        _playerInput = GameInputManager.instance.PlayerInput;
    }
    private void OnEnable()
    {
        _playerInput.Player.Reload.performed += Reload;
    }
    private void OnDisable()
    {
        _playerInput.Player.Reload.performed -= Reload;
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
        if (_bulletTrail == null) yield break;

        TrailRenderer trail = Instantiate(_bulletTrail, _bulletSpawnPoint.position, Quaternion.identity);
        trail.gameObject.SetActive(true);

        float time = 0f;
        Vector3 startPosition = trail.transform.position;

        while (time < 1f)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hitPoint, time);
            time += Time.deltaTime / _trailTime;
            yield return null;
        }

        trail.transform.position = hitPoint;

        yield return new WaitForSeconds(trail.time);
        Destroy(trail.gameObject);
    }
    private void Shoot()
    {
        _amountBullets--;
        _pistolAnimator.SetTrigger("Shoot");
        StartCoroutine(ShootCoolDown());
        _eventManager.ChangePlayerAmmo(_amountBullets, _maxBulletOnClip);
    }
    private void Reload(InputAction.CallbackContext obj)
    {
        if(!_isReloading && _amountBullets != _maxBulletOnClip)
            StartCoroutine(ReloadCoroutine());
    }
    private IEnumerator ReloadCoroutine()
    {
        _isReloading = true;
        _amountBullets = 0;
        _eventManager.Reload(_coolDownReload);
        yield return new WaitForSeconds(_coolDownReload);
        _amountBullets = _maxBulletOnClip;
        _eventManager.ChangePlayerAmmo(_amountBullets, _maxBulletOnClip);
        _isReloading = false;
        _canShoot = true;
    }
}
