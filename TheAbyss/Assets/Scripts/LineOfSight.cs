using System;
using UnityEngine;

//got help from a youtube series that is for making an rpg type game
//link: https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix

[Serializable]
public class LineOfSight
{
    [SerializeField]
    private GameObject firstLOS, secondLOS;

    public void DeactivateBlocks()
    {
        firstLOS.SetActive(false);
        secondLOS.SetActive(false);
    }

    public void ActivateBlocks()
    {
        firstLOS.SetActive(true);
        secondLOS.SetActive(true);
    }
}
