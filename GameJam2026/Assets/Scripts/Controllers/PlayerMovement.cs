using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Assign")]
    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private Collider2D _playerCollider;
    [SerializeField] private Transform _playerTransform;

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
        if (_rigidbody2D.velocity != Vector2.zero)
        {
            _horizontalInput = 0;
            _verticalInput = 0;
            return;
        }
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
        _playerTransform.transform.position = new Vector2(
            _playerTransform.position.x + _horizontalInput * 0.25f,
            _playerTransform.position.y + _verticalInput * 0.25f
            );
    }
}
