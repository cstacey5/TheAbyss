using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//got help from a youtube series that is for making an rpg type game
//link: https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix
public class InventoryScript : MonoBehaviour
{
    private static InventoryScript instance;

    public static InventoryScript MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<InventoryScript>();
            }

            return instance;
        }
    }

    private List<Bag> bagList = new List<Bag>();

    [SerializeField]
    private BagButton[] bagButtons;
    //Debug
    [SerializeField]
    private Item[] items;

    public bool CanAddBag
    {
        get
        {
            return bagList.Count < 5;
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        Bag bag = (Bag)Instantiate(items[0]);
        bag.Initialize(16);
        bag.Use();
    }

    private void Update()
    {
        //debug functions for checking if items work
        if (Input.GetKeyDown(KeyCode.K))
        {
            Bag bag = (Bag)Instantiate(items[0]);
            bag.Initialize(16);
            AddItem(bag);
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            HealthPotion potion = (HealthPotion)Instantiate(items[1]);
            AddItem(potion);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            EnergyPotion potion = (EnergyPotion)Instantiate(items[2]);
            AddItem(potion);
        }
    }

    public void AddBag(Bag bag)
    {
        foreach(BagButton bagButton in bagButtons)
        {
            if(bagButton.MyBag == null)
            {
                bagButton.MyBag = bag;
                bagList.Add(bag);
                break;
            }
        }
    }

    public void AddItem(Item item)
    {
        if(item.MyStackCount > 0)
        {
            if (StackPlace(item))
            {
                return;
            }
        }
        EmptyPlace(item);
    }


    private bool StackPlace(Item item)
    {
        foreach (Bag bag in bagList)
        {
            foreach(SlotScript slots in bag.MyBagScript.MySlotsList)
            {
                if (slots.StackItem(item))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void EmptyPlace(Item item)
    {
        foreach(Bag bag in bagList)
        {
            if (bag.MyBagScript.AddItem(item))
            {
                return;
            }
        }
    }
}
