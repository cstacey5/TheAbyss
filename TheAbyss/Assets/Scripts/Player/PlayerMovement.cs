using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    

    // WANTED TILE BASED MOVEMENT LIKE IN POKEMON, I.E. Tap A once and it still moves a set tile amount
    // This is going to change at a later date most likely
    // Found a tutorial on youtube for this
    // Link: https://www.youtube.com/watch?v=_Pm16a18zy8&ab_channel=GameDevExperiments

    public float moveSpeed = 5.0f;

     private bool isMoving;
     private Vector2 input;

     private Animator animator;

     private bool isAttacking;

     private void Awake()
     {
         animator = GetComponent<Animator>();
     }

     private void Update()
     {
        

         if (!isMoving)
         {
             input.x = Input.GetAxisRaw("Horizontal");
             input.y = Input.GetAxisRaw("Vertical");

             //remove diagonal movement
             if (input.x != 0)
                 input.y = 0;

             if(input != Vector2.zero)
             {
                 animator.SetFloat("moveX", input.x);
                 animator.SetFloat("moveY", input.y);
                 var targetPos = transform.position;
                 targetPos.x += input.x;
                 targetPos.y += input.y;

                 StartCoroutine(Move(targetPos));
             }

         }


         animator.SetBool("isMoving", isMoving);
        //basic attack animation
         if (Input.GetKey("1"))
         {
             isAttacking = true;
             animator.SetBool("isAttacking", isAttacking);
         }
         else
         {
             isAttacking = false;
             animator.SetBool("isAttacking", isAttacking);
         }
     }

     IEnumerator Move(Vector3 targetPos)
     {
         isMoving = true;

         while((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
         {
             transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
             yield return null;
         }
         transform.position = targetPos;
         isMoving = false;
     }
}
