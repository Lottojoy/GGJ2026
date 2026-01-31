using UnityEngine;
using System.Collections;

public class ThunderDash : DashMask
{
    protected override IEnumerator PerformDashRoutine(Vector3 dir)
    {
        // 1. ล็อคระบบเดินปกติทันที
        _movement.LockMovement(true);

        // 2. ดึงพิกัดปัจจุบันของผู้เล่นมาเป็นจุดเริ่มต้น (ห้ามใช้ค่าจาก MovePoint เพราะมันอาจจะลอยอยู่ไกล)
        // เราจะปัดเศษให้เป็นจำนวนเต็มเพื่อความแม่นยำของ Grid
        Vector3 startPos = new Vector3(
            Mathf.Round(transform.position.x),
            Mathf.Round(transform.position.y),
            0
        );

        // 3. บังคับให้ทั้งคู่มาอยู่ที่จุดเดียวกันก่อนเริ่มพุ่ง
        transform.position = startPos;
        _movement.MovePoint.position = startPos;

        // รอ 1 เฟรมเพื่อให้ตำแหน่งใน Engine นิ่งสนิท
        yield return new WaitForFixedUpdate();

        // 4. คำนวณหา "จุดหมายสุดท้าย" ในตัวแปรชั่วคราว (ห้ามแก้ค่า MovePoint ใน Loop นี้)
        Vector3 finalDestination = startPos;
        Vector3 dashDirection = new Vector3(Mathf.Round(dir.x), Mathf.Round(dir.y), 0);

        for (int i = 0; i < _dashDistance; i++)
        {
            Vector3 potentialStep = finalDestination + dashDirection;

            // เช็คสิ่งกีดขวาง
            bool isBlocked = Physics2D.OverlapCircle(potentialStep, 0.2f, _movement.ObstacleLayer);

            if (!isBlocked)
            {
                finalDestination = potentialStep;
            }
            else
            {
                break; // ติดกำแพง หยุดคำนวณ
            }
        }

        // 5. เมื่อได้จุดหมายที่ถูกต้องแล้ว ค่อยส่งค่าให้ MovePoint ครั้งเดียว!
        _movement.MovePoint.position = finalDestination;

        // 6. ตั้งความเร็วและรอจนกว่าจะพุ่งถึง
        float originalSpeed = 5f;
        if (PlayerStats.Instance != null) originalSpeed = PlayerStats.Instance.BaseSpeed;

        _movement.SetSpeed(_dashSpeed);

        // วนลูปเช็คจนกว่าระยะห่างจะเหลือน้อย (IsAtTarget)
        while (!_movement.IsAtTarget)
        {
            yield return null;
        }

        // 7. คืนค่าสถานะปกติ
        _movement.SetSpeed(originalSpeed);
        _movement.LockMovement(false);
    }
}