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
        if (photonView.IsMine && playersInGame == PhotonNetwork.PlayerList.Length)
        {
            GameObject player = PhotonNetwork.Instantiate(playerPrefabLocation, new Vector3(0, 0, 0), Quaternion.identity);

            PlayerController playerScript = player.GetComponent<PlayerController>();

            playerScript.photonView.RPC("Initialize", RpcTarget.All, PhotonNetwork.LocalPlayer);
        }

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
        // int currentPlayerId = activePlayer.getPlayerId(player);
        // Debug.Log("current player id: " + currentPlayerId);
        //activePlayer.PlayerTurn();
    }
}