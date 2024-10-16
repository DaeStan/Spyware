using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//import player ids (using player controller)

public class CardManager : MonoBehaviourPunCallbacks, IPunObservable
{
    public static int maxCardsForHand = 3;
    public static int maxNumberOfPlayers = 4;
    int numberOfCardsInDeck;
    int shufflingDeck;
    int[] currentPlayerHand;
    int[] nextPlayerHand;
    int nextPlayerId;
    int passedCard;
    List<int> passingCard;
    private int numberOfPlayers; // = GameManager.playersInGame;


    private List<int> currentPlayers;

    HashSet<int> cardsDelt = new HashSet<int>();

    public GameManager GameManager;
    public PlayerController PlayerController;

    public static CardManager instance;

    //inital player cards hard coded
    //int[] playerOne = {1, 2, 3};
    //int[] playerTwo = { 4, 5, 0 };
    //int[] playerThree = { 6, 7, 0 };
    //int[] playerFour = {8, 9, 0 };

    //playerOne = {1, 2, 3}


    void Awake() 
    { 
        instance = this; 
    }


    //deck of cards
    public Dictionary<string, int>  cards = 
        new Dictionary<string, int>() {
        {"1card", 1 }, {"2card", 2} , {"3card", 3} , {"4card", 4} , {"5card", 5},
            {"6card", 6}, {"7card", 7}, {"8card", 8}, {"9card", 9}
        };

    public Dictionary<int, int[]> currentPlayerHansds =
        new Dictionary<int, int[]>(); //{
            //{1, playerOne}
       // };


    //ShuffleCards function
    [PunRPC]
    public void ShuffleDeck()
    {
        numberOfCardsInDeck = cards.Count;

        for (int j = 1; j <= maxNumberOfPlayers; j++)
        {
            int[] playerHand = new int[maxCardsForHand];

            for (int i = 0; i < maxCardsForHand - 1; i++)
            {
                do
                {
                    shufflingDeck = Random.Range(1, numberOfCardsInDeck + 1);
                } while (cardsDelt.Contains(shufflingDeck));

                playerHand[i] = shufflingDeck;
                cardsDelt.Add(shufflingDeck);
            }
            if (j == 1)
            {
                do
                {
                    shufflingDeck = UnityEngine.Random.Range(1, numberOfCardsInDeck + 1);
                } while (cardsDelt.Contains(shufflingDeck));

                playerHand[maxCardsForHand - 1] = shufflingDeck;
                cardsDelt.Add(shufflingDeck);
                currentPlayerHansds.Add(j, playerHand);
            }
            else
            {
                playerHand[maxCardsForHand - 1] = 0;
                currentPlayerHansds.Add(j, playerHand);
            }
        }
    } 


    //function dealCards
    //get player ids
    //currenthands Dictionary<PlayerController id, array>() {}; //make second int negitive (cards[myKey] = (-)myNewValue;)
    //deal max cards to each player id
    //make cards negitvitve in the cards dictionary to show they are dealt
    [PunRPC]
    public void DealCards(int playerid)
    {
        DisplayHand.instance.DisplayPLayerHand(playerid, currentPlayerHansds[playerid], cards);
    }


    //function passCard (current player id, next player id, passed card)
    //current player id, remove passed card from currenthand
    //next player id, add passed card to current hand
    [PunRPC]
    public int PassCard(int currentPlayerId, int passedCard)
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
        DisplayList(passingCard);
        currentPlayerHand = passingCard.ToArray();
        currentPlayerHansds[currentPlayerId] = currentPlayerHand; //may have to update this array for the id
        DisplayHand.instance.DisplayPLayerHand(currentPlayerId, currentPlayerHand, cards);

        //adding card to next players hand
        passingCard = new List<int>(nextPlayerHand);
        if (passingCard.Contains(0))
        {
            passingCard.Remove(0);
        }
        passingCard.Add(passedCard);
        DisplayList(passingCard);
        nextPlayerHand = passingCard.ToArray();
        currentPlayerHansds[nextPlayerId] = nextPlayerHand;
        DisplayHand.instance.DisplayPLayerHand(nextPlayerId, nextPlayerHand, cards);

        return nextPlayerId;
    }


    void DisplayList(List<int> passingCard)
    {
        Debug.Log("displaying List:   ");
        foreach (int card in passingCard)
        {
            Debug.Log(card);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Send data to other clients
            stream.SendNext(currentPlayerHansds);
            stream.SendNext(cardsDelt);
        }
        else
        {
            // Receive data from other clients
            currentPlayerHansds = (Dictionary<int, int[]>)stream.ReceiveNext();
            cardsDelt = (HashSet<int>)stream.ReceiveNext();
        }
    }
}
