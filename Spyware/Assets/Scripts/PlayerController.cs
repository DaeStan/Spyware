using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;

//import CardManager

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public int id;
    public Player photonPlayer;

    private CardManager CardManager;

    //bool for winnig card
    //int winningcard = 0;

    [PunRPC]
    public void Initialize(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;
        Debug.Log("player entered game");
        Debug.Log(PhotonNetwork.PlayerList[id - 1].NickName);

        GameManager.instance.players[id - 1] = this;

        //CardManager = GetComponent<CardManager>();
        CardManager.instance.dealCards(id);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }

    //function playerCards 
    //track winning card for player



    //beginging of each turn
        //If (winning card == 0)
        //let player choose winning card
        //set winning card = to (-)selected prefab

        //check what card the player id has
        //
        //check if player has winning card (cardmanager winning in == current)
            

    //then play
        //get current players id and next players id
        //update current player id and currenthand dictionary
        //update next player id and currenthand dictionary

 
}