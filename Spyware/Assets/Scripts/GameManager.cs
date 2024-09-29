using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Linq;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static GameManager instance;

    public GameObject playerPrefabLocation; // path in Resources folder to the Player prefab

    public PlayerController[] players; // array of all the players
    private int playersInGame; // number of players in the game

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
        if (playersInGame == PhotonNetwork.PlayerList.Length)
        {
            GameObject player = GameObject.Instantiate(playerPrefabLocation, new Vector3(0, 0, 0), Quaternion.identity);


            player.GetComponent<PlayerController>().photonView.RPC("Initalize", RpcTarget.All, PhotonNetwork.LocalPlayer);
        }

    }
}