using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] protected int _currenHealth = 100;

    public void DealDamage(int damage)
    {
        _currenHealth -= damage;
        if (_currenHealth <= 0)
        {
            Destroy(this.gameObject);
        }

    }
}
