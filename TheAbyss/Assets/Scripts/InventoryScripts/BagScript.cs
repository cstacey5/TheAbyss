using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//got help from a youtube series that is for making an rpg type game
//link: https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix
public class BagScript : MonoBehaviour
{
    [SerializeField]
    private GameObject slotPrefab;

    private CanvasGroup group;

    private List<SlotScript> slotsList = new List<SlotScript>();

    public List<SlotScript> MySlotsList
    {
        get
        {
            return slotsList;
        }
    }


    private void Awake()
    {
        group = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            OpenClose();
        }
    }
    public void AddSlots(int slots)
    {
        for(int i = 0; i < slots; i++)
        {
            SlotScript slot = Instantiate(slotPrefab, transform).GetComponent<SlotScript>();
            slotsList.Add(slot);
        }
    }

    public void OpenClose()
    {
        group.alpha = group.alpha > 0 ? 0 : 1;
        group.blocksRaycasts = group.blocksRaycasts == true ? false : true;
    }

    public bool AddItem(Item item)
    {
        foreach(SlotScript slot in slotsList)
        {
            if (slot.IsEmpty)
            {
                slot.AddItem(item);
                return true;
            }
        }
        return false;
    }
}
