using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

//got help from a youtube series that is for making an rpg type game
//link: https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix
public class SlotScript : MonoBehaviour, IPointerClickHandler, IClickable
{
    [SerializeField]
    private Image icon;

    private ObservableStack<Item> itemStack = new ObservableStack<Item>();

    //property for checking if stack is empty
    public bool IsEmpty
    {
        get
        {
            return itemStack.Count == 0;
        }
    }

    //proprty for item being clicked
    public Item MyItem
    {
        get
        {
            if (!IsEmpty)
            {
                return itemStack.Peek();
            }

            return null;
        }
    }

    //property for image icon
    public Image MyIcon 
    {
        get
        {
            return icon;
        }
        set
        {
            icon = value;
        }
    }

    //property for stacksize count
    public int MyCount
    {
        get
        {
            return itemStack.Count;
        }
    }

    //property for stack text
    public Text MyStackText
    {
        get
        {
            return stackText;
        }
    }

    [SerializeField]
    private Text stackText;

    private void Awake()
    {
        //events so that every time an item is popped/pushed/cleared we update the stack
        itemStack.OnPop += new UpdateStackEvent(UpdateSlot);
        itemStack.OnPush += new UpdateStackEvent(UpdateSlot);
        itemStack.OnClear += new UpdateStackEvent(UpdateSlot);
    }

    //add item 
    public bool AddItem(Item item)
    {
        itemStack.Push(item);
        icon.sprite = item.MyItemIcon;
        icon.color = Color.white;
        item.MySlot = this;

        return true;
    }

    // remove item
    public void RemoveItem(Item item)
    {
        if (!IsEmpty)
        {
            itemStack.Pop();
        }
    }

    //handle clicking item
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            UseItem();
        }
    }

    //use the item
    public void UseItem()
    {
        if(MyItem is IUseable)
        {
            (MyItem as IUseable).Use();
        }
    }

    //updating item text/color based on stack size
    public void UpdateStackSize(IClickable clickable)
    {
        //2 or more stack
        if(clickable.MyCount > 1)
        {
            clickable.MyStackText.text = clickable.MyCount.ToString();
            clickable.MyStackText.color = Color.white;
            clickable.MyIcon.color = Color.white;
        }
        else //1 stack
        {
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }

        //empty stack
        if (clickable.MyCount == 0)
        {
            clickable.MyIcon.color = new Color(0, 0, 0, 0);
            clickable.MyStackText.color = new Color(0, 0, 0, 0);
        }
    }

    //stack item if we are stacking a stackable item
    public bool StackItem(Item item)
    {
        if (!IsEmpty && item.name == MyItem.name && itemStack.Count < MyItem.MyStackCount)
        {
            itemStack.Push(item);
            item.MySlot = this;

            return true;
        }

        return false;
    }

    //function bound to event changes
    private void UpdateSlot()
    {
        UpdateStackSize(this);
    }
}
