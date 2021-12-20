using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//got help from a youtube series that is for making an rpg type game
//link: https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix

[CreateAssetMenu(fileName = "EnergyPotion", menuName = "Items/EnergyPotion", order = 2)]
public class EnergyPotion : Item, IUseable
{
    public Sprite MyIcon => throw new System.NotImplementedException();

    [SerializeField]
    private int potionValue;

    public void Use()
    {
        Remove();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().energyPot(potionValue);

    }
}