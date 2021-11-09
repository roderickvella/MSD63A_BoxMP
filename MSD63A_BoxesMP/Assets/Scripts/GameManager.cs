using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //instaniate object and sync to all players in this room
       // PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f),
         //   Quaternion.identity, 0);
    }

    public void SpawnButton()
    {
        TMP_Dropdown dropdown = GameObject.Find("DropdownColour").GetComponent<TMP_Dropdown>();

        //gives you the selected colour from dropdown
        string colour = dropdown.options[dropdown.value].text;

        //pass the colour to the OnPhotonInstantiate inside PlayerController
        object[] myCustomInitData = new object[1] { colour };

         PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f),
           Quaternion.identity, 0,myCustomInitData);



    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
