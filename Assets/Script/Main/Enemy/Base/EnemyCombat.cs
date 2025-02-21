using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask playerLayer;

    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float attackHeight = 3f;

    private Enemy Enemy;
    private CombatManager CombatManager;
    private EnemyStatistic attackerStat;
    private PlayerStatistic defenderStat;

    private bool _isAttacking;

    private void Awake()
    {
        CombatManager = new CombatManager();
        Enemy = GetComponent<Enemy>();
    }

    public void TriggerAttack(float knockbackForce)
    {
        if (_isAttacking)
            return;

        _isAttacking = true;

        Vector2 size = new Vector2(attackRange, attackHeight);
        Collider2D hit = Physics2D.OverlapBox(attackPoint.position, size, 0f, playerLayer);

        if (hit != null)
        {
            if (hit.TryGetComponent(out Player player))
            {
                attackerStat = this.GetComponent<EnemyStatistic>();
                defenderStat = player.GetComponent<PlayerStatistic>();

                KnockbackPlayer(player, knockbackForce);

                if (player.IsVulnerable)
                {
                    CombatManager.ApplyPhysicDamage(attackerStat, defenderStat);
                }
                else
                {
                    PlayerCombat playerCombat = player.GetComponent<PlayerCombat>();
                    playerCombat.isSucceedBlocking = true;
                }
            }
        }

        StartCoroutine(ResetAttackFlag());
    }

    private System.Collections.IEnumerator ResetAttackFlag()
    {
        yield return new WaitForSeconds(0.3f);
        _isAttacking = false;
    }

    public void KnockbackPlayer(Player player, float knockbackForce)
    {
        float flip = 180f;
        bool flipped = transform.rotation == Quaternion.Euler(0f, flip, 0f);

        Vector2 knockbackDir = flipped ? Vector2.right : Vector2.left;
        knockbackDir = new Vector2(knockbackDir.x, 0.1f);

        PlayerPhysics playerPhysics = player.GetComponent<PlayerPhysics>();
        playerPhysics.ApplyKnockback(knockbackDir, knockbackForce);
    }

    public bool ActivateAttackPoint() => attackPoint.gameObject.activeSelf;

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.green;
        Vector2 size = new Vector2(attackRange, attackHeight);
        Gizmos.DrawWireCube(attackPoint.position, size);
    }
}