using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("=== Player Properties ==========")]
    [SerializeField] public float jumpForce = 14f;
    [SerializeField] public float moveSpeed = 10f;
    [SerializeField] public LayerMask Ground;

    [HideInInspector] public float posX;
    [HideInInspector] public float posY;
    [HideInInspector] public bool IsFacingRight = true;
    [HideInInspector] public bool AllowToFlip = true;
    [HideInInspector] public Vector3 flip;

    [HideInInspector] public Rigidbody2D rb2d;
    [HideInInspector] public Collider2D playerCollider2D;
    [HideInInspector] public Animator animator;

    [HideInInspector] public PlayerStateMachine stateMachine;
    [HideInInspector] public MoveState moveState;
    [HideInInspector] public JumpState jumpState;
    [HideInInspector] public AttackState attackState;

    [HideInInspector] public static Player player;

    private void Awake()
    {
        player = this;

        stateMachine = new PlayerStateMachine();
        moveState = new MoveState(this, stateMachine);
        jumpState = new JumpState(this, stateMachine);
        attackState = new AttackState(this, stateMachine);
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCollider2D = GetComponent<Collider2D>();

        stateMachine.Initialize(moveState);
    }

    private void FixedUpdate()
    {
        UpdateFlip();

        stateMachine.playerState.PhysicsUpdate();
    }

    private void Update()
    {
        InputMovement(out posX, out posY);

        stateMachine.playerState.FrameUpdate();
    }

    private void UpdateFlip()
    {
        if (posX > 0 && !IsFacingRight)
        {
            CheckIsFacingRight();
        }
        else if (posX < 0 && IsFacingRight)
        {
            CheckIsFacingRight();
        }
    }
    public void CheckIsFacingRight()
    {
        if (!AllowToFlip) return;

        IsFacingRight = !IsFacingRight;

        flip = transform.localScale;
        flip.x = -1;

        float yRot = flip.x > 0 ? 0f : 180f;
        transform.Rotate(0, yRot, 0);
    }
    public void InputMovement(out float horizontal, out float vertical)
    {
        horizontal = !AllowToFlip ? 0f : Input.GetAxisRaw("Horizontal");
        vertical = !AllowToFlip ? 0f : Input.GetAxisRaw("Vertical");
    }
    public bool IsGrounded()
    {
        return Physics2D.BoxCast(playerCollider2D.bounds.center, playerCollider2D.bounds.size, 0f, Vector2.down, .1f, Ground);
    }
}
