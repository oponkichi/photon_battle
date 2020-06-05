using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        var prefab = GameInstance.instance?.setectedCharacter?.prefab;
        if(!prefab)
        {
            prefab = playerPrefab;
        }

        InstantiateObject(prefab, new Vector3(0, 3.0f, 0), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void InstantiateObject(GameObject prefab, Vector3 pos, Quaternion rotation)
    {
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.Instantiate(prefab.name, pos, rotation);
        }
        else
        {
            //for test
            GameObject.Instantiate(prefab, pos, rotation);
        }
    }

    public void ExitGame()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene("Menu");
    }

}
