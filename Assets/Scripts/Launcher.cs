using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Com.MyCompany.MyGame
{
    public enum JoinRoomType
    {
        JOIN = 0,
        REJOIN = 1,
    }

    public class Launcher : MonoBehaviourPunCallbacks
    {
        [SerializeField]
        private byte maxPlayerPerRoom = 4;
        private string gameVersion = "1";
        private bool isConnecting = false;
        JoinRoomType joinType = JoinRoomType.JOIN;

        [SerializeField] 
        private GameObject controlPanel;
        [SerializeField] 
        private GameObject progressLabel;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        public void Connect()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            
            if (PhotonNetwork.IsConnected)
            {
                RoomOptions ops = new RoomOptions();
                ops.PlayerTtl = 60000;
                PhotonNetwork.JoinOrCreateRoom("river", ops, TypedLobby.Default);
            }
            else
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
                joinType = JoinRoomType.JOIN;
            }
        }

        public void Rejoin()
        {
            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.RejoinRoom("river");
            }
            else
            {
                isConnecting = PhotonNetwork.Reconnect();
                PhotonNetwork.GameVersion = gameVersion;
                joinType = JoinRoomType.REJOIN;
            }
        }

        #region Pun Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            if (isConnecting)
            {
                if (joinType == JoinRoomType.JOIN)
                {
                    RoomOptions ops = new RoomOptions();
                    PhotonNetwork.JoinOrCreateRoom("river", ops, TypedLobby.Default);
                } else {
                    PhotonNetwork.RejoinRoom("river");
                }

                isConnecting = false;
            }
        }
        
        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
            progressLabel.SetActive(false);
            controlPanel.SetActive(true);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            // #Critical: we failed to join a random room, maybe none exists or they are all full. No worries, we create a new room.
            PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = maxPlayerPerRoom});
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

            // #Critical: We only load if we are the first player, else we rely on `PhotonNetwork.AutomaticallySyncScene` to sync our instance scene.
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                Debug.Log("We load the 'Room for 1' ");

                // #Critical
                // Load the Room Level.
                PhotonNetwork.LoadLevel("Room");
            }
        }
        #endregion
    }
}