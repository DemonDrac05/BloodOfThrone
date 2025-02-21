using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private RectTransform _healthBarRoot;
    [SerializeField] private Image _healthBarFill;
    [SerializeField] private bool isBoss;

    private Vector3 offset = Vector3.up;
    private float visibilityDistance = 15f;

    private float _maxHealth;
    private float _currentHealth;

    private EnemyStatistic _enemyStatistic;
    private Camera mainCamera;

    private void Awake()
    {
        _enemyStatistic = GetComponent<EnemyStatistic>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        if (_enemyStatistic != null)
        {
            SetMaxHealth(_enemyStatistic.Health);
        }
    }

    private void LateUpdate()
    {
        if (!isBoss)
        {
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(transform.position + offset);

            float distanceFromCamera = Vector3.Distance(mainCamera.transform.position, transform.position);

            _healthBarRoot.gameObject.SetActive(distanceFromCamera <= visibilityDistance);

            _healthBarRoot.position = screenPosition;
        }
    }

    private void SetMaxHealth(float val)
        => _maxHealth = _currentHealth = val;

    public bool UpdateHealth()
    {
        if (_currentHealth > _enemyStatistic.Health)
        {
            _currentHealth = _enemyStatistic.Health;
            return true;
        }
        return false;
    }

    public void UpdateHealthFill()
        => _healthBarFill.fillAmount = (float)(_currentHealth / _maxHealth);

    public bool IsDead() => _currentHealth <= 0f;
}
