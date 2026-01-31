
using UnityEngine;

public class BossLaserDamage : MonoBehaviour
{
    public int damage = 5;
    public float damageInterval = 0.5f;

    private float nextDamageTime = 0f;

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other.name);
        if (!other.gameObject.CompareTag("Player")) return;

        if (PlayerStats.Instance == null) return;

        if (Time.time >= nextDamageTime)
        {
            PlayerStats.Instance.TakeDamage(damage);
            nextDamageTime = Time.time + damageInterval;
        }
    }
}
