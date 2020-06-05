using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInGameStatus : MonoBehaviour
{
    [SerializeField]
    Text txtNumPlayers;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (txtNumPlayers)
        {
            txtNumPlayers.text = PhotonNetwork.PlayerList.Length + "人";
        }
    }
}
