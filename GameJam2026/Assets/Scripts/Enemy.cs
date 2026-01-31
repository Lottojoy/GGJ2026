using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int _health = 100;

    public void DealDamage(int damage)
    {
        _health -= damage;
        Debug.Log($"{gameObject.name} เลือดเหลือ: {_health}");

        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}