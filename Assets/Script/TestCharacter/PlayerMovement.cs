using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] public float jumpForce = 14f;
    [SerializeField] public float moveSpeed = 10f;
    [SerializeField] public LayerMask Ground;

    public float posX;
    public float posY;
    public bool IsFacingRight = true;
    public Vector3 flip;

    public float jumpDelayTime;
    public float jumpDelayDuration = 0.2f;

    public Rigidbody2D rb2d;

    public BoxCollider2D bc2d;

    public Animator animator;

    public PlayerStateMachine stateMachine { get; set; }
    public MovementState movementState { get; set; }
    public BeforeJumpState beforeJumpState { get; set; }
    public AfterJumpState afterJumpState { get; set; }
    public JumpState jumpState { get; set; }
    public AttackState attackState { get; set; }
    public OpenWeapon openWeaponState { get; set; }

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        movementState = new MovementState(this, stateMachine);
        beforeJumpState = new BeforeJumpState(this, stateMachine);
        jumpState = new JumpState(this, stateMachine);
        afterJumpState = new AfterJumpState(this, stateMachine);
        attackState = new AttackState(this, stateMachine);
        openWeaponState = new OpenWeapon(this, stateMachine);
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bc2d = GetComponent<BoxCollider2D>();

        stateMachine.Initialize(movementState);
    }

    public void inputMovement(out float horizontal, out float vertical)
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
    }

    public void CheckIsFacingRight()
    {
        IsFacingRight = !IsFacingRight;

        flip = transform.localScale; flip.x = -1;

        if (flip.x > 0) transform.Rotate(0, 0, 0);
        else if (flip.x < 0) transform.Rotate(0, 180, 0);
    }

    public bool isGrounded()
    {
        return Physics2D.BoxCast(bc2d.bounds.center, bc2d.bounds.size, 0f, Vector2.down, .1f, Ground);
    }

    private void FixedUpdate()
    {
        if (posX > 0 && !IsFacingRight)
            CheckIsFacingRight();
        else if(posX < 0 && IsFacingRight)
            CheckIsFacingRight();

        stateMachine.playerState.PhysicsUpdate();
    }

    void Update()
    {
        stateMachine.playerState.FrameUpdate();
    }
}
