﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCharacter : MonoBehaviourPun, IPunObservable
{
    [SerializeField]
    float speed = 5.0f;

    [SerializeField]
    GameObject statuUIPrbfab;

    Rigidbody mRigidbody;
    PlayerCamera mCamera;
    UIPlayerStatus mStatusUI;


    bool IsMine()
    {
        return !PhotonNetwork.IsConnected || photonView.IsMine;
    }

    // Start is called before the first frame update
    void Start()
    {
        mRigidbody = GetComponent<Rigidbody>();

        var uiObj = Instantiate(statuUIPrbfab);
        mStatusUI = uiObj.GetComponent<UIPlayerStatus>();
        mStatusUI.SetPlayer(this);
        mStatusUI.transform.parent = InGameManager.instance.UICanvas.transform;

        if (!IsMine())
        {
            return;
        }

        mCamera = FindObjectOfType<PlayerCamera>();
        mCamera.target = gameObject;



    }

    // Update is called once per frame
    void Update()
    {
        if(!IsMine())
        {
            return;
        }

        Vector3 accel = new Vector3();
        if (Input.GetKey(KeyCode.UpArrow))
        {
            accel.z += 1.0f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            accel.z -= 1.0f;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            accel.x -= 1.0f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            accel.x += 1.0f;
        }

        if(accel != Vector3.zero)
        {
            accel.Normalize();
            accel *= speed;

            var fwd = mCamera.transform.forward;
            var right = mCamera.transform.right;
            fwd.y = 0.0f;
            right.y = 0.0f;
            fwd.Normalize();
            right.Normalize();

            var accel2 = fwd * accel.z + right * accel.x;

            mRigidbody.AddForce(accel2 * Time.deltaTime, ForceMode.Acceleration);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {

        }
        else
        {

        }
    }
}
