using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private RectTransform _healthBarRoot;
    [SerializeField] private Image _healthBarFill;

    private float _maxHealth;
    private float _currentHealth;

    private PlayerStatistic _playerStatistic;

    private void Awake()
    {
        _playerStatistic = GetComponent<PlayerStatistic>();
    }

    private void Start()
    {
        if (_playerStatistic != null)
        {
            SetMaxHealth(_playerStatistic.Health);
        }
    }

    private void SetMaxHealth(float val)
        => _maxHealth = _currentHealth = val;

    public bool UpdateHealth()
    {
        if (_currentHealth > _playerStatistic.Health)
        {
            _currentHealth = _playerStatistic.Health;
            return true;
        }
        return false;
    }

    public void UpdateHealthFill()
        => _healthBarFill.fillAmount = (float)(_currentHealth / _maxHealth);

    public bool IsDead() => _currentHealth <= 0f;
}
