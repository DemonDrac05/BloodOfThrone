using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [Header("=== GRAVITY SETTINGS ===========")]
    [SerializeField] private float gravityScale = 1f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float jumpLowMultiplier = 2f;

    [Header("=== GROUND CHECKING ==========")]
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D _rigidbody2D;
    private Collider2D _collider2D;

    public float verticalVelocity { get; private set; }
    public bool isGrounded { get; private set; }

    [Header("=== KNOCKBACK PROPERTIES ==========")]
    public float knockbackForce = 500f;
    public float knockbackDuration = 0.2f;

    private float knockbackTimer = 0f;
    private bool isKnockedBack = false;
    private Vector2 _knockbackDirection;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _collider2D = GetComponent<Collider2D>();

        _rigidbody2D.gravityScale = gravityScale;
    }

    private void Update()
    {
        isGrounded = Physics2D.BoxCast(_collider2D.bounds.center, _collider2D.bounds.size, 0f, Vector2.down, .1f, groundLayer);

        ApplyGravity();

        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;

            if (knockbackTimer <= 0)
            {
                isKnockedBack = false;
            }
            else
            {
                _rigidbody2D.linearVelocity = _knockbackDirection * knockbackForce * Time.deltaTime;
            }
        }
    }

    public void ApplyGravity()
    {
        if (isGrounded) return;

        if (_rigidbody2D.linearVelocityY < 0)
        {
            _rigidbody2D.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (_rigidbody2D.linearVelocityY > 0)
        {
            _rigidbody2D.linearVelocity += Vector2.up * Physics2D.gravity.y * (jumpLowMultiplier - 1) * Time.deltaTime;
        }
    }

    public void ApplyKnockback(Vector2 direction, float knockbackForce)
    {
        if (isKnockedBack) return;

        isKnockedBack = true;
        knockbackTimer = knockbackDuration;

        _knockbackDirection = direction.normalized;
        _rigidbody2D.linearVelocity = _knockbackDirection * knockbackForce;
    }
}