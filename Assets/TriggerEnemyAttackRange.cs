using UnityEngine;

public class TriggerEnemyAttackRange : MonoBehaviour
{
    private Enemy Enemy;

    private void Awake()
    {
        Enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Enemy.SetPlayerInAttackRange(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Enemy.SetPlayerInAttackRange(false);
        }
    }
}
