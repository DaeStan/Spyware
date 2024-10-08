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

    bool activePlayer = false;
    int winningcard = -1;
    bool canWin = false;

    int cardPlayed;

    int[] currentPlayerHand;

    [PunRPC]
    public void Initialize(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;
        Debug.Log("player entered game");
        Debug.Log(PhotonNetwork.PlayerList[id - 1].NickName);
        GameManager.instance.players[id - 1] = this;
        Debug.Log("Initalize player Id: " + id);
        CardManager.instance.DealCards(id);
    }
 
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
        //Debug.Log("playerturn cureent id: " + id);

        var cardManager = CardManager.instance;
        currentPlayerHand = cardManager.currentPlayerHansds[id];

        if (id == 1 && canWin == false)
        {
            Debug.Log("this is the first turn player 1 may start...");
            activePlayer = true;
        }

        for (int i = 0; i < currentPlayerHand.Length; i++)
        {
            if (currentPlayerHand[i] == winningcard && canWin == true)
            {
                Debug.Log("Player " + id + " has WON!!!!!!!!!!!!!!!!!!!!!!");
            }
        }
        if (activePlayer == true) 
        {
            string selectedCard = EventSystem.current.currentSelectedGameObject.name;
            Debug.Log(id + ": " + selectedCard);
            if (winningcard == -1)
            {
                //cardNumber = selectedCard[0];
                winningcard = (int)Char.GetNumericValue(selectedCard[0]);
                Debug.Log("PLayer " + id + " choose: " + winningcard);
            }
            else
            {
                //pass card to next player
                //cardPlayed = GameObject.Find(selectedCard);
                cardPlayed = (int)Char.GetNumericValue(selectedCard[0]);
                CheckForWinningCard();
                Debug.Log("PlayerTurn Id: " + id);
                int nextPlayer = CardManager.instance.PassCard(id, cardPlayed);

                activePlayer = false;

                GameManager.instance.GetPlayer(nextPlayer).activePlayer = true;
            }
        } 
    }
    
    void CheckForWinningCard()
    {
        if (winningcard == cardPlayed)
        {
            canWin = true;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send local player data to other players
            stream.SendNext(id);
            stream.SendNext(activePlayer);
            stream.SendNext(winningcard);
            stream.SendNext(canWin);

            // Optional: Send player's current hand if it's needed on all clients
            stream.SendNext(currentPlayerHand);
        }
        else
        {
            // Receive data from the other players
            id = (int)stream.ReceiveNext();
            activePlayer = (bool)stream.ReceiveNext();
            winningcard = (int)stream.ReceiveNext();
            canWin = (bool)stream.ReceiveNext();

            // Optional: Receive current hand if synced
            currentPlayerHand = (int[])stream.ReceiveNext();
        }
    }
}