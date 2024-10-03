using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using System;
//import CardManager

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public int id;
    public Player photonPlayer;

   // private GameObject cardPlayed;

    //private CardManager CardManager;

    //bool playerHasWon = false;
    bool activePlayer = true;
    int winningcard = 0;
   // string cardNumber;
    int cardPlayed;

    int[] currentPlayerHand;

    [PunRPC]
    public void Initialize(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;
        //getPlayerId(player);
        Debug.Log("player entered game");
        Debug.Log(PhotonNetwork.PlayerList[id - 1].NickName);

        GameManager.instance.players[id - 1] = this;

        //CardManager = GetComponent<CardManager>();
        Debug.Log("Initalize player Id: " + id);
        CardManager.instance.dealCards(id);
    }
  /*  public int getPlayerId(Player player)
    {
        photonPlayer = player;
        Debug.Log(photonPlayer);
        id = player.ActorNumber;
        Debug.Log("Player.Actornumber: " + player.ActorNumber);
        Debug.Log("player id in get player id: " + id);
        return id;
    } */
    //function playerCards 
    //track winning card for player



    //beginging of each turn
    //check if it is players turn if player has 5 cards
    //If (winning card == 0)
    //let player choose winning card
    //set winning card = to (-)selected prefab

    //check what card the player id has

    //check if player has winning card (cardmanager winning in == current)


    //then play call function
    //get current players id and next players id
    //update current player id and currenthand dictionary
    //update next player id and currenthand dictionary

    //make it where it can handle double digits 10+
    [PunRPC]
    public void PlayerTurn()
    {
        Debug.Log("playerturn cureent id: " + id);

        var cardManager = CardManager.instance;
        currentPlayerHand = cardManager.currentPlayerHansds[id];

        for (int i = 0; i < currentPlayerHand.Length; i++)
        {
            if (currentPlayerHand[i] == null)
            {
                activePlayer = false;
            }
        }

        Debug.Log("current player array length: " + currentPlayerHand.Length);
        if (activePlayer == true) //change this to be while player has 5 cards in hand later arrayl.length = maxCardsForHand
        {
            if (id == 0)  // Check if ID is not properly initialized
            {
                Debug.LogError("Player ID is not initialized before PlayerTurn is called.");
                return;  // Exit early if id is invalid
            }

            string selectedCard = EventSystem.current.currentSelectedGameObject.name;
            Debug.Log(id + ": " + selectedCard);
            if (winningcard == 0)
            {
                //cardNumber = selectedCard[0];
                //  winningcard = int.Parse(cardNumber);
                winningcard = (int)Char.GetNumericValue(selectedCard[0]);
                Debug.Log("PLayer " + id + " choose: " + winningcard);
            }
           // if (winningcard == 7) //check to see if the winning card has left and made it back to player hand
           // {

           // }
            else
            {
                //pass card to next player
                //cardPlayed = GameObject.Find(selectedCard);
                cardPlayed = (int)Char.GetNumericValue(selectedCard[0]);
                Debug.Log("PlayerTurn Id: " + id);
                CardManager.instance.passCard(id, cardPlayed);
            }
        }
        
    }

    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
     //   throw new System.NotImplementedException();
    //}

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // This is the local player. We send data to other players.
            stream.SendNext(id);
            stream.SendNext(activePlayer);
            stream.SendNext(winningcard);
        }
        else
        {
            // This is a remote player. We receive data from the other players.
            id = (int)stream.ReceiveNext();
            activePlayer = (bool)stream.ReceiveNext();
            winningcard = (int)stream.ReceiveNext();
        }
    }

}