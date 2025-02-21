using UnityEngine;
using Cysharp.Threading.Tasks;

public class BaseStatistic : MonoBehaviour
{
    public BaseStatSO baseStatSO;

    public float MovementSpeed { get; protected set; }
    public float Health { get; protected set; }
    public float PhysicalAtk { get; protected set; }
    public float PhysicalDef { get; protected set; }
    public float PhysicalPiercing { get; protected set; }
    public float MagicalAtk { get; protected set; }
    public float MagicalDef { get; protected set; }
    public float MagicalPiercing { get; protected set; }
    public float CriticalChance { get; protected set; }
    public float CriticalDamage { get; protected set; }

    public virtual async void Awake()
    {
        MovementSpeed = baseStatSO.movementSpeed;
        Health = baseStatSO.maxHealth;
        
        PhysicalAtk = baseStatSO.physicalAtk;
        PhysicalDef = baseStatSO.physicalDef;
        PhysicalPiercing = baseStatSO.physicalPiercing;

        MagicalAtk = baseStatSO.magicalAtk;
        MagicalDef = baseStatSO.magicalDef;
        MagicalPiercing = baseStatSO.magicalPiercing;

        CriticalChance = baseStatSO.criticalChance;
        CriticalDamage = baseStatSO.criticalDamage;

        await UniTask.Yield();
    } 

    public void TakeHealthDamage(float damage)
    {
        Health = Mathf.Max(0f, Health - damage);
        if (baseStatSO is PlayerStatSO)
        {
            var player = GetComponent<Player>();
            var playerHealth = GetComponent<PlayerHealth>();

            if (playerHealth.UpdateHealth())
            {
                playerHealth.UpdateHealthFill();
                player.stateMachine.ChangeState(player.lifeState);
            }
        }
        else if (baseStatSO is EnemyStatSO)
        {
            var enemy = GetComponent<Enemy>();
            var enemyHealth = GetComponent<EnemyHealth>();

            if (enemyHealth.UpdateHealth())
            {
                enemyHealth.UpdateHealthFill();
                enemy.stateMachine.ChangeState(enemy.EnemyLifeState);
            }
        }
    }
}
