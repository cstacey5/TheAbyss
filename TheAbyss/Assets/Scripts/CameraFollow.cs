using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //get the transform of our playermodel
    public Transform playerModel;

    void Update()
    {
        //change the transform of our camera to our playermodels x, y coordinate and keep z the same since this is a 2d game
        transform.position = new Vector3(playerModel.position.x, playerModel.position.y, transform.position.z);
    }
}
