using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    [Header("Icon")]
    [SerializeField] private Image _attackSkill;
    [SerializeField] private Image _defenseSkill;
    [SerializeField] private Image _dashSkill;

    [Header("CD")]
    [SerializeField] private Image _attackCD;
    [SerializeField] private Image _defenseCD;
    [SerializeField] private Image _dashCD;

    [Header("Player")]
    [SerializeField] private SpriteRenderer _playerSprite;
    [SerializeField] private Rigidbody2D _playerRB2;
    [SerializeField] private float _dashForce = 50;

    private float _horizontalInput;
    private float _verticalInput;

    private Coroutine _attackCoroutineCD;
    private Coroutine _defenseCoroutineCD;
    private Coroutine _dashCoroutineCD;
    private Coroutine _stopDashCoroutine;

    private void Start()
    {
        _attackCD.gameObject.SetActive(false);
        _defenseCD.gameObject.SetActive(false);
        _dashCD.gameObject.SetActive(false);
    }

    private void Update()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            DefenseBehavior();
            DefenseCD();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DashBehavior();
            DashCD();
        }

    }

    

    public void DefenseBehavior() 
    {
        
    }

    public void DashBehavior()
    {
        Vector2 moveInput = new Vector2(_horizontalInput, _verticalInput);

        if (moveInput != Vector2.zero)
        {
            Vector2 dashDirection = moveInput.normalized;
            _playerRB2.AddForce(dashDirection * _dashForce, ForceMode2D.Impulse);

            if (_stopDashCoroutine == null) StartCoroutine(StopDesh(0.25f));
        }

        
    }

    public void AttackCD()
    {

    }

    public void DefenseCD()
    {
        
    }

    public void DashCD()
    {
        _dashCD.gameObject.SetActive(true);
        _dashCD.rectTransform.sizeDelta = new Vector2(100, 100);

        if (_dashCoroutineCD == null) _dashCoroutineCD = StartCoroutine(DashCDCorotine(2));
    }

    private IEnumerator DashCDCorotine(float cdTime)
    {
        float elapsedTime = 0f;
        while (elapsedTime < cdTime)
        {
            elapsedTime += Time.deltaTime;
            var size = 100 - (100 * (elapsedTime / cdTime));
            _dashCD.rectTransform.sizeDelta = new Vector2(100, size);
            yield return null;
        }
        _dashCD.rectTransform.sizeDelta = new Vector2(100, 0);
        _dashCD.gameObject.SetActive(false);
        _dashCoroutineCD = null;
    }

    private IEnumerator StopDesh(float time)
    {
        yield return new WaitForSeconds(time);
        _playerRB2.velocity = Vector2.zero;
    }

    
}
