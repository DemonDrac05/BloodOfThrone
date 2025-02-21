using UnityEngine;

public class Henchman : Enemy
{
    [Header("=== Special Ability ==========")]
    public bool isBouncing;
    public bool playerInTriggerRange { get; private set; }

    private float DelayStaticTimer = 2f;
    private const float DelayStaticDuration = 1.5f;

    private float UltimateCooldownTimer = 2f;
    private const float UltimateCooldownDuation = 10f;

    public HenchmanUltimateState HenchmanUltimateState { get; private set; }
    public HenchmanTransformState HenchmanTransformState { get; private set; }

    public override void InitializeStateMachine()
    {
        base.InitializeStateMachine();
        HenchmanUltimateState = new HenchmanUltimateState(this, stateMachine);
        HenchmanTransformState = new HenchmanTransformState(this, stateMachine);
    }

    public override void Update()
    {
        base.Update();
        UpdateRealTime();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInTriggerRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInTriggerRange = false;
        }
    }

    private void UpdateRealTime()
    {
        UpdateCooldown(ref DelayStaticTimer);
        UpdateCooldown(ref UltimateCooldownTimer);
    }

    private void UpdateCooldown(ref float cooldownTime)
    {
        if (cooldownTime > 0f)
        {
            cooldownTime -= Time.deltaTime;
        }
        else
        {
            cooldownTime = 0f;
        }
    }

    public bool ReadyToAction() => DelayStaticTimer <= 0f;
    public void SetActionCooldown() => DelayStaticTimer = DelayStaticDuration;

    public bool ReadyToUltimate() => UltimateCooldownTimer <= 0f;
    public void SetUltimateCooldown() => UltimateCooldownTimer = UltimateCooldownDuation;
}
