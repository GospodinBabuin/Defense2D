using System;
using UnityEngine;

public class Enemy : Unit
{ 
    private void Start()
    {
        Animator = gameObject.GetComponent<Animator>();
        Rigidbody = gameObject.GetComponent<Rigidbody2D>();

        gameObject.name = Name;
        WorldManager.Instance.EnemyList.Add(this);

        PlayAppearOrDisappearSound(spawnSound);
        
        lastPosition = new Vector3(transform.position.x, transform.position.y);
        needToCheckWalkingAnimation = true;
    }
    private void Update()
    {
        if (needToCheckWalkingAnimation == true)
            SetWalkingAnimation();

        Behaviour();
    }
    private void FixedUpdate()
    {
        FindTarget("Allies");
        if (WorldManager.Instance.dayState == WorldManager.DayState.DAY)
            TakeDamage(Health);
    }
    private void OnDrawGizmosSelected()
    {
        if (AttackPoint == null)
            return;
        Gizmos.DrawWireSphere(AttackPoint.position, AttackRange);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            needToCheckWalkingAnimation = false;
            Animator.SetBool("IsWalking", false);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            needToCheckWalkingAnimation = true;
            Animator.SetBool("IsWalking", true);
        }
    }
}