using UnityEngine;

public class MadnessSystem : MonoBehaviour
{
    [Header("Madness Value")]
    [Range(0f, 100f)] [SerializeField] private float _madness;
    [SerializeField] private float _maxMadness = 100f;

    [Header("Madness Gain")]
    [SerializeField] private float _baseGainPerUse = 8f;
    [SerializeField] private float _sameMaskBonus = 6f;

    [Header("Madness Decay")]
    [SerializeField] private float _decayPerSecond = 5f;

    [Header("Thresholds")]
    [SerializeField] private float _riskThreshold = 40f;
    [SerializeField] private float _chaosThreshold = 70f;

    private string _lastMaskId;

    public float Madness => _madness;
    public float Madness01 => _madness / _maxMadness;

    private void Update()
    {
        // ลด Madness ตามเวลา
        if (_madness > 0f)
        {
            _madness -= _decayPerSecond * Time.deltaTime;
            _madness = Mathf.Max(0f, _madness);
        }
    }

    public void OnUseMask(string maskId, float bonus = 0f)
    {
        float gain = _baseGainPerUse + bonus;

        // ใช้ Mask เดิมซ้ำ
        if (_lastMaskId == maskId)
            gain += _sameMaskBonus;

        _madness = Mathf.Clamp(_madness + gain, 0f, _maxMadness);
        _lastMaskId = maskId;
    }

    public void ReduceMadness(float amount)
    {
        _madness = Mathf.Max(0f, _madness - amount);
    }

    public MadnessResult RollMadness()
    {
        // ถ้า Madness ต่ำ → ปลอดภัย
        if (_madness < _riskThreshold)
            return MadnessResult.Normal;

        // ยิ่งบ้า ยิ่งเสี่ยง
        float badChance = Mathf.InverseLerp(
            _riskThreshold,
            _maxMadness,
            _madness
        );

        bool isBad = Random.value < badChance;

        if (_madness >= _chaosThreshold)
            return isBad ? MadnessResult.Bad : MadnessResult.Good;

        // ช่วงกลาง: มีแต่ Good เล็กน้อย
        return isBad ? MadnessResult.Normal : MadnessResult.Good;
    }
}

public enum MadnessResult
{
    Normal,
    Good,
    Bad
}
