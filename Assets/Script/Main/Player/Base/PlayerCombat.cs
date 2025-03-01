using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private float attackRange = 3f;
    [SerializeField] private float attackHeight = 3f;

    private CombatManager CombatManager;
    private PlayerStatistic attackerStat;
    private EnemyStatistic defenderStat;

    private bool _isAttacking;

    [HideInInspector] public bool isSucceedBlocking = false;

    private void Awake()
    {
        CombatManager = new CombatManager();
    }

    public void TriggerAttack()
    {
        if (_isAttacking)
            return;

        _isAttacking = true;

        Vector2 size = new Vector2(attackRange, attackHeight);
        Collider2D hit = Physics2D.OverlapBox(attackPoint.position, size, 0f, enemyLayer);

        if (hit != null)
        {
            if (hit.TryGetComponent(out Enemy enemy))
            {
                attackerStat = this.GetComponent<PlayerStatistic>();
                defenderStat = enemy.GetComponent<EnemyStatistic>();

                if (enemy.IsVulnerable)
                {
                    CombatManager.ApplyPhysicDamage(attackerStat, defenderStat);
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

    public bool ActivateAttackPoint() => attackPoint.gameObject.activeSelf;

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.green;
        Vector2 size = new Vector2(attackRange, attackHeight);
        Gizmos.DrawWireCube(attackPoint.position, size);
    }
}