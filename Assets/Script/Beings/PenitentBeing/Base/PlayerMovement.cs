using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("=== Movement Properties ==========")]
    [SerializeField] public float jumpForce = 14f;
    [SerializeField] public float moveSpeed = 10f;

    // --- MOVEMENT INPUT PROPERTIES -----------
    [HideInInspector] public float posX;
    [HideInInspector] public float posY;

    // --- FLIPPING PROPERTIES ----------
    public bool IsFacingRight { get; private set; } = true;
    private Vector3 _flip;

    // --- SINGLETON CLASS ----------
    private Player player;

    // --- INPUT ACTIONS REFERENCE ----------
    private InputSystem_Actions _inputActions;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Update()
    {
        InputMovement();
    }

    private void FixedUpdate()
    {
        UpdateFlip();
    }

    public void UpdateFlip()
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
        if (!player.IsFlippable) return;

        IsFacingRight = !IsFacingRight;

        _flip = transform.localScale;
        _flip.x = -1;

        float yRot = _flip.x > 0 ? 0f : 180f;
        transform.Rotate(0, yRot, 0);
    }

    public Vector3 GetFlip() => _flip;

    public void InputMovement()
    {
        if (!player.IsFlippable) posX = 0;
        else posX = player.Inputs.Player.Move.ReadValue<Vector2>().x;
    }
}
