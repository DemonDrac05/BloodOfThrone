using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IComponents, IVariables
{
    [Header("=== Target Aiming ==========")]
    [SerializeField] private Transform Target;

    [SerializeField] private LayerMask Ground;

    public bool playerInAttackRange { get; private set; }

    // --- IVARIABLES -----------
    public bool IsVulnerable { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsFacingRight { get; private set; } = false;

    // --- ICOMPONENTS -----------
    public Rigidbody2D Rigidbody2D { get; private set; }
    public Collider2D Collider2D { get; private set; }
    public Animator Animator { get; private set; }

    public static AnimatorStateInfo CurrentState { get; private set; }

    // --- STATE MACHINE ----------
    public EnemyStateMachine stateMachine { get; private set; }
    public EnemyLifeState EnemyLifeState { get; private set; }
    public EnemyIdleState EnemyIdleState { get; private set; }
    public EnemyChaseState EnemyChaseState { get; private set; }
    public EnemyAttackState EnemyAttackState { get; private set; }

    // --- SINGLETON INSTANCE ----------
    private EnemyStatistic EnemyStatistic;

    private void Awake()
    {
        InitializeStateMachine();
        InitializeComponents();
        InitializeClasses();
    }

    public virtual void InitializeStateMachine()
    {
        // --- Initialize State Machine ------------
        stateMachine = new EnemyStateMachine();

        //
        EnemyLifeState = new EnemyLifeState(this, stateMachine);
        EnemyIdleState = new EnemyIdleState(this, stateMachine);
        EnemyChaseState = new EnemyChaseState(this, stateMachine);
        EnemyAttackState = new EnemyAttackState(this, stateMachine);
    }

    private void InitializeComponents()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();
        Animator = GetComponent<Animator>();
    }

    private void InitializeClasses()
    {
        EnemyStatistic = GetComponent<EnemyStatistic>();
    }

    public virtual void Start()
    {
        stateMachine.Initialize(EnemyIdleState);
        SetVulnerable(true);
        IsFacingRight = true; //Set to true if sprite is facing right by default.
    }

    public virtual void Update()
    {
        CurrentState = Animator.GetCurrentAnimatorStateInfo(0);
        stateMachine.enemyState.FrameUpdate();
    }

    private void FixedUpdate()
    {
        stateMachine.enemyState.PhysicsUpdate();
    }

    private bool CheckOnGround()
    {
        return Physics2D.BoxCast(Collider2D.bounds.center, Collider2D.bounds.size, 0f, Vector2.down, 0.1f, Ground);
    }

    public void SetPlayerInAttackRange(bool inAttackRange) => playerInAttackRange = inAttackRange;

    public void SetVulnerable(bool vulnerable) => IsVulnerable = vulnerable;

    public Player GetPlayer() => Target.GetComponent<Player>();

    public void UpdatePosition()
    {
        Vector2 moveDirection = (Target.transform.position - transform.position).normalized;
        moveDirection.y = 0f; // Don't move vertically
        float distance = Vector2.Distance(transform.position, Target.transform.position);

        if (distance > 0f) // Stop at a certain distance, adjust this value based on your game
        {
            Move(moveDirection * EnemyStatistic.MovementSpeed);
        }
        else
        {
            Move(Vector2.zero);
        }

    }

    public int Flip() => IsFacingRight ? 1 : -1;

    private void Move(Vector2 velocity)
    {
        Rigidbody2D.linearVelocity = new Vector2(velocity.x, Rigidbody2D.linearVelocity.y); // Keep y velocity
        CheckIsFacingRight(velocity);
    }

    private void CheckIsFacingRight(Vector2 velocity)
    {
        if (IsFacingRight && velocity.x < 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
        else if (!IsFacingRight && velocity.x > 0f)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f);
            transform.rotation = Quaternion.Euler(rotator);
            IsFacingRight = !IsFacingRight;
        }
    }
}

public static class AnimatorExtensions
{
    public static void ResetAllTriggers(this Animator animator)
    {
        if (animator == null) return;
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.type == AnimatorControllerParameterType.Trigger)
            {
                animator.ResetTrigger(parameter.name);
            }
        }
    }
}
