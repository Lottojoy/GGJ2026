using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : Singleton<PlayerStats>
{
    [SerializeField] private int _currentHp = 100;
    [SerializeField] private int _currentStamina = 100;

    private int _maxHp = 100;
    private int _maxStamina = 100;

    public void DealDamage(int damage)
    {
        _currentHp -= damage;
    }

    public void UseStamina(int Stamina)
    {
        _currentStamina -= Stamina;
    }
}
