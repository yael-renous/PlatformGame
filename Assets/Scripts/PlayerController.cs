using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 2.0f;
    public float turnSpeed = 400.0f; // Degrees per second
    public float jumpForce = 5.0f;

    private Animator _animator;
    private Rigidbody _rigidbody;
    private bool _isWalking = false;
    private Vector3 _movementInput;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        // Getting movement input
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculating movement direction
        _movementInput = new Vector3(horizontalInput, 0, verticalInput).normalized;

        // Check if the player is moving
        bool isMoving = _movementInput.magnitude > 0;
        if (_isWalking != isMoving)
        {
            _isWalking = isMoving;
            _animator.SetBool("IsWalking", isMoving);
        }

        // Jump logic
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _animator.SetTrigger("Jump");
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void FixedUpdate()
    {
        // Move and rotate player using Rigidbody
        if (_isWalking)
        {
            // Moving the player
            _rigidbody.MovePosition(_rigidbody.position + _movementInput * (moveSpeed * Time.fixedDeltaTime));

            // Rotating the player to face the direction of movement
            if (_movementInput != Vector3.zero)
            {
                Quaternion toRotation = Quaternion.LookRotation(_movementInput, Vector3.up);
                _rigidbody.MoveRotation(Quaternion.RotateTowards(_rigidbody.rotation, toRotation,
                    turnSpeed * Time.fixedDeltaTime));
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(GameTags.Coin))
        {
            Destroy(other.gameObject);
        }
        else if (other.gameObject.CompareTag(GameTags.Key))
        {
        }
        else if (other.gameObject.CompareTag(GameTags.Door))
        {
        }
        else if (other.gameObject.CompareTag(GameTags.Trap))
        {
            _animator.SetTrigger("Hit");
            //todo freeze movement when hit
        }
    }
}