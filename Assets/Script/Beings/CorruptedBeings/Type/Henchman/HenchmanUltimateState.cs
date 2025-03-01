using UnityEngine;

public class HenchmanUltimateState : EnemyState
{
    private EnemyStatistic HenchmanStatistic;
    private EnemyCombat HenchmanCombat;
    private Henchman Henchman;
    private Player Player;

    private float CriticalChance;
    private float PhysicalPiercing;

    private bool performAttacked = false;

    public HenchmanUltimateState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
        HenchmanCombat = enemy.GetComponent<EnemyCombat>();
        HenchmanStatistic = enemy.GetComponent<EnemyStatistic>();

        if (enemy is Henchman) Henchman = (Henchman)enemy;
        if (Henchman != null) Player = Henchman.GetPlayer();

        CriticalChance = HenchmanStatistic.CriticalChance;
        PhysicalPiercing = HenchmanStatistic.PhysicalPiercing;
    }

    public override void EnterState()
    {
        Henchman.SetVulnerable(false);
        performAttacked = false;
    }

    public override void ExitState()
    {
        SetStatistic(CriticalChance, PhysicalPiercing);
        Henchman.SetVulnerable(true);
        performAttacked = false;

    }

    public override void FrameUpdate()
    {
        if (Henchman.isBouncing)
        {
            Henchman.Rigidbody2D.linearVelocity = new Vector2(Henchman.Flip() * 10f, Henchman.Rigidbody2D.linearVelocity.y);
        }
        else
        {
            Henchman.Rigidbody2D.linearVelocity = Vector3.zero;
        }
        if (!Henchman.Animator.GetCurrentAnimatorStateInfo(0).IsName("Bounce"))
        {
            Henchman.SetUltimateCooldown();
            Henchman.SetActionCooldown();

            Henchman.stateMachine.ChangeState(Henchman.EnemyIdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        if (HenchmanCombat.ActivateAttackPoint() && !performAttacked)
        {
            performAttacked = true;

            SetStatistic(1f, 1f);
            HenchmanCombat.TriggerAttack(10f);
        }
        else if (Henchman.playerInTriggerRange && !performAttacked)
        {
            performAttacked = true;

            SetStatistic(0f, 0f);
            if (Player.IsVulnerable)
            {
                PlayerStatistic playerStat = Player.GetComponent<PlayerStatistic>();
                playerStat.TakeHealthDamage(HenchmanStatistic.PhysicalAtk + (0.05f * playerStat.Health));
            }
            HenchmanCombat.KnockbackPlayer(Player, 0.5f);
        }
    }

    void SetStatistic(float critChance, float physicPierce)
    {
        HenchmanStatistic.SetCriticalChance(critChance);
        HenchmanStatistic.SetPhysicalPiercing(physicPierce);
    }
}
