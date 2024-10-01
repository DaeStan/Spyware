using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

//import CardManager

public class PlayerController : MonoBehaviourPunCallbacks, IPunObservable
{
    public int id;
    public Player photonPlayer;

   // private GameObject cardPlayed;

    private CardManager CardManager;

    //bool playerHasWon = false;
    //bool activePlayer = false;
    int winningcard = 0;
    string cardNumber;
    int cardPlayed;

    [PunRPC]
    public void Initialize(Player player)
    {
        photonPlayer = player;
        id = player.ActorNumber;
        Debug.Log("player entered game");
        Debug.Log(PhotonNetwork.PlayerList[id - 1].NickName);

        GameManager.instance.players[id - 1] = this;

        //CardManager = GetComponent<CardManager>();
        Debug.Log("Initalize player Id: " + id);
        CardManager.instance.dealCards(id);
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
        while (true) //change this to be while player has 5 cards in hand later arrayl.length = maxCardsForHand
        {
            //id = PhotonNetwork.LocalPlayer.ActorNumber;
           // if (id == 0) //quick fix for now
           // {
           //     id = 1;
           // }
            string selectedCard = EventSystem.current.currentSelectedGameObject.name;
            Debug.Log(id + ": " + selectedCard);
            if (winningcard == 0)
            {
              //  cardNumber = selectedCard[0];
              //  winningcard = int.Parse(cardNumber);
                winningcard = selectedCard[0]; //this is not giving correct card number fix later
                Debug.Log("PLayer " + id + " choose: " + winningcard);
            }
            if (winningcard == 7) //check to see if the winning card has left and made it back to player hand
            {

            }
            else
            {
                //pass card to next player
                //cardPlayed = GameObject.Find(selectedCard);
                cardPlayed = selectedCard[0];
                Debug.Log("PlayerTurn Id: " + id);
                CardManager.instance.passCard(id, cardPlayed);
            }
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        throw new System.NotImplementedException();
    }

}