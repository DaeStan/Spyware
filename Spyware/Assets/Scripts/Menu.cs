using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

public class Menu : MonoBehaviourPunCallbacks
{
    [Header("Main Screen")]
    public Button createRoomButton;
    public Button joinRoomButton;

     [Header("Screens")]
     public GameObject mainScreen;
     public GameObject lobbyScreen;

     [Header("Lobby Screen")]
     public TextMeshProUGUI playerListText;
     public Button startGameButton;

    // Start is called before the first frame update
    void Start()
    {
     // disable the buttons at the start as we're not connected to the server yet
     createRoomButton.interactable = false;
     joinRoomButton.interactable = false;
    }

    // called when we connect to the master server
    // enables the "Create Room" and "Join Room" buttons
    public override void OnConnectedToMaster ()
    {
     createRoomButton.interactable = true;
     joinRoomButton.interactable = true;
    }

    void SetScreen (GameObject screen)
    {
     // deactivate all screens
     mainScreen.SetActive(false);
     lobbyScreen.SetActive(false);
     // enable the requested screen
     screen.SetActive(true);
    }

    public void OnCreateRoomButton (TMP_InputField roomNameInput)
    {
        NetworkManager.instance.CreateRoom(roomNameInput.text);
    }

    public void OnJoinRoomButton (TMP_InputField roomNameInput)
    {
        NetworkManager.instance.JoinRoom(roomNameInput.text);
    }


    public void OnPlayerNameUpdate (TMP_InputField playerNameInput)
    {
        PhotonNetwork.NickName = playerNameInput.text;
    }

    // called when we join a room
    public override void OnJoinedRoom()
    {
        SetScreen(lobbyScreen);

        // since there's now a new player in the lobby , tell everyone to update the lobby
        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
    }

    public override void OnPlayerLeftRoom (Player otherPlayer)
    {
        // we don't RPC it like when we join the lobby
        // that's because OnJoinedRoom is only called for the cliet who just joined the room
        // OnPlayerLeftRoom gets called for all clients in the room, so we don't need to
        UpdateLobbyUI();
    }

    // updates the lobby UI to show player list and host buttons
    [PunRPC]
    public void UpdateLobbyUI ()
    {
        playerListText.text = "";

        // display all of the players currently in the lobby
        foreach(Player player in PhotonNetwork.PlayerList)
        {
            playerListText.text += player.NickName + "\n";

        }

        //only the host can start the game
        if (PhotonNetwork.IsMasterClient)
            startGameButton.interactable = true;
        else
            startGameButton.interactable = false;
    }

    public void OnLeaveLobbyButton ()
    {
        PhotonNetwork.LeaveRoom();
        SetScreen(mainScreen);
    }

    public void OnStartGameButton ()
    {
        //add check for player count
        NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Game");
    }
}