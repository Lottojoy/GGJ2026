using UnityEngine;

/// <summary>
/// ควบคุมการเคลื่อนที่แบบอิสระ (Free Movement) และระบบล็อคการควบคุม
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    private float _moveSpeed => PlayerStats.Instance.BaseSpeed;
    [SerializeField] private Animator _playerAnimator;

    private Rigidbody2D _rb;
    private Vector2 _moveInput;
    private bool _isActionLocked;

    // เก็บค่าทิศทางล่าสุดที่ผู้เล่นกด
    public Vector2 LastMoveDirection { get; private set; } = Vector2.down;
    public Vector2 CurrentInput => _moveInput;

    private string _moveDiraction;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rb.gravityScale = 0f;
        // ล็อคไม่ให้ตัวละครหมุนเมื่อชนสิ่งกีดขวาง
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Update()
    {
        if (_isActionLocked)
        {
            _moveInput = Vector2.zero;
            return;
        }

        _moveInput.x = Input.GetAxisRaw("Horizontal");
        _moveInput.y = Input.GetAxisRaw("Vertical");

        if (_moveInput != Vector2.zero)
        {
            LastMoveDirection = _moveInput.normalized;
        }

        _playerAnimator.SetBool("MoveRight", _moveInput.x > 0 ? true : false);

        _playerAnimator.SetBool("MoveLeft", _moveInput.x < 0 ? true : false);

        _playerAnimator.SetBool("MoveUp", _moveInput.y > 0 ? true : false);

        _playerAnimator.SetBool("MoveDown", _moveInput.y < 0 ? true : false);



    }

    private void FixedUpdate()
    {
        if (!_isActionLocked)
        {
            _rb.velocity = _moveInput.normalized * _moveSpeed;
        }
    }

    /// <summary>
    /// ล็อคการควบคุมของผู้เล่น (ใช้ตอน Dash หรือโจมตี)
    /// </summary>
    public void LockMovement(bool isLocked)
    {
        _isActionLocked = isLocked;
    }
}