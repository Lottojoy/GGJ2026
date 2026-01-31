using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobAttack : MonoBehaviour
{
    public int Damage = 10;
     public string playerTag = "Player"; 

     [Header("Attack Cooldown")]
    public float attackCooldown = 1f; // เวลารอระหว่างการโจมตี
    private float lastAttackTime = 0f;
    private bool canAttack = true;
   private void OnCollisionEnter2D(Collision2D collision)
    {
         if (!collision.gameObject.CompareTag(playerTag))
            return;

            if (!canAttack)
            return;

         Debug.Log(gameObject.name + " attacked player for " + Damage + " damage");   
        /*PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(Damage);
        }*/

    }
}
