using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 1f;

    private CharacterController characterController = null;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();
    }

    /// <summary>
    /// Thanks, Brackeys
    /// </summary>
    void Move()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            characterController.Move(direction * MoveSpeed * Time.deltaTime);
        }
    }
}
