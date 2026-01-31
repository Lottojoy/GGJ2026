using System.Collections;
using UnityEngine;

public class MobAttack : MonoBehaviour
{
    public int Damage = 10;
    public string playerTag = "Player";

    [Header("Attack Cooldown")]
    private float attackCooldown = 2f; // ตีทุกๆ 2 วิ

    private float nextAttackTime = 0f;

    [SerializeField] private Animator _attackAnimator1;
    [SerializeField] private Animator _attackAnimator2;
    [SerializeField] private GameObject _attackEffect;
    [SerializeField] private float _attackTime = 0.3f;

    private Coroutine _attackCoroutine;

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(playerTag))
            return;

        // เช็คเวลาคูลดาวน์
        if (Time.time >= nextAttackTime)
        {
            AttackPlayer();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    private void AttackPlayer()
    {
        if (PlayerStats.Instance == null) return;

        if (_attackEffect != null) _attackEffect.SetActive(true);

        if (_attackCoroutine == null) StartCoroutine(AttackTime(_attackTime));

        if (_attackAnimator1 != null)  _attackAnimator1.SetBool("Hit", true);

        if (_attackAnimator2 != null) _attackAnimator2.SetBool("Hit", true);


        Debug.Log(gameObject.name + " attacked player for " + Damage + " damage");
        PlayerStats.Instance.TakeDamage(Damage);
    }

    private IEnumerator AttackTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (_attackEffect != null) _attackEffect.SetActive(false);

        _attackCoroutine = null;

    }
}
