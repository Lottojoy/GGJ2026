using UnityEngine;

/// <summary>
/// คลาสฐานสำหรับหน้ากากจัดการ UI Cooldown และการรับ Hotkey
/// </summary>
public abstract class MaskBase : MonoBehaviour
{
    public enum MaskHotkey { Mouse0, Mouse1, Space, F }

    [Header("UI & Mask Settings")]
    [SerializeField] protected RectTransform _maskCD;
    [SerializeField] protected float _cooldownTime = 5f;
    [SerializeField] protected MaskHotkey _hotkey;

    protected float _currentCD;
    protected bool _isReady = true;

    // ⭐ Madness System (ใช้กับทุก Mask)
    protected MadnessSystem _madness;

    protected virtual void Start()
    {
        if (_maskCD != null) _maskCD.gameObject.SetActive(false);

        // หา MadnessSystem ครั้งเดียว
        _madness = FindAnyObjectByType<MadnessSystem>();
    }

    protected virtual void Update()
    {
        if (!_isReady)
        {
            _currentCD -= Time.deltaTime;
            if (_maskCD != null)
                _maskCD.sizeDelta = new Vector2(100, 100f * (_currentCD / _cooldownTime));

            if (_currentCD <= 0)
            {
                _isReady = true;
                if (_maskCD != null) _maskCD.gameObject.SetActive(false);
            }
        }

        if (IsHotkeyPressed() && _isReady && CanUseMask())
        {
            // ⭐ ใช้ Mask ใด ๆ → Madness เพิ่ม
            _madness?.OnUseMask(this.GetType().Name);

            UseMask();
        }
    }

    private bool IsHotkeyPressed()
    {
        switch (_hotkey)
        {
            case MaskHotkey.Mouse0: return Input.GetMouseButtonDown(0);
            case MaskHotkey.Mouse1: return Input.GetMouseButtonDown(1);
            case MaskHotkey.Space: return Input.GetKeyDown(KeyCode.Space);
            case MaskHotkey.F: return Input.GetKeyDown(KeyCode.F);
            default: return false;
        }
    }

    protected void StartCooldown()
    {
        _isReady = false;
        _currentCD = _cooldownTime;
        if (_maskCD != null)
        {
            _maskCD.gameObject.SetActive(true);
            _maskCD.sizeDelta = new Vector2(100, 100);
        }
    }

    protected virtual bool CanUseMask() => true;
    protected abstract void UseMask();

    // ================= MADNESS HELPERS =================

    protected MadnessResult RollMadness()
    {
        return _madness != null
            ? _madness.RollMadness()
            : MadnessResult.Normal;
    }

    protected int ApplyMadnessToDamage(int baseDamage)
    {
        MadnessResult result = RollMadness();

        switch (result)
        {
            case MadnessResult.Good:
                return baseDamage * 2;

            case MadnessResult.Bad:
                PlayerStats.Instance.TakeDamage(5);
                return Mathf.Max(1, baseDamage / 2);

            default:
                return baseDamage;
        }
    }
}
