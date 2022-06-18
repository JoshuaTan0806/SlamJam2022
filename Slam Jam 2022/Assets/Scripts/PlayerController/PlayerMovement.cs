using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{

    public float MoveSpeed = 1f;
    private float currentVertMoveSpeed;
    private float currentHozMoveSpeed;

    public float TurnSpeed = 10f;
    public float rotationAcceleration = 0.1f;
    private float currentRotSpeed;

    private float lastVertAxisValue;
    private float lastHozAxisValue;

    public float acceleration;
    public int scorePlace = 0;
    public int controller = 0;

    private string verticalAxis = "Vertical";
    private string horizontalAxis = "Horizontal";
    private CharacterController characterController = null;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        Move();

        Rotate();
    }

    void Move()
    {   //You probably want to merge vert and horizontal move.
        VertMove();

        HozMove();
    }

    void VertMove()
    {
        //Player Vertical Movement
        float axis = Input.GetAxis(verticalAxis);

        //Make sure the play wants to accelerate in a direction
        if (axis != 0)
        {
            //Store for reference when the player no-longer wants to move
            lastVertAxisValue = axis;
            //Find the speed the player wants
            float expectedSpeed = MoveSpeed * axis;
            //Get the vertical moveSpeed
            currentVertMoveSpeed = SetSpeed(currentVertMoveSpeed, acceleration, expectedSpeed, MoveSpeed, axis);
            //Moves the player on the vertical axis
            transform.Translate(Vector3.up * (currentVertMoveSpeed * Time.deltaTime), Space.World);
        }
        else
        {
            //Determines if the player is moving forward or backwards, left or right
            if (currentVertMoveSpeed > 0f)
            {
                //Deceleration on vertical axis
                currentVertMoveSpeed -= acceleration * Time.deltaTime;

            }
            else if (currentVertMoveSpeed < 0f)
            {
                //Accelerate on vertical axis
                currentVertMoveSpeed += acceleration * Time.deltaTime;

            }
            //Moves the player
            transform.Translate(Vector3.up * (currentVertMoveSpeed * Time.deltaTime * Mathf.Abs(lastVertAxisValue)), Space.World);
        }
    }

    void HozMove()
    {
        //Player Horizontal Movement
        float axis = Input.GetAxis(horizontalAxis);
        if (axis != 0)
        {
            //Get the expected speed of the player
            float expectedSpeed = MoveSpeed * axis;
            //Previous player input speed
            lastHozAxisValue = axis;

            //Set the players speed
            currentHozMoveSpeed = SetSpeed(currentHozMoveSpeed, acceleration, expectedSpeed, MoveSpeed, axis);
            //Moves the player on the horizontal axis
            transform.Translate(Vector3.left * (currentHozMoveSpeed * Time.deltaTime), Space.World);
        }
        else
        {
            if (currentHozMoveSpeed > 0f)
            {
                //Deceleration on horizontal axis
                currentHozMoveSpeed -= acceleration * Time.deltaTime;

                if (currentHozMoveSpeed < 0)
                {
                    currentHozMoveSpeed = 0;
                }
            }
            else if (currentHozMoveSpeed < 0f)
            {
                //Accelerate on horizontal axis
                currentHozMoveSpeed += acceleration * Time.deltaTime;

                if (currentHozMoveSpeed > 0)
                {
                    currentHozMoveSpeed = 0;
                }
            }
            transform.Translate(Vector3.left * (currentHozMoveSpeed * Time.deltaTime * Mathf.Abs(lastHozAxisValue)), Space.World);
        }
    }

    float SetSpeed(float currentSpeed, float acceleration, float expectedSpeed, float maxSpeed, float axisValue)
    {

        //Which direction does the player want to go?
        if (axisValue > 0)
        {
            //Check to see if we are going in the correct direction
            if (currentSpeed >= 0)
            {
                //Check to see if we need to accelerate or decelerate the player
                if (currentSpeed < expectedSpeed)
                {
                    currentSpeed += acceleration * Time.deltaTime;
                }
                else
                {
                    currentSpeed -= acceleration * Time.deltaTime;
                }
            }
            else
            {
                //Accelerate the player at twice the rate to enable quick turning
                currentSpeed += acceleration * 2 * Time.deltaTime;
            }
        }
        else
        {
            //Is the player travelling in the correct direction
            if (currentSpeed <= 0)
            {
                //Is the player going faster than intended?
                if (currentSpeed > expectedSpeed)
                {
                    currentSpeed -= acceleration * Time.deltaTime;
                }
                else
                {
                    currentSpeed += acceleration * Time.deltaTime;
                }
            }
            else
            {
                //Decelerate the player at twice the rate
                currentSpeed -= acceleration * 2 * Time.deltaTime;
            }
        }

        //Make sure the player is not exceeding the speed limit
        if (currentSpeed > maxSpeed)
        {
            currentSpeed = maxSpeed;
        }
        else if (currentSpeed < -maxSpeed)
        {
            currentSpeed = -maxSpeed;
        }

        return currentSpeed;
    }

    void Rotate()
    {   //Old rotation code. Not what we want presumably
        //Makes sure the player wants to adjust their rotation
        /*if (Input.GetAxis(horizontalAxis) == 0f && Input.GetAxis(rotateAxis) == 0f)
        {
            //Either decelerate or acelerate the rotation until the player no longer rotates
            if (currentRotSpeed > 0f)
            {
                currentRotSpeed -= rotationAcceleration * Time.deltaTime;

                if (currentRotSpeed < 0f)
                {
                    currentRotSpeed = 0f;
                }
            }
            else if (currentRotSpeed < 0f)
            {
                currentRotSpeed += rotationAcceleration * Time.deltaTime;

                if (currentRotSpeed > 0f)
                {
                    currentRotSpeed = 0f;
                }
            }

        }
        else
        {
            //Find the angles;
            float targetAngle = Mathf.Atan2(Input.GetAxis(horizontalAxis), Input.GetAxis(rotateAxis)) * Mathf.Rad2Deg;
            float angleDif = Mathf.DeltaAngle(transform.rotation.eulerAngles.z, targetAngle);
            //Either increase or decrease the speed depending on the rotation direction
            if (angleDif < 0)
            {
                if (currentRotSpeed > 0)
                {
                    currentRotSpeed -= rotationAcceleration * 2 * Time.deltaTime;
                }
                else
                {
                    currentRotSpeed -= rotationAcceleration * Time.deltaTime;
                }

                if (currentRotSpeed < -TurnSpeed)
                {
                    currentRotSpeed = -TurnSpeed;
                }
            }
            else
            {
                if (currentRotSpeed < 0)
                {
                    currentRotSpeed += rotationAcceleration * 2 * Time.deltaTime;
                }
                else
                {
                    currentRotSpeed += rotationAcceleration * Time.deltaTime;
                }
                currentRotSpeed += rotationAcceleration * Time.deltaTime;

                if (currentRotSpeed > TurnSpeed)
                {
                    currentRotSpeed = TurnSpeed;
                }
            }
        }
        //Rotates the player towards the direction
        transform.Rotate(Vector3.forward, currentRotSpeed);*/
    }
}
