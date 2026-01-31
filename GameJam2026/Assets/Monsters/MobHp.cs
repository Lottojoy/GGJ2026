using UnityEngine;
using System.Collections;

public class MobHp : MonoBehaviour, IHitDamageEffect
{
    [Header("Health Settings")]
    public int maxHp = 100;
    public int currentHp;

    [Header("Damage Color Effect")]
    public Color damageColor = Color.red;
    public float damageColorDuration = 2f; // เวลาไป-กลับรวม

    [Header("Invulnerability")]
    public float invulnerabilityDuration = 0.5f;

    [Header("Item Drops")]
    public GameObject ItemPrefab;
    [Range(0, 100)] public float ItemDropChance = 5f;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private Color baseColor;          // สีเดิม
    private Color currentTintColor;   // สีจากระบบ Damage

    private bool isInvulnerable;
    private bool isDead;

    private Coroutine damageColorRoutine;
    private Coroutine invulRoutine;

    void Awake()
    {
        currentHp = maxHp;

        if (spriteRenderer == null)
        {
            Debug.LogError("NO SpriteRenderer found on " + gameObject.name);
            return;
        }

        baseColor = spriteRenderer.color;
        currentTintColor = baseColor;
    }

    // ================= DAMAGE =================
    public void TakeDamage(int damage)
    {
        if (isDead || isInvulnerable) return;

        currentHp -= damage;
        currentHp = Mathf.Max(currentHp, 0);

        // เริ่มเอฟเฟกต์สีโดนตี
        if (damageColorRoutine != null)
            StopCoroutine(damageColorRoutine);
        damageColorRoutine = StartCoroutine(DamageColorEffect());

        // เริ่มช่วงอมตะ
        if (invulRoutine != null)
            StopCoroutine(invulRoutine);
        invulRoutine = StartCoroutine(InvulnerabilityPeriod());

        if (currentHp <= 0)
            Die();
    }

    // ================= COLOR APPLY =================
    private void ApplyColor(float alpha = 1f)
    {
        Color c = currentTintColor;
        c.a = alpha;
        spriteRenderer.color = c;
    }

    // ================= DAMAGE COLOR =================
    private IEnumerator DamageColorEffect()
    {
        float half = damageColorDuration / 2f;
        float t = 0f;

        // ค่อย ๆ แดง
        while (t < half)
        {
            t += Time.deltaTime;
            currentTintColor = Color.Lerp(baseColor, damageColor, t / half);
            ApplyColor();
            yield return null;
        }

        t = 0f;

        // ค่อย ๆ กลับ
        while (t < half)
        {
            t += Time.deltaTime;
            currentTintColor = Color.Lerp(damageColor, baseColor, t / half);
            ApplyColor();
            yield return null;
        }

        currentTintColor = baseColor;
        ApplyColor();
    }

    // ================= INVUL =================
    private IEnumerator InvulnerabilityPeriod()
    {
        isInvulnerable = true;
        float timer = 0f;

        while (timer < invulnerabilityDuration)
        {
            timer += Time.deltaTime;

            float alpha = Mathf.PingPong(timer * 8f, 1f) > 0.5f ? 0.4f : 1f;
            ApplyColor(alpha); // เปลี่ยนแค่โปร่งใส ไม่ยุ่งสี

            yield return null;
        }

        isInvulnerable = false;
        ApplyColor(1f);
    }

    // ================= DIE =================
    private void Die()
    {
        isDead = true;

        float randomValue = Random.Range(0f, 100f);
        if (randomValue <= ItemDropChance && ItemPrefab != null)
        {
            Instantiate(ItemPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
