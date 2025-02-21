using UnityEngine;

public class DeadGround : MonoBehaviour
{
    [SerializeField] private Transform RespawnPoint;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            _ = CanvasManager.Instance.RespawnEffect(player, RespawnPoint);
        }
    }
}
