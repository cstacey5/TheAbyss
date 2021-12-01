using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public abstract class CharacterBase : MonoBehaviour
{
    //got help from a youtube series that is for making an rpg type game
    //link: https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix


    [SerializeField]
    protected float moveSpeed;

    protected Vector2 moveDirection;
    protected bool isMoving;
    protected bool isAttacking = false;
    protected Rigidbody2D rb2d;

    protected Animator animator;

    [SerializeField]
    protected Transform hitBox;

    protected float timeBetweenMelee;
    [SerializeField]
    protected float startTimeBetweenMelee;
    [SerializeField]
    protected float meleeRange;

    [SerializeField]
    protected PlayerStat playerHealth;

    [SerializeField]
    private float initialHealth;
    [SerializeField]
    private float maxHealth;

    protected bool isHurt;

    private float closestDistance = 100;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        playerHealth.Initialize(initialHealth, maxHealth);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Animate();
        calculateSortingLayer();
    }

    private void FixedUpdate()
    {
        Move();
        hurtAnimationReset();
    }

    public void Move()
    {
       // transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        rb2d.velocity = moveDirection.normalized * moveSpeed; //normalize so we dont move extra fast in diagonals

    }

    public void Animate()
    {
        AnimateCharacter(moveDirection);
    }

    public void AnimateCharacter(Vector2 direction)
    {
        if(direction != Vector2.zero)
        {
            animator.SetFloat("moveX", direction.x);
            animator.SetFloat("moveY", direction.y);
        }

        //only set the animation direction when the movement vector != 0, so we dont snap back to the idle down animation when not pressing a key
        if (direction != Vector2.zero) //when we are not idle, do walking anim. if we are idle, do idle anim
        {

            isMoving = true; //change flag for walking anim

        }
        else
        {
            isMoving = false; //change it back to go to idle
        }

        animator.SetBool("isMoving", isMoving);
        //if (isHurt)
        //{
        //    isHurt = false;
        //    animator.SetBool("isHurt", isHurt);
        //}
    }

    public virtual void TakeDamage(float damageTaken)
    {
        //reduce health
        playerHealth.MyCurrentValue -= damageTaken;
        isHurt = true;
        animator.SetBool("isHurt", isHurt);

        if(playerHealth.MyCurrentValue <= 0)
        {
            animator.SetTrigger("Die");
        }
    }

    public void calculateSortingLayer()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject closestEnemy = GameObject.FindGameObjectWithTag("Enemy");

        if(enemies != null && closestEnemy != null)
        {
            foreach (GameObject enemy in enemies)
            {
                if (Vector2.Distance(GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position, enemy.GetComponent<Transform>().position) < closestDistance)
                {
                    closestDistance = Vector2.Distance(GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position, enemy.GetComponent<Transform>().position);
                    closestEnemy = enemy;
                }
            }
            if (GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position.y > closestEnemy.GetComponent<Transform>().position.y)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().sortingLayerName = "PlayerBehindEnemy";
            }
            else
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>().sortingLayerName = "Player";
            }
        }
        closestDistance = 0;
    }

    public void hurtAnimationReset()
    {
        if (isHurt)
        {
            isHurt = false;
            animator.SetBool("isHurt", isHurt);
        }
    }

    
}
