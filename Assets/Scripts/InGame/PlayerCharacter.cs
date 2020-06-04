using Photon.Pun;
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
    void FixedUpdate()
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

        foreach(var info in mCollisionInfos)
        {
            mRigidbody.AddForceAtPosition(info.impluseDirection * info.impluseStrength, info.implusePos);
        }
        mCollisionInfos.Clear();
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

    private void OnCollisionEnter(Collision collision)
    {
        var me = (photonView.Owner != null) ? photonView.Owner.NickName : gameObject.name;

        var otherPun = collision.gameObject.GetComponent<MonoBehaviourPun>();
        var otherPlayer = collision.gameObject.GetComponent<PlayerCharacter>();
        var otherOwner = otherPun?.photonView.Owner;
        var him = (otherOwner != null) ? otherOwner.NickName : gameObject.name;
        Debug.Log("Colloded object :" + me + ", collider : " + him);

        if (!IsMine())
        {
            return;
        }

        //ここでForceを与えるのではなく、相手側にRPCを送信してそこで飛んでもらう
        var contact = collision.contacts[0];
        var impluseStrength = 100.0f;

        if (otherPlayer && otherOwner != null)
        {
            Debug.Log("Sent RPC_OnCollidedEventFromOtherPlayer", this);
            photonView.RPC("RPC_OnCollidedEventFromOtherPlayer", otherOwner, impluseStrength, contact.normal * -1.0f, contact.point);
            photonView.RPC("RPC_OnCollidedEventFromOtherPlayer", photonView.Owner, impluseStrength, contact.normal, contact.point);
        }
        else
        {
            var tgtRb = collision.collider.GetComponent<Rigidbody>();
            if (tgtRb)
            {
                tgtRb.AddForceAtPosition(contact.normal * -impluseStrength, contact.point);
            }
        }
    }

    struct CollisionInfo
    {
        public float impluseStrength;
        public Vector3 impluseDirection;
        public Vector3 implusePos;

        public CollisionInfo(float impluseStrength, Vector3 impluseDirection, Vector3 implusePos)
        {
            this.impluseStrength = impluseStrength;
            this.impluseDirection = impluseDirection;
            this.implusePos = implusePos;
        }
    }
    List<CollisionInfo> mCollisionInfos = new List<CollisionInfo>();


    [PunRPC]
    private void RPC_OnCollidedEventFromOtherPlayer(float impluseStrength, Vector3 impluseDirection, Vector3 implusePos)
    {
        if (!IsMine())
        {
            return;
        }

        Debug.Log("RPC_OnCollidedEventFromOtherPlayer", this);
        //mRigidbody.AddForceAtPosition(impluseDirection * impluseStrength, implusePos);

        mCollisionInfos.Add(new CollisionInfo(impluseStrength, impluseDirection, implusePos));

    }
}
