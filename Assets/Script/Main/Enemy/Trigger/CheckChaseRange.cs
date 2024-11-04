using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckChaseRange : MonoBehaviour
{
    protected Enemy enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemy.InRangeChaseable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            enemy.InRangeChaseable = false;
        }
    }
}
