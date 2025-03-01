using UnityEngine;

public class PlayerPhysics : MonoBehaviour
{
    [Header("=== Layer Mask ==========")]
    [SerializeField] public LayerMask Ground;

    [Header("=== Knockback Properties ==========")]
    public float knockbackForce = 500f;
    public float knockbackDuration = 0.2f;

    private float knockbackTimer = 0f;
    private bool isKnockedBack = false;


    // --- SINGLETON CLASS ----------
    private Player player;

    private void Awake() => player = GetComponent<Player>();

    public bool IsGrounded()
        => Physics2D.BoxCast(player.Collider2D.bounds.center, player.Collider2D.bounds.size, 0f, Vector2.down, .1f, Ground);

    private void Update()
    {
        if (isKnockedBack)
        {
            knockbackTimer -= Time.deltaTime;

            if (knockbackTimer <= 0)
            {
                isKnockedBack = false;
            }
        }
    }

    public void ApplyKnockback(Vector2 direction, float knockbackForce)
    {
        if (isKnockedBack)
        {
            return;
        }

        isKnockedBack = true;
        knockbackTimer = knockbackDuration;

        player.Rigidbody2D.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
    }
}
