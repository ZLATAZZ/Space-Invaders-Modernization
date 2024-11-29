using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour {
    private float _speed = 3.0f;
    public enum PowerUpType {
        FIRE_RATE = 0,
        HEAL = 1,
        ADD_SPACECRAFT = 2,
    }

    [SerializeField] private PowerUpType _type;

    public void SetType(PowerUpType type) {
        _type = type;
    }

    private void Update() {
        SetPowerUpPosition();
    }

    private void OnTriggerEnter(Collider other) {
        
        var player = other.GetComponent<Player>();
        if (player != null)
        {
            player.AddPowerUp(_type);
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void SetPowerUpPosition()
    {
        var p = transform.position;
        p += Vector3.down * (_speed * Time.deltaTime);
        transform.position = p;
    }
}
