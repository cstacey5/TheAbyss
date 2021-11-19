using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : NPC
{
    //got help from a youtube series that is for making an rpg type game
    //link: https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix


    [SerializeField]
    private CanvasGroup healthBarGroup;

    protected override void Start()
    {
        base.Start();
    }
    public override Transform SelectTarget()
    {
        healthBarGroup.alpha = 1;
        return base.SelectTarget();
    }

    public override void UnselectTarget()
    {
        healthBarGroup.alpha = 0;
        base.UnselectTarget();
    }

}
