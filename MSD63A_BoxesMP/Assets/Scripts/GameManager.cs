using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
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

        if (PhotonNetwork.IsMasterClient)
        {
            GameObject.Find("ButtonChangeSizes").SetActive(true);
        }
        else
        {
            GameObject.Find("ButtonChangeSizes").SetActive(false);
        }
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

    public void ChangeSizesButton()
    {
        List<PlayerInfo> playerInfos = new List<PlayerInfo>();
        Photon.Realtime.Player[] allPlayers = PhotonNetwork.PlayerList;

        foreach(Photon.Realtime.Player player in allPlayers)
        {
            float size = Random.Range(0.5f, 1.5f);
            playerInfos.Add(new PlayerInfo() { actorNumber = player.ActorNumber, size = new Vector3(size, size, 1) });
        }

        string json = JsonConvert.SerializeObject(playerInfos);
        GetComponent<NetworkManager>().ChangeSizes(json);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
