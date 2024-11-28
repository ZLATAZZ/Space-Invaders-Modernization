using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInput : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKey)
        {
            Debug.Log("A key is being pressed");
        }
    }
}
