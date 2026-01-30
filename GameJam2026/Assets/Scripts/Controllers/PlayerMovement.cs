using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Collider2D _playerCollider;

    [Header("Settings")]
    [Range(1f, 10f)]
    [SerializeField] private float _moveSpeed = 5f;

    [Header("PlayerStats")]
    [SerializeField] private float _verticalInput;
    [SerializeField] private float _horizontalInput;
    private void OnValidate()
    {
        if (_rigidbody2D == null) _rigidbody2D = this.GetComponent<Rigidbody2D>();

        if (_playerCollider == null) _playerCollider = this.GetComponentInChildren<Collider2D>();
    }

    private void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        MoveLogic();
    }


    //============================
    //functions
    //============================

    private void MoveLogic()
    {
        if (_horizontalInput != 0)
        {
            _rigidbody2D.velocity = new Vector2(_horizontalInput * _moveSpeed, _rigidbody2D.velocity.y);
        }
        else
        {
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
        }

        if (_verticalInput != 0)
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, _verticalInput * _moveSpeed);
        }
        else
        {
            _rigidbody2D.velocity = new Vector2(_rigidbody2D.velocity.x, 0);
        }
    }
}
