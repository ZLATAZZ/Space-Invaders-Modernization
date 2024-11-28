using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class Player : MonoBehaviour {

    public event System.Action OnDie;
    
    [SerializeField] private GameObject _prefabExplosion;
    [SerializeField] private Projectile _prefabProjectile;
    [SerializeField] private Transform _projectileSpawnLocation;
    [SerializeField] private GameObject _additionalPlayer;

    [HideInInspector] public int _playerHealth = 3;
    
    private Rigidbody _body = null;
    
    private Vector2 _lastInput;
    private bool _hasInput = false;
    
    float _fireInterval = 0.4f;
    private float _fireTimer = 0.0f;

    private ObjectPoolManager _poolManagerInstance;
    private AudioManager _audioManagerInstance;
    private float playAreaMaxX = 3f;
    private void Awake() {
        _body = GetComponent<Rigidbody>();
        _poolManagerInstance = ObjectPoolManager.Instance;
        _audioManagerInstance = AudioManager.Instance;
    }

    void Start() {
        Object.FindObjectOfType<GameplayUi>(true).UpdateHealth(_playerHealth);
        Object.FindObjectOfType<GameOverUi>(true).Close();
    }

    private void Update() {

        if (gameObject.CompareTag("Player"))
        {
            if (Input.GetMouseButtonDown(0)) _hasInput = true;
            if (Input.GetMouseButtonUp(0)) _hasInput = false;
            if (Input.GetMouseButton(0))
            {
                _lastInput = Input.mousePosition;
            }
            if (_hasInput)
            {
                Vector3 screenPosition = _lastInput;
                screenPosition.z = 0;

                Vector3 pos = Camera.main.ScreenToWorldPoint(screenPosition);

                const float playAreaMinX = -3f;
                const float playAreaMinY = 0f;
                if (_additionalPlayer.activeInHierarchy)
                {
                    playAreaMaxX = .5f;
                }
                else
                {
                    playAreaMaxX = 3f;
                }
                
                const float playAreaMaxY = 5f;

                _body.MovePosition(new Vector3(Mathf.Clamp(pos.x, playAreaMinX, playAreaMaxX), Mathf.Clamp(pos.y, playAreaMinY, playAreaMaxY), 0.0f));

            }
        }

        _fireTimer += Time.deltaTime;
        if (_fireTimer >= _fireInterval) {
            GameObject bullet = _poolManagerInstance.GetPooledGameObject(ObjectPoolManager.TypesOfPoolObjects.BULLET);
            _poolManagerInstance.ActivatePooledGameObject(bullet, _projectileSpawnLocation);
            Tween.ScaleY(transform, 1.2f, 1, .5f);
            _fireTimer -= _fireInterval;
        }
    }

    public void Hit() {

        _playerHealth--;
        _audioManagerInstance.PlayHitSound();
        Object.FindObjectOfType<GameplayUi>(true).UpdateHealth(_playerHealth);

        if (_playerHealth <= 0) {
            _audioManagerInstance.PlayGameOverSound();
            GameObject fx = _poolManagerInstance.GetPooledGameObject(ObjectPoolManager.TypesOfPoolObjects.EXPLOSION);
            _poolManagerInstance.ActivatePooledGameObject(fx, transform);
            gameObject.SetActive(false);
            _poolManagerInstance.DeactivatePooledObjects();
            Invoke("GameOver", .5f);//small delay to see the explosion
        }
    }
    private void GameOver()
    {

        Object.FindObjectOfType<GameOverUi>(true).Open();
        gameObject.SetActive(false);
        OnDie?.Invoke();
        Time.timeScale = 0;
        return;
    }

    public void AddPowerUp(PowerUp.PowerUpType type) {
        GameObject fxPowerUp = _poolManagerInstance.GetPooledGameObject(ObjectPoolManager.TypesOfPoolObjects.POWER_UP_VFX);
        _poolManagerInstance.ActivatePooledGameObject(fxPowerUp, transform);
        _audioManagerInstance.PlayPowerUpSound();
        switch (type)
        {
            case PowerUp.PowerUpType.FIRE_RATE: _fireInterval *= 0.9f; break;
            case PowerUp.PowerUpType.HEAL when _playerHealth < 3: _playerHealth+=1; Object.FindObjectOfType<GameplayUi>(true).UpdateHealth(_playerHealth); break;
            case PowerUp.PowerUpType.ADD_SPACECRAFT: _additionalPlayer.SetActive(true); Invoke("DeactivateAdditionalSpacecraft", 10); break;
        }
    }

    private void DeactivateAdditionalSpacecraft()
    {
        _additionalPlayer.SetActive(false);
    }
}