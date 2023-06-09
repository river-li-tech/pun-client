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
        public static JoinRoomType joinType = JoinRoomType.JOIN;

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
            joinType = JoinRoomType.JOIN;
            PhotonNetwork.EnableViewSynchronization = false;

            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            
            if (PhotonNetwork.IsConnected)
            {
                JoinRoom();
            }
            else
            {
                isConnecting = PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        public void Rejoin()
        {
            joinType = JoinRoomType.REJOIN;
            PhotonNetwork.EnableViewSynchronization = false;

            progressLabel.SetActive(true);
            controlPanel.SetActive(false);
            if (PhotonNetwork.IsConnected)
            {
                RejoinRoom();
            }
            else
            {
                isConnecting = PhotonNetwork.Reconnect();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }

        private void JoinRoom()
        {
            RoomOptions ops = new RoomOptions();
            ops.PlayerTtl = 300000;
            PhotonNetwork.JoinOrCreateRoom("river", ops, TypedLobby.Default);
        }

        private void RejoinRoom()
        {
            PhotonNetwork.RejoinRoom("river");
        }

        #region Pun Callbacks
        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
            if (isConnecting)
            {
                if (joinType == JoinRoomType.JOIN)
                {
                    JoinRoom();
                } else {
                    RejoinRoom();
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