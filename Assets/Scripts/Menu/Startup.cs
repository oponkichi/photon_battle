using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

namespace PunBattle
{
    public class Startup : MenuBase
    {
        [SerializeField]
        Text mTxtConnecting = null;

        void ConnectToPhoton()
        {
            PhotonNetwork.ConnectUsingSettings();
        }


        // Update is called once per frame
        void Update()
        {
            if(mTxtConnecting)
            {
                var txt = "Connecting";
                int n = ((int)(Time.time * 5.0f)) % 5;
                for(int i = 0; i < n; ++i)
                {
                    txt += ".";
                }

                mTxtConnecting.text = txt;
            }
        }

        public override void ActivateMenu()
        {
            //TODO : Singletonに処理させる
            PhotonNetwork.AutomaticallySyncScene = true;

            base.ActivateMenu();
            ConnectToPhoton();
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connection to master server established.", this);
            base.OnConnectedToMaster();
            menuManager.ActivateMenu("Title");
        }
    }
}