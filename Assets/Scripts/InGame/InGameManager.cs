﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviourPunCallbacks
{

    [SerializeField]
    GameObject playerPrefab;
    // Start is called before the first frame update

    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}