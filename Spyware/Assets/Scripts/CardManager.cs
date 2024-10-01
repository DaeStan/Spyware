using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//import player ids (using player controller)

public class CardManager : MonoBehaviourPunCallbacks //, IPunObservable
{
    int maxCardsForHand = 5;
    int numberOfCardsInDeck;
    int shufflingDeck;
    int[] playerHand = new int[5]; //= new int[MaxCardsForHand];
    private int numberOfPlayers; // = GameManager.playersInGame;

    public GameManager GameManager;
    public PlayerController PlayerController;

    public static CardManager instance;

    void Awake() { instance = this; }

    //get player id 

    //prefab name as string for now, int
    //deck of cards
    public Dictionary<string, int>  cards = 
        new Dictionary<string, int>() {
        {"1card", 1 }, {"2card", 2} , {"3card", 3} , {"4card", 4} , {"5card", 5},
        };

    public Dictionary<int, int[]> currentPlayerHansds = new Dictionary<int, int[]>();

    //function dealCards
        //get player ids
        //currenthands Dictionary<PlayerController id, array>() {}; //make second int negitive (cards[myKey] = (-)myNewValue;)
        //deal max cards to each player id
        //make cards negitvitve in the cards dictionary to show they are dealt
   [PunRPC]
    public void dealCards(int playerid)
    {
        numberOfCardsInDeck = cards.Count;
        numberOfPlayers = GameManager.playersInGame;

        //add a way to stop duplicates later
        for (int i = 0;  i < maxCardsForHand; i++)
        {
            shufflingDeck = Random.Range(1, numberOfCardsInDeck);
            playerHand[i] = shufflingDeck;
            Debug.Log("PlayerID: " + playerid + "RandomNumber: " + playerHand[i]);
        }

        currentPlayerHansds.Add(playerid, playerHand);
        
        //displaying player and hand for testing purposes
        Debug.Log("Player ID: " + currentPlayerHansds.Keys + "Cards Held: " + currentPlayerHansds.Values);

    }

    //function passCard (current player id, next player id, passed card)
    //current player id, remove passed card from currenthand
    //next player id, add passed card to current hand






    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    ((IPunObservable)PlayerController).OnPhotonSerializeView(stream, info);
    //}
}
