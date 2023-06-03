using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance;
        public GameObject playerPrefab;

        void Start()
        {
            Instance = this;

            if (playerPrefab == null)
            {
                Debug.LogError(
                    "<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'",
                    this);
            }
            else
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
                if (PlayerManager.LocalPlayerInstance == null && Launcher.joinType != JoinRoomType.REJOIN)
                {
                    Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                    // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                    PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnJoinedRoom()
        {
            //PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void SwitchMaster()
        {
            PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
        }

        public void Disconnect()
        {
            PhotonNetwork.Disconnect();

        }

        public void RequestRoomObjOwner()
        {
            foreach (var roomObj in ObjManager.Instance.RoomObjs)
            {
                if (roomObj != null)
                {
                    PhotonView view = roomObj.GetComponent<PhotonView>();
                    if (view != null)
                    {
                        view.RequestOwnership();
                    }
                    else
                    {
                        PhotonLog.Log("RequestRoomObjOwner roomObj view miss.");
                    }
                }
                else
                {
                    PhotonLog.Log("RequestRoomObjOwner roomObj miss.");
                }
            }
        }

        public void TransferRoomObjOwner()
        {
            foreach (var roomObj in ObjManager.Instance.RoomObjs)
            {
                if (roomObj != null)
                {
                    PhotonView view = roomObj.GetComponent<PhotonView>();
                    if (view != null)
                    {
                        view.TransferOwnership(PhotonNetwork.LocalPlayer);
                    }
                    else
                    {
                        PhotonLog.Log("TransferRoomObjOwner roomObj view miss.");
                    }
                }
                else
                {
                    PhotonLog.Log("TransferRoomObjOwner roomObj miss.");
                }
            }
        }

        public void RequestPlayerObjOwner()
        {
            foreach (var playerObj in ObjManager.Instance.PlayerObjs)
            {
                if (playerObj != null)
                {
                    PhotonView view = playerObj.GetComponent<PhotonView>();
                    if (view != null)
                    {
                        view.RequestOwnership();
                    }
                    else
                    {
                        PhotonLog.Log("RequestPlayerObjOwner roomObj view miss.");
                    }
                }
                else
                {
                    PhotonLog.Log("RequestPlayerObjOwner roomObj miss.");
                }
            }
        }

        public void TransferPlayerObjOwner()
        {
            foreach (var playerObj in ObjManager.Instance.PlayerObjs)
            {
                if (playerObj != null)
                {
                    PhotonView view = playerObj.GetComponent<PhotonView>();
                    if (view != null)
                    {
                        view.TransferOwnership(PhotonNetwork.LocalPlayer);
                    }
                    else
                    {
                        PhotonLog.Log("TransferPlayerObjOwner roomObj view miss.");
                    }
                }
                else
                {
                    PhotonLog.Log("TransferPlayerObjOwner roomObj miss.");
                }
            }
        }

        void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
                return;
            }

            //Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
            //PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
            PhotonNetwork.LoadLevel("Room");
        }

        #region Photon Callbacks

        public override void OnPlayerEnteredRoom(Player other)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
        }

        public override void OnPlayerLeftRoom(Player other)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects

            //if (PhotonNetwork.IsMasterClient)
            //{
            //    Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}",
            //        PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
            //    LoadArena();
            //}
        }
        #endregion
    }
}
