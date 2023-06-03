using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace Com.MyCompany.MyGame
{
    public class ObjManager : MonoBehaviourPunCallbacks, IPunOwnershipCallbacks
    {
        private static ObjManager _instance;
        public static ObjManager Instance { get { return _instance; } }

        public void Start()
        {
            GameObject.DontDestroyOnLoad(this.gameObject);

            _instance = this;
        }

        public void OnDestroy()
        {
            _instance = null;
        }

        private List<GameObject> roomObjs = new List<GameObject>();
        public void AddRoomObj(GameObject obj)
        {
            roomObjs.Add(obj);
        }

        public void RemoveRoomObj(GameObject obj)
        {
            roomObjs.Remove(obj);
        }

        public List<GameObject> RoomObjs
        {
            get { return roomObjs; }
        }

        private List<GameObject> playerObjs = new List<GameObject>();
        public void AddPlayerObj(GameObject obj)
        {
            playerObjs.Add(obj);
        }

        public void RemovePlayerObj(GameObject obj)
        {
            playerObjs.Remove(obj);
        }

        public List<GameObject> PlayerObjs
        {
            get { return playerObjs; }
        }

        public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
        {
            PhotonLog.LogFormat("===> Client{0} OnOwnershipRequest viewid:{1} rplayer:{2}",
                PhotonNetwork.LocalPlayer != null ? PhotonNetwork.LocalPlayer.ActorNumber : -1, 
                targetView.ViewID, requestingPlayer.ActorNumber);
        }

        public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
        {
            PhotonLog.LogFormat("===> Client{0} OnOwnershipTransfered viewid:{1} prevplayer:{2}",
                PhotonNetwork.LocalPlayer != null ? PhotonNetwork.LocalPlayer.ActorNumber : -1,
                targetView.ViewID, previousOwner.ActorNumber);
        }

        public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
        {
            PhotonLog.LogFormat("===> Client{0} OnOwnershipTransferFailed viewid:{1} senderplayer:{2}",
                PhotonNetwork.LocalPlayer != null ? PhotonNetwork.LocalPlayer.ActorNumber : -1,
                targetView.ViewID, senderOfFailedRequest.ActorNumber);
        }
    }
}
