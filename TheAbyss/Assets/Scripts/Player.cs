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

     //attack damages
     [SerializeField]
     private int baseMeleeDamage;
     private int meleeDamageMultiplier = 1;

     [SerializeField]
     private float playerRegenHealthSpeed;

    //private float hurtReset = 1;

     public Transform minimapSpriteTransform;

    [SerializeField]
    private int teleportRange;
    [SerializeField]
    private GameObject teleportPrefab;
    [SerializeField]
    private float teleportCooldownStart;
    private float teleportCooldownCurrent = 0;


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
        regenHealth();

        base.Update();
     
     }

    private void GetPlayerInput()
    {


        //testing health/ mana bar functionality
        //will be removed later, just using for debug purposes
        //if (Input.GetKeyDown(KeyCode.I))
        //{
        //    playerHealth.MyCurrentValue -= 10;
        //    energy.MyCurrentValue -= 10;
        //}
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    playerHealth.MyCurrentValue += 10;
        //    energy.MyCurrentValue += 10;
        //}


        //movement
        moveDirection = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveDirection += Vector2.up;
            attackIndex = 0;
            minimapSpriteTransform.eulerAngles = new Vector3(0, 0, 0);
            
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveDirection += Vector2.right;
            attackIndex = 1;
            minimapSpriteTransform.eulerAngles = new Vector3(0, 0, 270);

        }
        if (Input.GetKey(KeyCode.S))
        {
            moveDirection += Vector2.down;
            attackIndex = 2;
            minimapSpriteTransform.eulerAngles = new Vector3(0, 0, 180);
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveDirection += Vector2.left;
            attackIndex = 3;
            minimapSpriteTransform.eulerAngles = new Vector3(0, 0, 90);
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
            if (Input.GetKeyDown("1") && MyTarget != null)
            {
                LOSBlock();
                Debug.Log(Vector2.Distance(transform.position, MyTarget.transform.position));
                if (Vector2.Distance(transform.position, MyTarget.transform.position) <= meleeRange && InLineOfSight() && energy.MyCurrentValue >= 10)
                {
                    Debug.Log("Melee");
                    isAttacking = true;
                    energy.MyCurrentValue -= 10;
                    animator.SetBool("isAttacking", isAttacking);
                    MyTarget.GetComponentInParent<Enemy>().TakeDamage(baseMeleeDamage * meleeDamageMultiplier);

                    timeBetweenMelee = startTimeBetweenMelee;
                }
                else
                {

                    if (!InLineOfSight()) //not in line of sight
                    {
                        errorText.text = "Target not in line of sight!";
                        errorTextTime = 3;
                        errorTextOn = true;
                    }
                    if (energy.MyCurrentValue < 10) //not enough energy
                    {
                        errorText.text = "Not enough energy!";
                        errorTextTime = 3;
                        errorTextOn = true;
                    }
                    if (Vector2.Distance(transform.position, MyTarget.transform.position) > meleeRange) //target out of range
                    {
                        errorText.text = "Target out of range!";
                        errorTextTime = 3;
                        errorTextOn = true;
                    }
                }
            }
            else if (Input.GetKeyDown("1") && MyTarget == null) //no target
            {
                errorText.text = "No target selected!";
                errorTextTime = 3;
                errorTextOn = true;
            }
        }
        else
        {
            timeBetweenMelee -= Time.deltaTime;
            isAttacking = false;
            animator.SetBool("isAttacking", isAttacking);
        }


        //teleport
        if(teleportCooldownCurrent > 0)
        {
            teleportCooldownCurrent -= Time.deltaTime;
        }
        if (Input.GetKeyDown("2") && teleportCooldownCurrent <= 0)
        {
            teleportAbility();
            teleportCooldownCurrent = teleportCooldownStart;
        }
        else if(Input.GetKeyDown("2") && teleportCooldownCurrent > 0)
        {
            errorText.text = "Teleport on cooldown!";
            errorTextTime = 3;
            errorTextOn = true;
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

    private void regenHealth()
    {
        if(playerHealth.MyCurrentValue < playerHealth.MyMaxValue)
        {
            playerHealth.MyCurrentValue += Time.deltaTime * playerRegenHealthSpeed;
        }
        if(playerHealth.MyCurrentValue > playerHealth.MyMaxValue)
        {
            playerHealth.MyCurrentValue = playerHealth.MyMaxValue;
        }
    }
    
    //function that uses raycast to check if the player can teleport the full range, if no collider is hit, the normal teleport takes place
    //if a collider is hit, then we teleport to hit.point so we dont accidentally teleport over a collider / rubber band against the collider
    private void teleportAbility()
    {
        //Debug.Log(LayerMask.GetMask("Both"));
        Vector3 teleportPos;
        if(attackIndex == 0) // up
        {
            teleportPos = new Vector3(transform.position.x, transform.position.y + teleportRange, transform.position.z);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.up, Vector2.Distance(transform.position, teleportPos), LayerMask.GetMask("Both"));
            if (hit.collider == null)
            {
                //teleport player to teleport pos
                Instantiate(teleportPrefab, teleportPos, Quaternion.identity);
                transform.position = teleportPos;
            }
            else
            {
                //Debug.Log(hit.collider);
                teleportPos = hit.point;
                Instantiate(teleportPrefab, teleportPos, Quaternion.identity);
                transform.position = teleportPos;
                //teleport player to location of collider interaction
            }

        }
        else if(attackIndex == 1) //right
        {
            teleportPos = new Vector3(transform.position.x + teleportRange, transform.position.y, transform.position.z);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, Vector2.Distance(transform.position, teleportPos), LayerMask.GetMask("Both"));
            if (hit.collider == null)
            {
                //teleport player to teleport pos
                Instantiate(teleportPrefab, teleportPos, Quaternion.identity);
                transform.position = teleportPos;
            }
            else
            {
                //Debug.Log(hit.collider);
                teleportPos = hit.point;
                Instantiate(teleportPrefab, teleportPos, Quaternion.identity);
                transform.position = teleportPos;
                //teleport player to location of collider interaction
            }
        }
        else if(attackIndex == 2) //down
        {
            teleportPos = new Vector3(transform.position.x, transform.position.y - teleportRange, transform.position.z);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, Vector2.Distance(transform.position, teleportPos), LayerMask.GetMask("Both"));
            if (hit.collider == null)
            {
                //teleport player to teleport pos
                Instantiate(teleportPrefab, teleportPos, Quaternion.identity);
                transform.position = teleportPos;
            }
            else
            {
                //Debug.Log(hit.collider);
                teleportPos = hit.point;
                Instantiate(teleportPrefab, teleportPos, Quaternion.identity);
                transform.position = teleportPos;
                //teleport player to location of collider interaction
            }
        }
        else if(attackIndex == 3) //left
        {
            teleportPos = new Vector3(transform.position.x - teleportRange, transform.position.y, transform.position.z);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.left, Vector2.Distance(transform.position, teleportPos), LayerMask.GetMask("Both"));
            if (hit.collider == null)
            {
                //teleport player to teleport pos
                Instantiate(teleportPrefab, teleportPos, Quaternion.identity);
                transform.position = teleportPos;

            }
            else
            {
                Debug.Log(hit.collider);
                teleportPos = hit.point;
                Instantiate(teleportPrefab, teleportPos, Quaternion.identity);
                transform.position = teleportPos;
                //teleport player to location of collider interaction
            }
        }
    }
}
