using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    //got help from a youtube series that is for making an rpg type game
    //link: https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix

     [SerializeField]
     private PlayerStat health;

     [SerializeField]
     private PlayerStat mana;

     private float initialHealth = 100;
     private float maxHealth = 100;

     private float initialMana = 80;
     private float maxMana = 80;

     protected override void Start()
    {
        health.Initialize(initialHealth, maxHealth);
        mana.Initialize(initialMana, maxMana);
        base.Start();
    }

     protected override void Update()
     {
        GetPlayerInput();
        base.Update();
     
     }

    private void GetPlayerInput()
    {

        
        //testing health/mana bar functionality
        //will be removed later, just using for debug purposes
        if (Input.GetKeyDown(KeyCode.I))
        {
            health.MyCurrentValue -= 10;
            mana.MyCurrentValue -= 10;
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            health.MyCurrentValue += 10;
            mana.MyCurrentValue += 10;
        }


        //movement
        moveDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += Vector2.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector2.right;
        }

        //attacking
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

}
