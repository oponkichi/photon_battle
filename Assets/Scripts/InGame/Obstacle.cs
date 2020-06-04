using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviourPun
{
    [SerializeField]
    Color[] colors;

    bool IsMine()
    {
        return !PhotonNetwork.IsConnected || photonView.IsMine;
    }

    Rigidbody mRigidbody;
    List<PlayerCharacter.CollisionInfo> mCollisionInfos = new List<PlayerCharacter.CollisionInfo>();

    // Start is called before the first frame update
    void Start()
    {
        if (colors != null)
        {
            var color = colors[UnityEngine.Random.Range(0, colors.Length - 1)];
            GetComponent<MeshRenderer>().material.color = color;
        }
        mRigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= -10.0)
        {
            GameObject.Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        #region 他プレイヤーとの衝突の処理
        foreach (var info in mCollisionInfos)
        {
            mRigidbody.AddForceAtPosition(info.impluseDirection * info.impluseStrength, info.implusePos);
        }
        mCollisionInfos.Clear();
        #endregion
    }

    [PunRPC]
    private void RPC_OnCollidedEventFromOtherPlayer(float impluseStrength, Vector3 impluseDirection, Vector3 implusePos)
    {
        if (!IsMine())
        {
            return;
        }

        Debug.Log("RPC_OnCollidedEventFromOtherPlayer", this);
        //mRigidbody.AddForceAtPosition(impluseDirection * impluseStrength, implusePos);

        mCollisionInfos.Add(new PlayerCharacter.CollisionInfo(impluseStrength, impluseDirection, implusePos));

    }
}
