using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cmrfllw : MonoBehaviour {
    public GameObject player;
    private Vector3 offset;
   
    void Start ()
    {
        offset = transform.position;
        player = GameObject.Find("player");
    }

    void LateUpdate ()
    {
        transform.position = new Vector3(offset.x + player.transform.position.x, offset.y, offset.z);
    }
}
