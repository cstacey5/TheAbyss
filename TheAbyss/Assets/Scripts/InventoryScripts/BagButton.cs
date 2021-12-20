using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//got help from a youtube series that is for making an rpg type game
//link: https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix

public class BagButton : MonoBehaviour
{

    private Bag bag;

    [SerializeField]
    private Sprite fullSprite;
    [SerializeField]
    private Sprite emptySprite;

    public Bag MyBag
    {
        get
        {
            return bag;
        }
        set
        {
            if (value != null)
            {
                GetComponent<Image>().sprite = fullSprite;
            }
            else
            {
                GetComponent<Image>().sprite = emptySprite;
            }
            bag = value;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
