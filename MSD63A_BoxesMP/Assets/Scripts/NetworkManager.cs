using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Photon.Pun;
using UnityEngine;

public class NetworkManager : MonoBehaviour,IPunObservable
{

    private PhotonView photonView;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
       
    }

    // Start is called before the first frame update
    void Start()
    {
        photonView = PhotonView.Get(this);
    }

    public void ChangeSizes(string jsonString)
    {
        //send message to all connected players (even master client) with random sizes
        photonView.RPC("ChangeSizesRPC", RpcTarget.All, jsonString);
    }

    [PunRPC]
    public void ChangeSizesRPC(string jsonSizes)
    {
        List<PlayerInfo> playerInfos = JsonConvert.DeserializeObject<List<PlayerInfo>>(jsonSizes);
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject player in players)
        {
            player.GetComponent<PlayerController>().ChangeSizeFromMaster(playerInfos);
        }

    }

    public void DestroyPlayer(int destroyPlayerId)
    {
        //send message to everyone (including master client) to destroy object with destroyPlayerId
        photonView.RPC("DestroyPlayerRPC", RpcTarget.All, destroyPlayerId);
    }

    [PunRPC]
    public void DestroyPlayerRPC(int destroyPlayerId)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            if(player.GetComponent<PhotonView>().Owner.ActorNumber == destroyPlayerId)
            {
                if (player.GetComponent<PhotonView>().AmOwner) //you have to be the owner of that PhotonView to destroy
                {
                    PhotonNetwork.Destroy(player);
                }
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
