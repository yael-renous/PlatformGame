using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerController : InteractableObject
{
    public static readonly string TAG_STRING = "Player";

    public float moveDistance = 0.04f;
    public float moveSpeed = 2.0f;
    public float turnSpeed = 400.0f; // Degrees per second
    public float jumpForce = 8.0f;

    private Animator _animator;
    private Rigidbody _rigidbody;
    private bool _isWalking = false;
    private Vector3 _movementInput;
    private bool _isGrounded = true;

    private float _lastHitTime = -5f;
    private readonly float _hitCooldown = 5f;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    public override string GetTag()
    {
        return TAG_STRING;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculating movement direction
        _movementInput = new Vector3(horizontalInput, 0, verticalInput).normalized;

        UpdatePlayerAnimations();
    }

    private void UpdatePlayerAnimations()
    {
        bool isMoving = _movementInput.magnitude > 0;
        if (_isWalking != isMoving)
        {
            _isWalking = isMoving;
            _animator.SetBool("IsWalking", isMoving);
        }

        // If the player presses jump and is grounded
        if (Input.GetKeyDown(KeyCode.Space) && _isGrounded && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
        {
            _animator.SetTrigger("Jump");
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _isGrounded = false;
            _animator.SetBool("IsGrounded", false);
        }
        else if (_isGrounded)
        {
            _animator.SetBool("IsGrounded", true);
        }
    }


    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        bool isHit = stateInfo.IsName("Hit");
        if (isHit)
        {
            return;
        }

        if (_isWalking)
        {
            Vector3 nextPosition = _rigidbody.position + _movementInput * (moveSpeed * Time.fixedDeltaTime);

            //Clamp
            nextPosition.x = Mathf.Clamp(nextPosition.x, GameManager.MinGameBounds.x,
                GameManager.MaxGameBounds.x);
            nextPosition.z = Mathf.Clamp(nextPosition.z, GameManager.MinGameBounds.z,
                GameManager.MaxGameBounds.z);
            _rigidbody.MovePosition(nextPosition);

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
        if (other.gameObject.CompareTag(KeyController.TAG_STRING))
        {
            GameManager.Instance.GotKey();
        }
        else if (other.gameObject.CompareTag(CoinController.TAG_STRING))
        {
            GameManager.Instance.CollectedCoin();
        }
        else if (other.gameObject.CompareTag(TrapController.TAG_STRING))
        {
            if (Time.time - _lastHitTime < _hitCooldown) return;
            _lastHitTime = Time.time;
    
            _animator.SetTrigger("Hit");
            GameManager.Instance.GotHit();
        }
        else if (other.gameObject.CompareTag(DoorController.TAG_STRING) && GameManager.Instance.HasKey)
        {
            GameManager.Instance.LevelFinished();
        }
        else if (other.gameObject.CompareTag(PlatformController.TAG_STRING))
        {
            _isGrounded = true;
        }
    }
}