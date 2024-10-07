using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//import playercontroller

public class DisplayHand : MonoBehaviourPunCallbacks
{
    public static DisplayHand instance;

    int cardNumber;
    string cardPrefabName;

    GameObject playerCanvas;
    GameObject cardDeck;

    //get player id fromm player controller

    //function displayplayerhand
    //use id to get current hand var currenthand = get.playcontroller.currenthands[id]
    //create for loop to match array of strings
    //use result to match with card prefabs
    //set prefab to active
    // Start is called before the first frame update
    void Awake() { instance = this; }

    [PunRPC]
    public void DisplayPLayerHand(int playerid, int[] playerHand, Dictionary<string, int> deck)
    {
        playerCanvas = GameObject.Find("PlayerScreen");
        cardDeck = GameObject.Find("Deck");

        //clearing cards 
        foreach (Transform child in playerCanvas.transform)
        {
            Debug.Log("child of canvas:" + child.name);
            child.transform.SetParent(cardDeck.transform, true);
        }

        foreach (Transform child in cardDeck.transform)
        {
            Debug.Log("child of Deck:" + child.name);
            //child.transform.SetParent(cardDeck.transform, true);
        }

        for (int i = 0; i < playerHand.Length; i++)
        {
            cardNumber = playerHand[i];

            Debug.Log("inDisplayFunction: " + cardNumber);
            foreach (KeyValuePair<string, int> kvp in deck)
            {
                if (kvp.Value == cardNumber)
                {
                    cardPrefabName = kvp.Key;
                    //Debug.Log("Got Prefab Name: " + cardPrefabName);
                    GameObject cardPrefab = GameObject.Find(cardPrefabName);

                    //Debug.Log("Got card object" + cardPrefab.name);

                    cardPrefab.transform.SetParent(playerCanvas.transform, true);

                    //cardPrefab.SetActive(true);

                    // GameObject cardPrefab = cardPrefabs[cardPrefabName];
                    // GameObject cardInstance = Instantiate(cardPrefab);
                    //cardInstance.SetActive(true);
                    //Debug.Log("Got Prefab Name: " + cardPrefabName);
                    //Debug.Log("Got canvas Name: " + playerCanvas);
                    //Debug.Log("Got card object" + cardPrefab.name);
                    // cardInstance.onClick.AddListener(delegate { PlayerController.PlayerTurn(); })
                    //cardPrefab.SetActive(true);
                }
            }
        }
    }
}
