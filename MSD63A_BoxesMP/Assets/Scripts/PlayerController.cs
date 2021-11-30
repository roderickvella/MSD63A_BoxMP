using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviour, IPunObservable, IPunInstantiateMagicCallback
{
    Rigidbody2D body;

    float horizontal;
    float vertical;

    public float runSpeed = 5.0f;

    public FixedJoystick fixedJoystick;

    private PhotonView photonView;

    private Vector3 playerPos;
    private Vector3 playerScale;
 
    void Start()
    {
        photonView = PhotonView.Get(this);

        if (!photonView.IsMine)
        {
            //if player object (box) is not mine, then it should automatically be controlled by photon data

            //therefore destroy rigidbody
            Destroy(GetComponent<Rigidbody2D>());
        }
        else
        {
            body = GetComponent<Rigidbody2D>();
            fixedJoystick = GameObject.FindWithTag("Joystick").GetComponent<FixedJoystick>();
        }
      
    }

    void Update()
    {

        //horizontal = Input.GetAxisRaw("Horizontal");
        //vertical = Input.GetAxisRaw("Vertical");
        if (!photonView.IsMine)
        {
            //if the player is not mine, then we need to change manually its position and scale with data from server
            transform.position = Vector3.Lerp(transform.position, this.playerPos, Time.deltaTime * 10);
        }
        else
        {
            horizontal = fixedJoystick.Horizontal;
            vertical = fixedJoystick.Vertical;
        }

        transform.localScale = new Vector3(playerScale.x, playerScale.y, playerScale.z);

      
    }

    private void FixedUpdate()
    {
        if(photonView.IsMine)
            body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //we own this instance (this player-box), therefore send others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.localScale);
        }
        else
        {
            //receive data from other player
            this.playerPos = (Vector3)stream.ReceiveNext();
            this.playerScale = (Vector3)stream.ReceiveNext();

        }
    }

    private void UpdatePlayerName(string playerName)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = playerName;
    }

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        UpdatePlayerName(info.photonView.Owner.NickName);

        float size = Random.Range(0.5f, 1.5f);
        this.playerScale = new Vector3(size, size, 1);

        object[] instantiationData = info.photonView.InstantiationData;
        string colour = (string)instantiationData[0];

        if(colour == "Red")
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if(colour == "Blue")
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else if(colour == "Green")
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }

    }

    public void ChangeSizeFromMaster(List<PlayerInfo> playerInfos)
    {
        foreach(PlayerInfo playerInfo in playerInfos)
        {
            if(photonView.Owner.ActorNumber == playerInfo.actorNumber)
            {
                print("PlayerInfoSize"+playerInfo.size);
                this.playerScale = playerInfo.size;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (photonView.IsMine)
            {
                //get scale of object we collided with
                float scaleOther = collision.transform.localScale.x;

                float scaleMine = this.transform.localScale.x;

                //get id of smallest player
                int destroyPlayerId;
                if(scaleMine > scaleOther)
                {
                    destroyPlayerId = collision.gameObject.GetComponent<PlayerController>().photonView.Owner.ActorNumber;
                }
                else
                {
                    destroyPlayerId = this.photonView.Owner.ActorNumber;
                }

                //inform everyone to destroy (eat) smallest box - player
                GameObject.Find("Scripts").GetComponent<NetworkManager>().DestroyPlayer(destroyPlayerId);

            }
        }
    }
}
