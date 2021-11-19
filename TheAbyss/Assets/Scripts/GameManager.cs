using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //got help from a youtube series that is for making an rpg type game
    //link: https://www.youtube.com/playlist?list=PLX-uZVK_0K_6JEecbu3Y-nVnANJznCzix

    [SerializeField]
    private Player player;

    private NPC currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(LayerMask.GetMask("Clickable"));
        Debug.Log(player.MyTarget);
        ClickTarget();
    }

    //system for targetting an enemy
    private void ClickTarget()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //cast a ray from our mouse position to the game world
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, 512);

            //if we hit something
            if (hit.collider != null)
            {
                if(currentTarget != null)
                {
                    currentTarget.UnselectTarget();
                }

                currentTarget = hit.collider.GetComponent<NPC>();

                player.MyTarget = currentTarget.SelectTarget();
            }
            //if we dont hit something
            else
            {
                if(currentTarget != null)
                {
                    currentTarget.UnselectTarget();
                }
                currentTarget = null;
                player.MyTarget = null;
            }
        }
    }
}
