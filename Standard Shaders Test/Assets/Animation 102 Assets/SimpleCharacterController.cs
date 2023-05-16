using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SimpleCharacterController : MonoBehaviour
{
    private float _runSpeed = 7.0f;
    private float _yVelocity;
    private float _gravity = -50f;
    private float _jumpVelocity = 15f;
    private bool _grounded = true;

    private Animator _animator;

    void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector3 frameMovement = new Vector3(Input.GetAxis("Horizontal") * _runSpeed, _yVelocity);

        // Update Movement
        transform.Translate(frameMovement * Time.deltaTime);
        UpdateYMovement();

        // Update Facing
        if (frameMovement.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(frameMovement.x), 1, 1);

        // Update Animation
        _animator.SetFloat("NormalizedRunSpeed", Mathf.Abs(frameMovement.x) / _runSpeed);
        _animator.SetBool("IsGrounded", _grounded);
        _animator.SetBool("IsFalling", _yVelocity < 0);
    }

    void UpdateYMovement()
    {
        if (transform.position.y < -2.5f)
        {
            _grounded = true;
            _yVelocity = 0;
            Vector3 nudge = transform.position;
            nudge.y = -2.5f;
            transform.position = nudge;
        }

        if (!_grounded)
            _yVelocity += _gravity * Time.deltaTime;
        else
        {
            if (Input.GetButtonDown("Fire1"))
            {
                _yVelocity = _jumpVelocity;
                _grounded = false;
            }
        }
    }
}