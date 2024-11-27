using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using Object = UnityEngine.Object;

public class Player : MonoBehaviour {

    public event System.Action OnDie;
    
    [SerializeField] private GameObject _prefabExplosion;
    [SerializeField] private Projectile _prefabProjectile;
    [SerializeField] private Transform _projectileSpawnLocation;

    private int _playerHealth = 3;
    
    private Rigidbody _body = null;
    
    private Vector2 _lastInput;
    private bool _hasInput = false;
    
    float _fireInterval = 0.4f;
    private float _fireTimer = 0.0f;

    private ObjectPoolManager _poolManagerInstance;

    private void Awake() {
        _body = GetComponent<Rigidbody>();
        _poolManagerInstance = ObjectPoolManager.Instance;
    }

    void Start() {
        Object.FindObjectOfType<GameplayUi>(true).UpdateHealth(_playerHealth);
        Object.FindObjectOfType<GameOverUi>(true).Close();
    }

    private void Update() {
        
        if (Input.GetMouseButtonDown(0)) _hasInput = true;
        if (Input.GetMouseButtonUp(0)) _hasInput = false;
        if (Input.GetMouseButton(0)) {
            _lastInput = Input.mousePosition;
        }
        if (_hasInput)
        {
            Vector3 screenPosition = _lastInput;
            screenPosition.z = 0; 

            Vector3 pos = Camera.main.ScreenToWorldPoint(screenPosition);

            const float playAreaMinX = -3f;
            const float playAreaMinY = 0f;
            const float playAreaMaxX = 3f;
            const float playAreaMaxY = 5f;

            _body.MovePosition(new Vector3(Mathf.Clamp(pos.x,playAreaMinX, playAreaMaxX), Mathf.Clamp(pos.y, playAreaMinY, playAreaMaxY), 0.0f));

        }

        _fireTimer += Time.deltaTime;
        if (_fireTimer >= _fireInterval) {
            GameObject bullet = _poolManagerInstance.GetPooledGameObject(ObjectPoolManager.TypesOfPoolObjects.BULLET);
            _poolManagerInstance.ActivatePooledGameObject(bullet, _projectileSpawnLocation);
            
            _fireTimer -= _fireInterval;
        }
    }

    public void Hit() {

        _playerHealth--;

        Object.FindObjectOfType<GameplayUi>(true).UpdateHealth(_playerHealth);

        if (_playerHealth <= 0) {
            Object.FindObjectOfType<GameOverUi>(true).Open();
            var fx = Instantiate(_prefabExplosion);
            fx.transform.position = transform.position;
            Destroy(gameObject);
            OnDie?.Invoke();
            return;
        }
    }

    private void FixedUpdate() {
        
    }

    public void AddPowerUp(PowerUp.PowerUpType type) {

        if (type == PowerUp.PowerUpType.FIRE_RATE) {
            _fireInterval *= 0.9f;
        }
    }
}