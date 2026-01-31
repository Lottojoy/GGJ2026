using System.Collections;
using UnityEngine;

public class PlayerStats : Singleton<PlayerStats>
{
    [Header("Player Stats")]
    [SerializeField] private int _currentHp = 100;
    [SerializeField] private int _baseATK = 10;
    [SerializeField] private int _baseSpeed = 5;

    [Header("UI")]
    [SerializeField] private RectTransform _healthBar;

    [Header("Hit Effect")]
    [SerializeField] private Color _damageColor = Color.red;
    [SerializeField] private float _damageColorDuration = 2f;
    [SerializeField] private float _invulDuration = 0.5f;

    [SerializeField] private bool _alive = true;
    [SerializeField] private bool _isInvincible = false;

    public const int MAX_PLAYER_HP = 100;
    public int CurrentHp => _currentHp;
    public int BaseATK => _baseATK;
    public int BaseSpeed => _baseSpeed;
    public bool IsAlive => _alive;
    public bool IsInvincible => _isInvincible;

    [SerializeField] private float _hpLerpSpeed = 2f;
    private float _displayHp;

    // ===== Effect Vars =====
    [SerializeField] private SpriteRenderer _sprite;
    private Color _baseColor;
    private Color _currentTintColor;

    private Coroutine _damageRoutine;
    private Coroutine _invulRoutine;

    private void Start()
    {
        _displayHp = _currentHp;

        if (_sprite != null)
        {
            _baseColor = _sprite.color;
            _currentTintColor = _baseColor;
        }
    }

    private void Update()
    {
        // UI HP Bar
        _displayHp = Mathf.Lerp(_displayHp, _currentHp, Time.unscaledDeltaTime * _hpLerpSpeed);
        float targetWidth = _displayHp * 6f;
        _healthBar.sizeDelta = new Vector2(targetWidth, _healthBar.sizeDelta.y);
    }

    // ================= DAMAGE =================
    public void TakeDamage(int damage)
    {
        
        if (!_alive) return;
        if (_isInvincible) return;

        _currentHp -= damage;
        _currentHp = Mathf.Max(_currentHp, 0);

        // เอฟเฟกต์โดนตี
        if (_damageRoutine != null)
            StopCoroutine(_damageRoutine);
        _damageRoutine = StartCoroutine(DamageColorEffect());

        if (_invulRoutine != null)
            StopCoroutine(_invulRoutine);
        _invulRoutine = StartCoroutine(InvulnerabilityPeriod());

        if (_currentHp <= 0 && _alive)
        {
            _alive = false;
            Debug.Log("Player Died");
            Time.timeScale = 0f;
        }
    }

    // ================= COLOR APPLY =================
    private void ApplyColor(float alpha = 1f)
    {
        if (_sprite == null) return;
        Color c = _currentTintColor;
        c.a = alpha;
        _sprite.color = c;
    }

    // ================= DAMAGE COLOR =================
    private IEnumerator DamageColorEffect()
    {
        float half = _damageColorDuration / 2f;
        float t = 0f;

        while (t < half)
        {
            t += Time.unscaledDeltaTime;
            _currentTintColor = Color.Lerp(_baseColor, _damageColor, t / half);
            ApplyColor();
            yield return null;
        }

        t = 0f;

        while (t < half)
        {
            t += Time.unscaledDeltaTime;
            _currentTintColor = Color.Lerp(_damageColor, _baseColor, t / half);
            ApplyColor();
            yield return null;
        }

        _currentTintColor = _baseColor;
        ApplyColor();
    }

    // ================= INVUL =================
    private IEnumerator InvulnerabilityPeriod()
    {
        float timer = 0f;

        while (timer < _invulDuration)
        {
            timer += Time.unscaledDeltaTime;

            float alpha = Mathf.PingPong(timer * 8f, 1f) > 0.5f ? 0.4f : 1f;
            ApplyColor(alpha);

            yield return null;
        }
        ApplyColor(1f);
    }

    // ================= OTHER =================
    public void Heal(int healAmount)
    {

        _currentHp += healAmount;
        if (_currentHp > MAX_PLAYER_HP)
            _currentHp = MAX_PLAYER_HP;
    }

    public void SetSpeed(int speed) => _baseSpeed = speed;
    public void SetAtk(int atk) => _baseATK = atk;

    public void SetInvincible(bool value) => _isInvincible = value;
}
