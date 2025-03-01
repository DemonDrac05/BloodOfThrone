using UnityEngine;

public interface IComponents
{
    Rigidbody2D Rigidbody2D { get; }
    Collider2D Collider2D { get; }
    Animator Animator { get; }
}
