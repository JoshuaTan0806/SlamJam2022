﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 1f;
    public float Gravity = -9.8f;
    Vector3 velocity;

    public Transform GroundCheck;
    public float GroundDistance = 0.2f;
    public LayerMask Ground;
    bool isGrounded;

    private CharacterController characterController = null;
    private GameObject playerModel = null;


    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        playerModel = GetComponentInChildren<Animator>().gameObject;
    }

    void Update()
    {
        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, Ground);
        if(isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
        }

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
            playerModel.transform.forward = direction;
        }

        velocity.y += Gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
}
