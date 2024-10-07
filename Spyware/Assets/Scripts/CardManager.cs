using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

//import player ids (using player controller)

public class CardManager : MonoBehaviourPunCallbacks //, IPunObservable
{
    public static int maxCardsForHand = 3;
    int numberOfCardsInDeck;
    int shufflingDeck;
    int[] playerHand = new int[maxCardsForHand]; //= new int[MaxCardsForHand];
    int[] currentPlayerHand;
    int[] nextPlayerHand;
    int nextPlayerId;
    int passedCard;
    List<int> passingCard;
    private int numberOfPlayers; // = GameManager.playersInGame;

    HashSet<int> cardsDelt = new HashSet<int>();

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
            {"6card", 6}, {"7card", 7}, {"8card", 8}, {"9card", 9}
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

        for (int i = 0; i < maxCardsForHand - 1; i++)
        {
            do
            {
                shufflingDeck = UnityEngine.Random.Range(1, numberOfCardsInDeck);
            } while (cardsDelt.Contains(shufflingDeck));

            playerHand[i] = shufflingDeck;
            cardsDelt.Add(shufflingDeck);

            //Debug.Log("PlayerID: " + playerid + "RandomNumber: " + playerHand[i]);
        }

        if (playerid == 1)
        {
            do
            {
                shufflingDeck = UnityEngine.Random.Range(1, numberOfCardsInDeck);
            } while (cardsDelt.Contains(shufflingDeck));

            playerHand[maxCardsForHand - 1] = shufflingDeck;
        }

        currentPlayerHansds.Add(playerid, playerHand);

        DisplayHand.instance.displayPLayerHand(playerid, currentPlayerHansds[playerid], cards);

        //displaying player and hand for testing purposes
        //Debug.Log("Player ID: " + currentPlayerHansds[playerid] + "Cards Held: " + currentPlayerHansds.Values);


        //add a way to stop duplicates later
        /*       for (int i = 0;  i < maxCardsForHand - 1; i++)
               {
                   shufflingDeck = Random.Range(1, numberOfCardsInDeck);
                   playerHand[i] = shufflingDeck;
                   Debug.Log("PlayerID: " + playerid + "RandomNumber: " + playerHand[i]);
                   if (playerid == 1)
                   {
                       playerHand[4] = Random.Range(1, numberOfCardsInDeck);
                   }
               } 

               currentPlayerHansds.Add(playerid, playerHand); */
    }

    //function passCard (current player id, next player id, passed card)
    //current player id, remove passed card from currenthand
    //next player id, add passed card to current hand
    [PunRPC]
    public void passCard(int currentPlayerId, int passedCard)
    {
        numberOfPlayers = GameManager.playersInGame;//might move this outside of functions later

        Debug.Log("passCard playerID: " + currentPlayerId);
        currentPlayerHand = currentPlayerHansds[currentPlayerId];
        if (currentPlayerId + 1  > numberOfPlayers)
        {
            nextPlayerHand = currentPlayerHansds[1];
            nextPlayerId = 1;
        }
        else
        {
            nextPlayerHand = currentPlayerHansds[currentPlayerId + 1];
            nextPlayerId = currentPlayerId + 1;
        }

        //convert array to list to remove card and replace it with 0
        passingCard = new List<int>(currentPlayerHand);
        passingCard.Remove(passedCard);
        passingCard.Add(0);
        displayList(passingCard);
        currentPlayerHand = passingCard.ToArray();
        currentPlayerHansds[currentPlayerId] = currentPlayerHand; //may have to update this array for the id
        DisplayHand.instance.displayPLayerHand(currentPlayerId, currentPlayerHand, cards);

        //adding card to next players hand
        passingCard = new List<int>(nextPlayerHand);
        if (passingCard.Contains(0))
        {
            passingCard.Remove(0);
        }
        passingCard.Add(passedCard);
        displayList(passingCard);
        nextPlayerHand = passingCard.ToArray();
        currentPlayerHansds[nextPlayerId] = nextPlayerHand;
        DisplayHand.instance.displayPLayerHand(nextPlayerId, nextPlayerHand, cards);
    }


    void displayList(List<int> passingCard)
    {
        Debug.Log("displaying List:   ");
        foreach (int card in passingCard)
        {
            Debug.Log(card);
        }
    }


    //public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    //{
    //    ((IPunObservable)PlayerController).OnPhotonSerializeView(stream, info);
    //}
}
