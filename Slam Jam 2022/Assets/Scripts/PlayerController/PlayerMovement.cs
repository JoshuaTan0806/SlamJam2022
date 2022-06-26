using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float Gravity = -9.8f;
    Vector3 velocity;

    public Transform GroundCheck;
    public float GroundDistance = 0.2f;
    public LayerMask Ground;
    bool isGrounded;

    private CharacterController characterController = null;
    private GameObject playerModel = null;

    Player player;

    CharacterBody body;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        player = GetComponent<Player>();
        playerModel = GetComponentInChildren<Animator>().gameObject;
        body = playerModel.GetComponent<CharacterBody>();
    }

    void Update()
    {
        if (beingKnocked)
        {
            elapsedKnockback += Time.deltaTime;

            if(elapsedKnockback > knockbackTime)
            {
                velocity -= cachedVelocity[0];
                cachedVelocity.RemoveAt(0);

                beingKnocked = cachedVelocity.Count > 0;
            }
        }

        if (player.Dead)
            return;

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
            characterController.Move(direction * GetComponent<Player>().GetStat(Stat.Spd).TotalValue * Time.deltaTime);
            playerModel.transform.forward = direction;
        }

        velocity.y += Gravity * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    bool beingKnocked;
    float knockbackTime;
    float elapsedKnockback;
    List<Vector3> cachedVelocity = new List<Vector3>();
    public void AddVelocity(Vector3 velocity, float knockbackTime)
    {
        beingKnocked = true;

        cachedVelocity.Add(velocity);

        this.velocity += velocity;

        this.knockbackTime = knockbackTime;
        elapsedKnockback = 0;
    }
}
