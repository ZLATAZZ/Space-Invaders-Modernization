using System;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class Enemy : MonoBehaviour {

    [SerializeField] private GameObject _prefabExplosion;
    [SerializeField] private PowerUp _prefabPowerUp;
    [SerializeField] private Projectile _prefabProjectile;
    [SerializeField] private Transform _firePosition;

    private float _powerUpSpawnChance = 0.1f;
    private int _health = 2;
    private float _speed = 2.0f;
    private Rigidbody _body;

    private bool canFire = false;
    private float _fireInterval = 2.5f;
    private float _fireTimer = 0.0f;
    
    private ObjectPoolManager _poolManagerInstance;

    private void Awake() {
        _body = GetComponent<Rigidbody>();
        canFire = Random.value < 0.4f;
        _health = 2 + Mathf.Min(Mathf.FloorToInt(Time.time / 15f), 5);
        _poolManagerInstance = ObjectPoolManager.Instance;
    }

    void Update() {

        if (canFire) {
            _fireTimer += Time.deltaTime;
            if (_fireTimer >= _fireInterval) {
                GameObject fire = ObjectPoolManager.Instance.GetPooledGameObject(ObjectPoolManager.TypesOfPoolObjects.FIRE);
                _poolManagerInstance.ActivatePooledGameObject(fire, _firePosition);
                _fireTimer -= _fireInterval;
            }
        }
    }

    private void FixedUpdate() {
        var p = _body.position;
        p += Vector3.down * (_speed * Time.deltaTime);
        _body.MovePosition(p);
    }

    public void Hit(int damage) {
        _health -= damage;
        if (_health <= 0) {
            GameObject fx = _poolManagerInstance.GetPooledGameObject(ObjectPoolManager.TypesOfPoolObjects.VFX);
            _poolManagerInstance.ActivatePooledGameObject(fx, transform);
            
            if (Random.value < _powerUpSpawnChance) {
                GameObject powerup = _poolManagerInstance.GetPooledGameObject(ObjectPoolManager.TypesOfPoolObjects.POWER_UP);

                _poolManagerInstance.ActivatePooledGameObject(powerup, _firePosition);

                var types = Enum.GetValues(typeof(PowerUp.PowerUpType)).Cast<PowerUp.PowerUpType>().ToList();
                powerup.GetComponent<PowerUp>().SetType(types[Random.Range(0,types.Count)]);
                gameObject.SetActive(false);
            }

            
            Object.FindObjectOfType<GameplayUi>(true).AddScore(1);

        }
    }

}
