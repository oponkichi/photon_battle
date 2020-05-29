using Photon.Pun;
using Photon.Realtime;
using PunBattle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunBattle
{
    public class JoinToRoom : MenuBase
    {
        string RoomName = "PhotonBattle";

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void ActivateMenu()
        {
            base.ActivateMenu();

            Debug.Log("Joining to the room...");
            //PhotonNetwork.JoinLobby();
            var roomOptions = new RoomOptions();
            PhotonNetwork.JoinOrCreateRoom(RoomName, roomOptions, TypedLobby.Default);
        }
        /*
        public override void OnJoinedLobby()
        {
            base.OnJoinedLobby();
            Debug.Log("OnJoinedLobby");
        }*/

        public override void OnJoinedRoom()
        {
            Debug.Log("OnJoinedRoom");
            base.OnJoinedRoom();

            if( PhotonNetwork.IsMasterClient )
            {
                Debug.Log("MasterClient is loading BattleStage....");
                PhotonNetwork.LoadLevel("BattleStage");
            }
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            base.OnJoinRoomFailed(returnCode, message);
            Debug.LogError("OnJoinedRoomFailed code=" + returnCode + " message=" + message);
            menuManager.ActivateMenu("NetworkError");
        }
    }
}
