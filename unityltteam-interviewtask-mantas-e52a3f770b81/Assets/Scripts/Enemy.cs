using PrimeTween;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject _prefabExplosion;
    [SerializeField] private PowerUp _prefabPowerUp;
    [SerializeField] private Projectile _prefabProjectile;
    [SerializeField] private Transform _firePosition;
    [SerializeField] private Mesh[] powerUpMeshes;
    [SerializeField] private Material[] powerUpMaterials;

    private float _powerUpSpawnChance = 0.2f;
    [HideInInspector] public int _health = 2;
    private float _speed = 2.0f;
    private Rigidbody _body;
    private bool canFire = false;
    private float _fireInterval = 2.5f;
    private float _fireTimer = 0.0f;

    private ObjectPoolManager _poolManagerInstance;
    private AudioManager _audioManagerInstance;
    private void Awake()
    {
        _body = GetComponent<Rigidbody>();
        canFire = Random.value < 0.4f;
        _health = 2;
        _poolManagerInstance = ObjectPoolManager.Instance;
        _audioManagerInstance = AudioManager.Instance;
        Debug.Log(Time.deltaTime);
    }

    private void OnEnable()
    {
        _health = 2 + Mathf.Min(Mathf.FloorToInt(Time.timeSinceLevelLoad / 15f), 5);
        gameObject.GetComponentInChildren<Slider>().maxValue = _health;
    }

    void Update()
    {
        if (canFire)
        {
            Shoot();
        }
        FindObjectOfType<GameplayUi>(true).UpdateEnemyHealthBar(_health, gameObject.GetComponentInChildren<Slider>());
    }

    private void FixedUpdate()
    {
        SetPosition();
    }
    private void OnTriggerEnter(Collider other)
    {
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            _audioManagerInstance.PlayExplosionSound();
            player._playerHealth = 0;
            player.Hit();
        }
        else if (other.gameObject.CompareTag("BottomBorder"))
        {
            gameObject.SetActive(false);
        }
    }

    private void Shoot()
    {
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= _fireInterval)
        {
            GameObject fire = ObjectPoolManager.Instance.GetPooledGameObject(ObjectPoolManager.TypesOfPoolObjects.FIRE);
            _poolManagerInstance.ActivatePooledGameObject(fire, _firePosition);
            _fireTimer -= _fireInterval;
        }
    }
    private void SetPosition()
    {
        var p = _body.position;
        p += Vector3.down * (_speed * Time.deltaTime);
        _body.MovePosition(p);
    }

    public void Hit(int damage)
    {
        _health -= damage;

        _audioManagerInstance.PlayBlasterSound();
        Tween.Scale(transform, .6f, 1, .5f);

        GameObject fxHit = _poolManagerInstance.GetPooledGameObject(ObjectPoolManager.TypesOfPoolObjects.HIT_VFX);
        _poolManagerInstance.ActivatePooledGameObject(fxHit, transform);

        if (_health <= 0)
        {
            _audioManagerInstance.PlayExplosionSound();

            GameObject fx = _poolManagerInstance.GetPooledGameObject(ObjectPoolManager.TypesOfPoolObjects.EXPLOSION);
            _poolManagerInstance.ActivatePooledGameObject(fx, transform);

            if (Random.value < _powerUpSpawnChance)
            {
                GameObject powerup = _poolManagerInstance.GetPooledGameObject(ObjectPoolManager.TypesOfPoolObjects.POWER_UP);

                _poolManagerInstance.ActivatePooledGameObject(powerup, _firePosition);
                Tween.EulerAngles(powerup.transform, Vector3.zero, new Vector3(0, 360), 1);
                var types = Enum.GetValues(typeof(PowerUp.PowerUpType)).Cast<PowerUp.PowerUpType>().ToList();
                int randomPowerUpIndex = Random.Range(0, types.Count);
                powerup.GetComponent<PowerUp>().SetType(types[randomPowerUpIndex]);
                //change powerUp look accordingly to type
                powerup.GetComponentInChildren<MeshFilter>().mesh = powerUpMeshes[randomPowerUpIndex];
                powerup.GetComponentInChildren<MeshRenderer>().material = powerUpMaterials[randomPowerUpIndex];

            }

            gameObject.SetActive(false);
            Object.FindObjectOfType<GameplayUi>(true).AddScore(1);
        }
    }


}
