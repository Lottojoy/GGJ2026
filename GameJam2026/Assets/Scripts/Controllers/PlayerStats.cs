using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;

public class PlayerStats : Singleton<PlayerStats>
{
    [SerializeField] private int _currentHp = 100;
    [SerializeField] private int _currentStamina = 100;

    private int _maxHp = 100;
    private int _maxStamina = 100;
    /*
    public AttackMask attackMask = null;
    public DefenseMask defenseMask = null;
    public DashMask dashMask = null;
    */
    private Coroutine _regenCoroutine = null;

    public void DealDamage(int damage)
    {
        _currentHp -= damage;
    }

    public void UseStamina(int Stamina)
    {
        _currentStamina -= Stamina;
    }

    private void Update()
    {
        if (_currentStamina < 100)
        {
            if (_regenCoroutine == null) _regenCoroutine = StartCoroutine(StaminaRegen());
        }

    }

    private IEnumerator StaminaRegen()
    {
        while (_currentStamina < _maxStamina)
        {
            yield return new WaitForSeconds(1);
            _currentStamina += 50;
        }
        
        _regenCoroutine = null ;
    }

}

