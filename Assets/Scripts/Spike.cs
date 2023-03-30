using System;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<Unit>().TakeDamage(col.gameObject.GetComponent<Unit>().Health);
            gameObject.GetComponent<Buildings>().TakeDamage(gameObject.GetComponent<Buildings>().Health);
        }
    }
}
