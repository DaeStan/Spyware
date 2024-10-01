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

    //  [Header("Cards in Deck")]
    //  public GameObject card1;
    //  public GameObject card2;
    //  public GameObject card3;
    //  public GameObject card4;
    //  public GameObject card5;
    //  public GameObject card6;
    //  public GameObject card7;
    //   public GameObject card8;
    //  public GameObject card9;

    //get player id fromm player controller

    //function displayplayerhand
    //use id to get current hand var currenthand = get.playcontroller.currenthands[id]
    //create for loop to match array of strings
    //use result to match with card prefabs
    //set prefab to active
    // Start is called before the first frame update
    void Awake() { instance = this; }

    [PunRPC]
public void displayPLayerHand (int playerid, int[] playerHand, Dictionary<string, int> deck)
    {
        playerCanvas = GameObject.Find("Canvas");
        for (int i = 0; i < playerHand.Length; i++)
        {
            cardNumber = playerHand[i];
            if(cardNumber == 0) //get rid of once player id in other funtions are fixed
            {
                cardNumber = 1;
            }
            Debug.Log("inDisplayFunction: " + cardNumber);
            foreach (KeyValuePair<string, int> kvp in deck)
            {
                if (kvp.Value == cardNumber)
                {
                    cardPrefabName = kvp.Key;
                    Debug.Log("Got Prefab Name: " + cardPrefabName);
                    GameObject cardPrefab = Resources.Load<GameObject>(cardPrefabName);

                    // GameObject cardPrefab = cardPrefabs[cardPrefabName];
                    GameObject cardInstance = Instantiate(cardPrefab);
                    cardInstance.SetActive(true);
                    cardInstance.transform.SetParent(playerCanvas.transform);
                    //cardPrefab.SetActive(true);
                }
            }
        }
    }
}
