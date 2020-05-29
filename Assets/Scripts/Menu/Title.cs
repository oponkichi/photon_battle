using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PunBattle
{
    public class Title : MenuBase
    {
        string KeyPlayerName = "PlayerName";

        string playerName = null;

        [SerializeField]
        InputField inputPlayerName;

        // Start is called before the first frame update
        public override void ActivateMenu()
        {
            base.ActivateMenu();

            playerName = PlayerPrefs.GetString(KeyPlayerName);
            inputPlayerName.text = playerName;
        }

        // Update is called once per frame
        void Update()
        {

        }


        public void OnStartButtonPressed()
        {
            Debug.Log("Pressed S T A R T", this);
            menuManager.ActivateMenu("JoinToRoom");
        }

        public void OnPlayerNameChanged()
        {
            playerName = inputPlayerName.text;
            PlayerPrefs.SetString(KeyPlayerName, playerName);
            Debug.Log("PlayerName updated : " + playerName, this);
        }
    }

}