using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    //got help from a youtube series that is for making an rpg type game
    //link: https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private CanvasGroup healthBarGroup;

    [SerializeField]
    private int enemyMeleeDamage;

    private Transform target;
    public Transform Target
    { 
        get
        {
            return target;
        }
        set
        {
            target = value;
        }
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        FollowTarget();
        Attack();
        base.Update();
    }
    public override Transform SelectTarget()
    {
        healthBarGroup.alpha = 1;
        return base.SelectTarget();
    }

    public override void UnselectTarget()
    {
        healthBarGroup.alpha = 0;
        base.UnselectTarget();
    }

    private void FollowTarget()
    {
        if(target != null && playerHealth.MyCurrentValue > 0 && GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().isDead == false)
        {
            //transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            moveDirection = (target.position - transform.position).normalized;
            rb.velocity = moveDirection * moveSpeed;
            if (Vector2.Distance(Target.position, transform.position) <= meleeRange - 0.1)
            {
                moveDirection = Vector2.zero;
            }
        }
        else
        {
            moveDirection = Vector2.zero;
        }
    }

    private void Attack()
    {
        if(target != null && GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().isDead == false)
        {
            if (Vector2.Distance(Target.position, transform.position) <= meleeRange && timeBetweenMelee <= 0 && playerHealth.MyCurrentValue > 0)
            {
                isAttacking = true;
                animator.SetBool("isAttacking", isAttacking);
                timeBetweenMelee = startTimeBetweenMelee;
                GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().TakeDamage(enemyMeleeDamage);
            }
            else
            {
                timeBetweenMelee -= Time.deltaTime;
                isAttacking = false;
                animator.SetBool("isAttacking", isAttacking);
            }
        }
    }

}
