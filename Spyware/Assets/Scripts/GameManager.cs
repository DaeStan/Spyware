using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    public string playerPrefabLocation; // path in Resources folder to the Player prefab

    public PlayerController[] players; // array of all the players
    public int playersInGame; // number of players in the game

//    public PlayerController activePlayer;

    private void Awake()
    {
        instance = this;
    }

    [PunRPC]
    void Start()
    {
        players = new PlayerController[PhotonNetwork.PlayerList.Length];
        photonView.RPC("ImInGame", RpcTarget.All);
    }

    [PunRPC]
    void ImInGame()
    {
        playersInGame++;
        if (playersInGame == PhotonNetwork.PlayerList.Length) //photonView.IsMine && 
        {
            if (PhotonNetwork.IsMasterClient)
            {
                CardManager.instance.ShuffleDeck();
                Debug.Log("Cards Shuffled");
            }

            GameObject player = PhotonNetwork.Instantiate(playerPrefabLocation, new Vector3(0, 0, 0), Quaternion.identity);

            PlayerController playerScript = player.GetComponent<PlayerController>();

            playerScript.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
        }

    }

    public PlayerController GetPlayer(int id)
    {
        foreach (PlayerController player in players)
        {
            if (player != null && player.id == id)
            {
                return player;
            }
        }
        return null;
    }

    [PunRPC]
    public void OnButtonClicked()
    {
        foreach (PlayerController player in players)
        {
            if (player.photonView.IsMine)
            {
                player.PlayerTurn();
            }
        }
    }
}