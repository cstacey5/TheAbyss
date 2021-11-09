using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterBase : MonoBehaviour
{
    //got help from a youtube series that is for making an rpg type game
    //link: https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix


    [SerializeField]
    private float moveSpeed;

    protected Vector2 moveDirection;
    protected bool isMoving;
    protected bool isAttacking = false;
    protected Rigidbody2D rb2d;

    protected Animator animator;


    // Start is called before the first frame update
    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Animate();
    }

    private void FixedUpdate()
    {
        Move();
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
    }

    
}
