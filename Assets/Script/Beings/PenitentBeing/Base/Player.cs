using UnityEngine;

public class Player : MonoBehaviour, IComponents, IVariables
{

    // --- PROPERTIES ----------
    public bool IsVulnerable { get; private set; }
    public bool IsGrounded { get; private set; }
    public bool IsFlippable { get; private set; }

    // --- ICOMPONENTS -----------
    public Rigidbody2D Rigidbody2D { get; private set; }
    public Collider2D Collider2D { get; private set; }
    public Animator Animator { get; private set; }

    public static AnimatorStateInfo CurrentState { get; private set; }

    // --- STATE MACHINE ----------
    public PlayerStateMachine stateMachine { get; private set; }
    public LifeState lifeState { get; private set; }
    public MoveState moveState { get; private set; }
    public JumpState jumpState { get; private set; }
    public BlockState blockState { get; private set; }
    public AttackState attackState { get; private set; }

    // --- SINGLETON INSTANCE ----------
    public static Player player { get; private set; }

    // --- INPUT ACTIONS REFERENCE ----------
    public InputSystem_Actions Inputs;

    private void Awake()
    {
        player = this;

        Inputs = new InputSystem_Actions();
        Inputs.Player.Enable();

        InitializeStateMachine();
        InitializeComponents();
    }

    private void InitializeStateMachine()
    {
        // --- Initialize State Machine ------------
        stateMachine = new PlayerStateMachine();

        // --- Initialize States ------------
        lifeState = new LifeState(this, stateMachine);
        moveState = new MoveState(this, stateMachine);
        jumpState = new JumpState(this, stateMachine);

        blockState = new BlockState(this, stateMachine);
        attackState = new AttackState(this, stateMachine);
    }

    private void InitializeComponents()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();
        Collider2D = GetComponent<Collider2D>();
        Animator = GetComponent<Animator>();
    }

    private void Start()
    {
        stateMachine.Initialize(moveState);
        SetVulnerable(true);
    }

    private void Update()
    {
        CurrentState = Animator.GetCurrentAnimatorStateInfo(0);
        stateMachine.playerState.FrameUpdate();
    }

    private void FixedUpdate() => stateMachine.playerState.PhysicsUpdate();

    public void SetVulnerable(bool vulnerable) => IsVulnerable = vulnerable;
    public void SetGrounded(bool grounded) => IsGrounded = grounded;
    public void SetFlip(bool flippable) => IsFlippable = flippable;
}
