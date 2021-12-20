using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


//new version of stack, so we can call events when we push/pull/clear
//uses base stack class for most functionality

//got help from a youtube series that is for making an rpg type game
//link: https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix


public delegate void UpdateStackEvent();
class ObservableStack<T> : Stack<T>
{
    public event UpdateStackEvent OnPush;
    public event UpdateStackEvent OnPop;
    public event UpdateStackEvent OnClear;

    public new void Push(T item)
    {
        base.Push(item);

        if(OnPush != null)
        {
            OnPush();
        }
    }

    public new T Pop()
    {
        T item = base.Pop();

        if(OnPop != null)
        {
            OnPop();
        }

        return item;

    }

    public new void Clear()
    {
        base.Clear();

        if (OnClear != null)
        {
            OnClear();
        }
    }
}
