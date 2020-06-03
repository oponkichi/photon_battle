using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Canvas uiCanvas;

    [SerializeField]
    GameObject playerPrefab;
    // Start is called before the first frame update

    public static InGameManager instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    private void OnDestroy()
    {
        instance = null;
    }


    public Canvas UICanvas => uiCanvas;

    void Start()
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 0, 0), Quaternion.identity);
        }
        else
        {
            //for test
            GameObject.Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
