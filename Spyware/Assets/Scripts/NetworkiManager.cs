    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Photon.Pun;

    public class NetworkManager : MonoBehaviourPunCallbacks
    {
    // instance
    public static NetworkManager instance;
        void Awake ()
        {
            // if an instance already exists and it's not this one - destroy us
            if(instance != null && instance != this)
                gameObject.SetActive(false);
            else
            {
                // set the instance
                instance = this;
                DontDestroyOnLoad(gameObject);
            }   
        }

        // Start is called before the first frame update
        void Start()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        // attempt to create a new room
        public void CreateRoom (string roomName)
        {
            PhotonNetwork.CreateRoom(roomName);
            Debug.Log("Rm made");
        }

        // attempt to join an existing room
        public void JoinRoom (string roomName)
        {
            PhotonNetwork.JoinRoom(roomName);
        }

        // changes the scene using Photon's system
        [PunRPC]
        public void ChangeScene (string sceneName)
        {
         PhotonNetwork.LoadLevel(sceneName);
        }

        public override void OnConnectedToMaster ()
        {
            Debug.Log("Connected to master server");
            PhotonNetwork.JoinLobby();
        }
    }