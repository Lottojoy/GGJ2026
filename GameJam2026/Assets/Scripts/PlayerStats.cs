using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Rendering;

public class PlayerStats : Singleton<PlayerStats>
{
    [SerializeField] private int _currentHp = 100;
    [SerializeField] private int _baseATK = 10;
    [SerializeField] private int _baseSpeed = 5;

    private bool _alive = true;

    public const int MAX_PLAYER_HP = 100;
    public int CurrentHp => _currentHp;
    public int BaseATK => _baseATK;
    public int BaseSpeed => _baseSpeed;
    public bool IsAlive => _alive;

    public void Heal(int healAmount)
    {
        _currentHp += healAmount;
        if (_currentHp > MAX_PLAYER_HP)
        {
            _currentHp = MAX_PLAYER_HP;
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHp -= damage;

        if (_currentHp <= 0 && _alive)
        {
            _alive = false;
            _currentHp = 0;
            Debug.Log("Player Died");
        }

    }

    public void SetSpeed(int speed)
    {
        _baseSpeed = speed;
    }

    public void SetAtk(int atk)
    {
        _baseATK = atk;
    }

}

