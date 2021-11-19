using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : CharacterBase
{
    //got help from a youtube series that is for making an rpg type game
    //link: https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix


     [SerializeField]
     private PlayerStat energy;

     private float initialEnergy = 80;
     private float maxEnergy = 80;


     public Transform MyTarget { get; set; }

     [SerializeField]
     private LineOfSight[] losBlocks;
     private int attackIndex;

     [SerializeField]
     private Text errorText;
     private float errorTextTime = 3;
     private bool errorTextOn;



     protected override void Start()
    {

        energy.Initialize(initialEnergy, maxEnergy);

        base.Start();
    }

     protected override void Update()
     {
        GetPlayerInput();
        Attack();
        regenEnergy();
        base.Update();
     
     }

    private void GetPlayerInput()
    {


        //testing health/ mana bar functionality
        //will be removed later, just using for debug purposes
        if (Input.GetKeyDown(KeyCode.I))
            {
                this.playerHealth.MyCurrentValue -= 10;
                energy.MyCurrentValue -= 10;
            }
        if (Input.GetKeyDown(KeyCode.O))
        {
            this.playerHealth.MyCurrentValue += 10;
            energy.MyCurrentValue += 10;
        }


        //movement
        moveDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += Vector2.up;
            attackIndex = 0;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += Vector2.down;
            attackIndex = 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += Vector2.left;
            attackIndex = 3;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector2.right;
            attackIndex = 1;
        }

       
    }

    private bool InLineOfSight()
    {

        if(MyTarget != null)
        {
            Vector3 targetDirection = (MyTarget.transform.position - transform.position).normalized;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, targetDirection, Vector2.Distance(transform.position, MyTarget.transform.position), 256);

            if (hit.collider == null)
            {
                return true;
            }
        }

        return false;
    }
    private void Attack()
    {
        if (errorTextOn)
        {
            errorTextTime -= Time.deltaTime;
        }
        if(errorTextTime <= 0)
        {
            errorTextOn = false;
            errorText.text = "";
        }
        //attacking
        if (timeBetweenMelee <= 0)
        {
            if (Input.GetKeyDown("1"))
            {
                LOSBlock();
                Debug.Log(Vector2.Distance(transform.position, MyTarget.transform.position));
                if (Vector2.Distance(transform.position, MyTarget.transform.position) <= meleeRange && InLineOfSight() && energy.MyCurrentValue >= 10)
                {
                    Debug.Log("Melee");
                    isAttacking = true;
                    energy.MyCurrentValue -= 10;
                    animator.SetBool("isAttacking", isAttacking);
                    MyTarget.GetComponentInParent<Enemy>().TakeDamage(20);

                    timeBetweenMelee = startTimeBetweenMelee;
                }
                else
                {
                    if (!errorTextOn)
                    {
                        if (!InLineOfSight())
                        {
                            errorText.text = "Target not in line of sight!";
                            errorTextTime = 3;
                            errorTextOn = true;
                        }
                        if(energy.MyCurrentValue < 10)
                        {
                            errorText.text = "Not enough energy!";
                            errorTextTime = 3;
                            errorTextOn = true;
                        }
                    }
                }
            }
        }
        else
        {
            timeBetweenMelee -= Time.deltaTime;
            isAttacking = false;
            animator.SetBool("isAttacking", isAttacking);
        }
    }

    //chooses which los blocks to spawn
    private void LOSBlock()
    {
        foreach(LineOfSight block in losBlocks)
        {
            block.DeactivateBlocks();
        }

        losBlocks[attackIndex].ActivateBlocks();
    }

    //constant regen energy when below max energy
    private void regenEnergy()
    {
        if(energy.MyCurrentValue < energy.MyMaxValue)
        {
            energy.MyCurrentValue += Time.deltaTime;
        }
        if(energy.MyCurrentValue > energy.MyMaxValue)
        {
            energy.MyCurrentValue = energy.MyMaxValue;
        }
    }
}
