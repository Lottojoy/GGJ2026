using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobMover : MonoBehaviour
{
    public float moveSpeed = 5f;

    [Header("Separation")]
    public float separationDistance = 1f;
    public float separationStrength = 2f;

    [Header("Visual")]
    public SpriteRenderer spriteRenderer;
    public bool switchFilp = false;

    [Header("Boss Detection")]
    public bool isBoss = false;        // ✔ Boss เท่านั้น
    public float detectRadius = 6f;    // รัศมีมองเห็น Player

    private Transform player;
    private Rigidbody2D rb;
    private bool hasAggro = false;     // Boss เคยเห็น Player แล้วหรือยัง

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }

    void FixedUpdate()
    {
        if (player == null) return;

        // ---------------- Boss Detection ----------------
        if (isBoss)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance <= detectRadius)
            {
                hasAggro = true; // เจอแล้ว ไล่ยาว
            }

            if (!hasAggro)
            {
                rb.velocity = Vector2.zero;
                return; // ยังไม่เห็น Player → ไม่เดิน
            }
        }

        // ---------------- Move Logic ----------------
        Vector2 directionToPlayer = (player.position - transform.position).normalized;
        Vector2 separation = Vector2.zero;

        GameObject[] allMobs = GameObject.FindGameObjectsWithTag("Mob");

        foreach (GameObject mob in allMobs)
        {
            if (mob == this.gameObject) continue;

            float distance = Vector2.Distance(transform.position, mob.transform.position);
            if (distance < separationDistance)
            {
                Vector2 pushDir = (transform.position - mob.transform.position).normalized;
                separation += pushDir * (separationDistance - distance);
            }
        }

        Vector2 finalDir = (directionToPlayer + separationStrength * separation).normalized;
        rb.velocity = finalDir * moveSpeed;

        FacePlayer();
    }

    private void FacePlayer()
    {
        if (player == null) return;

        bool playerOnRight = player.position.x > transform.position.x;

        if (switchFilp)
            spriteRenderer.flipX = playerOnRight;
        else
            spriteRenderer.flipX = !playerOnRight;
    }

    // 🟢 วาดวงรัศมีใน Scene (Debug)
    private void OnDrawGizmosSelected()
    {
        if (!isBoss) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }

    public void SetMoveSpeed(float newSpeed)
{
    moveSpeed = newSpeed;
}

public float GetMoveSpeed()
{
    return moveSpeed;
}
}
