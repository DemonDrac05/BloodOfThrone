using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] public float jumpForce = 7f;
    [SerializeField] public float moveSpeed = 10f;
    [SerializeField] public LayerMask Ground;
    #endregion

    #region Private Fields
    [HideInInspector] public float posX;
    [HideInInspector] public float posY;
    [HideInInspector] public bool IsFacingRight = true;
    [HideInInspector] public Vector3 flip;

    [HideInInspector] public Rigidbody2D rb2d;
    [HideInInspector] public BoxCollider2D bc2d;
    [HideInInspector] public Animator animator;

    [HideInInspector] public EnemyStateMachine stateMachine;
    [HideInInspector] public EnemyIdleState idleState;
    [HideInInspector] public EnemyChaseState chaseState;
    [HideInInspector] public EnemyAttackState attackState;

    [HideInInspector] public bool InRangeChaseable = false;
    [HideInInspector] public bool InRangeAttackable = false;
    [HideInInspector] public float attackCDTime;
    [HideInInspector] public float attackCDDuration = 0.5f;

    [HideInInspector] public bool InRangeBounceable = false;
    [HideInInspector] public float bounceCDTime;
    [HideInInspector] public float bounceCDDuration = 5f;
    [HideInInspector] public bool isBouncing = false;
    #endregion

    #region Initialization
    private void Awake()
    {
        stateMachine = new EnemyStateMachine();
        idleState = new EnemyIdleState(this, stateMachine);
        chaseState = new EnemyChaseState(this, stateMachine);
        attackState = new EnemyAttackState(this, stateMachine);
    }

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bc2d = GetComponentInChildren<BoxCollider2D>();

        stateMachine.Initialize(idleState);
    }
    #endregion

    #region Helper Methods
    public void Move(Vector2 velocity)
    {
        rb2d.linearVelocity = velocity;
        CheckIsFacingRight(velocity);
    }
    public void CheckIsFacingRight(Vector2 velocity)
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

    public bool IsGrounded()
    {
        return Physics2D.BoxCast(bc2d.bounds.center, bc2d.bounds.size, 0f, Vector2.down, 0.1f, Ground);
    }
    #endregion

    #region Update Methods
    private void FixedUpdate()
    {
        stateMachine.enemyState.PhysicsUpdate();
        UpdateCooldown(ref bounceCDTime);
    }

    private void Update()
    {
        stateMachine.enemyState.FrameUpdate();
        UpdateRealTime();
    }

    private void UpdateRealTime()
    {
        UpdateCooldown(ref bounceCDTime);
        UpdateCooldown(ref attackCDTime);
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
    #endregion

    
}
