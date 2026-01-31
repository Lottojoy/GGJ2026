using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllAroundAttack : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy[] enemies = other.gameObject.GetComponents<Enemy>();

            foreach (var enemy in enemies)
            {
                enemy.DealDamage(10);
            }
        }
    }
}
