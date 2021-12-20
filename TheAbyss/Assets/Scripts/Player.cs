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

     public bool isDead = false;
     private float respawnTime = 10;

     //attack damages
     [SerializeField]
     private int baseMeleeDamage;
     private int meleeDamageMultiplier = 1;

     [SerializeField]
     private float playerRegenHealthSpeed;

    [SerializeField]
    private Image actionBar1Image;
    [SerializeField]
    private Text actionBar1Text;

    [SerializeField]
    private Image actionBar2Image;
    [SerializeField]
    private Text actionBar2Text;

    [SerializeField]
    private Image actionBar3Image;
    [SerializeField]
    private Text actionBar3Text;

    [SerializeField]
    private GameObject fireballPrefab;


    public Transform minimapSpriteTransform;

    [SerializeField]
    private int teleportRange;
    [SerializeField]
    private GameObject teleportPrefab;
    [SerializeField]
    private float teleportCooldownStart;
    private float teleportCooldownCurrent = 0;

    [SerializeField]
    private float castCooldownStart = 3;
    private float castCooldownCurrent = 0;
    private bool isCasting = false;
    [SerializeField]
    private int fireballDamage;
    private float animReset = 0.1f;

    
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


        if(teleportCooldownCurrent <= 0)
        {
            actionBar2Text.text = "";
            actionBar2Image.color = Color.white;
        }
        else
        {
            actionBar2Text.text = teleportCooldownCurrent.ToString();
            actionBar2Image.color = new Color(0.5f, 0.5f, 0.5f, 1);
        }

        if (timeBetweenMelee <= 0)
        {
            actionBar1Text.text = "";
            actionBar1Image.color = Color.white;
        }
        else
        {
            actionBar1Text.text = timeBetweenMelee.ToString();
            actionBar1Image.color = new Color(0.5f, 0.5f, 0.5f, 1);
        }

        if (castCooldownCurrent <= 0)
        {
            actionBar3Text.text = "";
            actionBar3Image.color = Color.white;
        }
        else
        {
            actionBar3Text.text = castCooldownCurrent.ToString();
            actionBar3Image.color = new Color(0.5f, 0.5f, 0.5f, 1);
        }

        if (playerHealth.MyCurrentValue <= 0.9 && !isDead)
        {
            isDead = true;
            animator.SetBool("isDead", isDead);
        }

        if (isDead)
        {
            respawnTime -= Time.deltaTime;
            errorText.text = "Respawn in " + respawnTime.ToString("F0");
        }

        if (respawnTime <= 0)
        {
            errorText.text = "";
            respawnTime = 10;
            isDead = false;
            playerHealth.MyCurrentValue = playerHealth.MyMaxValue;
            animator.SetBool("isDead", isDead);
            transform.position = new Vector3(-17.22f, -1.12f, transform.position.z);
        }

        base.Update();
     
     }

    //private void FixedUpdate()
    //{
    //    if (isCasting)
    //    {
    //        isCasting = false;
    //        animator.SetBool("isCasting", isCasting);
    //    }
    //}

    private void GetPlayerInput()
    {

        //movement
        moveDirection = Vector2.zero;
        if (!isDead)
        {
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
            if (Input.GetKeyDown("1") && MyTarget != null && !isDead)
            {
                LOSBlock();
                //Debug.Log(Vector2.Distance(transform.position, MyTarget.transform.position));
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
            else if (Input.GetKeyDown("1") && MyTarget == null && !isDead) //no target
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
        if (Input.GetKeyDown("2") && teleportCooldownCurrent <= 0 && !isDead)
        {
            teleportAbility();
            teleportCooldownCurrent = teleportCooldownStart;
        }
        else if(Input.GetKeyDown("2") && teleportCooldownCurrent > 0 && !isDead)
        {
            errorText.text = "Teleport on cooldown!";
            errorTextTime = 3;
            errorTextOn = true;
        }

        if(Input.GetKeyDown("3") && castCooldownCurrent <= 0 && !isDead)
        {
            CastAttack();
        }
        else if(Input.GetKeyDown("3") && castCooldownCurrent > 0 && !isDead)
        {
            errorText.text = "Fireball on cooldown!";
            errorTextTime = 3;
            errorTextOn = true;
        }

        if(castCooldownCurrent > 0)
        {
            castCooldownCurrent -= Time.deltaTime;
        }
        if(animReset > 0)
        {
            animReset -= Time.deltaTime;
        }

        if(animReset <= 0)
        {
            isCasting = false;
            animator.SetBool("isCasting", isCasting);
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

    public void energyPot(int potVal)
    {
        if (energy.MyCurrentValue < energy.MyMaxValue && !isDead)
        {
            energy.MyCurrentValue += potVal;
        }
        if (energy.MyCurrentValue > energy.MyMaxValue)
        {
            energy.MyCurrentValue = energy.MyMaxValue;
        }
    }
    //constant regen energy when below max energy
    private void regenEnergy()
    {
        if(energy.MyCurrentValue < energy.MyMaxValue && !isDead)
        {
            energy.MyCurrentValue += Time.deltaTime;
        }
        if(energy.MyCurrentValue > energy.MyMaxValue)
        {
            energy.MyCurrentValue = energy.MyMaxValue;
        }
    }

    public void healthPot(int potVal)
    {
        if (playerHealth.MyCurrentValue < playerHealth.MyMaxValue && !isDead)
        {
            playerHealth.MyCurrentValue += potVal;
        }
        if (playerHealth.MyCurrentValue > playerHealth.MyMaxValue)
        {
            playerHealth.MyCurrentValue = playerHealth.MyMaxValue;
        }
    }

    private void regenHealth()
    {
        if(playerHealth.MyCurrentValue < playerHealth.MyMaxValue && !isDead)
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

    private void CastAttack()
    {
        GameObject spawnedFireball;
        LOSBlock();
        if(InLineOfSight() && energy.MyCurrentValue >= 20 && MyTarget != null)
        {
            isCasting = true;
            animator.SetBool("isCasting", isCasting);
            castCooldownCurrent = castCooldownStart;
            energy.MyCurrentValue -= 20;
            animReset = 0.1f;

            spawnedFireball = Instantiate(fireballPrefab, MyTarget.transform.position, Quaternion.identity);
            MyTarget.GetComponentInParent<Enemy>().TakeDamage(fireballDamage);


        }
        else if (!InLineOfSight() && energy.MyCurrentValue >= 20 && MyTarget != null) //not in line of sight
        {
            errorText.text = "Target not in line of sight!";
            errorTextTime = 3;
            errorTextOn = true;
        }
        else if (InLineOfSight() && energy.MyCurrentValue < 20 && MyTarget != null) //not enough energy
        {
            errorText.text = "Not enough energy!";
            errorTextTime = 3;
            errorTextOn = true;
        }
        else if(MyTarget == null)
        {
            errorText.text = "No target selected!";
            errorTextTime = 3;
            errorTextOn = true;
        }
    }
}
