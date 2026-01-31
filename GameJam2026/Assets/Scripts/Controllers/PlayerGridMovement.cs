using UnityEngine;

/// <summary>
/// ควบคุมการเคลื่อนที่ของผู้เล่นแบบ Grid และจัดการระบบ MovePoint
/// </summary>
public class PlayerGridMovement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private Transform _movePoint;
    [SerializeField] private LayerMask _obstacleLayer;

    private bool _isActionLocked;

    public Transform MovePoint => _movePoint;
    public LayerMask ObstacleLayer => _obstacleLayer;
    public bool IsAtTarget => Vector3.Distance(transform.position, _movePoint.position) <= 0.05f;

    private void Start()
    {
        if (_movePoint != null)
        {
            // บังคับให้ MovePoint อยู่ที่เท้าผู้เล่นทันทีเมื่อเริ่มเกมเพื่อกันพิกัดกระโดด
            _movePoint.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);
            _movePoint.parent = null;
        }
    }

    private void Update()
    {
        // ตัวละครวิ่งหา MovePoint ตลอดเวลา
        transform.position = Vector3.MoveTowards(transform.position, _movePoint.position, _moveSpeed * Time.deltaTime);

        // รับ Input เดินปกติ (เฉพาะตอนที่ถึงจุดหมายและไม่ได้โดนล็อค)
        if (IsAtTarget && !_isActionLocked)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");

            if (h == 0 && v == 0) return;

            Vector3 moveDir = new Vector3(h, v, 0);

            if (Mathf.Abs(h) != Mathf.Abs(v)) Move(moveDir);
            else MoveDiagonal(moveDir);
        }
    }

    private void Move(Vector3 dir)
    {
        if (!Physics2D.OverlapCircle(_movePoint.position + dir, 0.2f, _obstacleLayer))
            _movePoint.position += dir;
    }

    private void MoveDiagonal(Vector3 dir)
    {
        bool targetBlocked = Physics2D.OverlapCircle(_movePoint.position + dir, 0.2f, _obstacleLayer);
        bool sideXBlocked = Physics2D.OverlapCircle(_movePoint.position + new Vector3(dir.x, 0, 0), 0.2f, _obstacleLayer);
        bool sideYBlocked = Physics2D.OverlapCircle(_movePoint.position + new Vector3(0, dir.y, 0), 0.2f, _obstacleLayer);

        if (!targetBlocked && !sideXBlocked && !sideYBlocked) _movePoint.position += dir;
        else if (!sideXBlocked) Move(new Vector3(dir.x, 0, 0));
        else if (!sideYBlocked) Move(new Vector3(0, dir.y, 0));
    }

    public void SetSpeed(float newSpeed) => _moveSpeed = newSpeed;
    public void LockMovement(bool isLocked) => _isActionLocked = isLocked;
}