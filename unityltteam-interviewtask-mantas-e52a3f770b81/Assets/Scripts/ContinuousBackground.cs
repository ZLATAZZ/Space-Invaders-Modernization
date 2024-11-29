using PrimeTween;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ContinuousBackground : MonoBehaviour
{
    private float _moveSpeed = 5.0f;
    private float _distance = 10f;

    void Update()
    {
        MoveAndResetBackground();
    }

    private void MoveAndResetBackground()
    { 
        transform.position += Vector3.up * _moveSpeed * Time.deltaTime;
        if (transform.position.y > _distance)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
    }
}
