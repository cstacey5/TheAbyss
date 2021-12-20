using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//got help from a youtube series that is for making an rpg type game
//link: https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix

//base class for items
public abstract class Item : ScriptableObject
{
    [SerializeField]
    private Sprite itemIcon;

    [SerializeField]
    private int stackCount;

    private SlotScript slot;

    public Sprite MyItemIcon
    {
        get
        {
            return itemIcon;
        }
    }

    public int MyStackCount
    {
        get
        {
            return stackCount;
        }
    }

    public SlotScript MySlot
    {
        get
        {
            return slot;
        }
        set
        {
            slot = value;
        }
    }

    public void Remove()
    {
        if(MySlot != null)
        {
            MySlot.RemoveItem(this);
        }
    }

}
