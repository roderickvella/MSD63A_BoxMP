using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //instaniate object and sync to all players in this room
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f),
            Quaternion.identity, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
