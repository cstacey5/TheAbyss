using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : CharacterBase
{
    //got help from a youtube series that is for making an rpg type game
    //link: https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix
    public virtual void UnselectTarget()
    {

    }

    public virtual Transform SelectTarget()
    {
        return hitBox;
    }
}
