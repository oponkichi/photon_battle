using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerCharacter : MonoBehaviourPun, IPunObservable
{
    public struct CollisionInfo
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

    [SerializeField]
    float speed = 5.0f;
    [SerializeField]
    float attackPower = 500.0f;
    [SerializeField]
    GameObject obstaclePrefab = null;

    [SerializeField]
    GameObject statuUIPrbfab = null;

    Rigidbody mRigidbody;
    PlayerCamera mCamera;
    UIPlayerStatus mStatusUI;
    List<CollisionInfo> mCollisionInfos = new List<CollisionInfo>();

    #region Replicated fields

    float mElapsedTimeFromSpawn = 0.0f;



    #endregion Replicated fields

    float mStaminna;
    float mMaxStamina;


    public float ElapsedTimeFromSpawn => mElapsedTimeFromSpawn;

    enum Button
    {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    bool IsButtonDOwn(Button btn)
    {
        switch (btn)
        {
            case Button.UP:
                return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
            case Button.DOWN:
                return Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
            case Button.LEFT:
                return Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
            case Button.RIGHT:
                return Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
        }
        return false;
    }


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
        if (IsMine())
        {
            if (obstaclePrefab)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    var pos = transform.position;
                    pos.y += 2.0f;
                    InGameManager.instance.InstantiateObject(obstaclePrefab, pos, Quaternion.identity);
                }
            }

            mElapsedTimeFromSpawn += Time.deltaTime;
        }
    }


    void FixedUpdate()
    {
        if(!IsMine())
        {
            return;
        }

# region 入力の処理
        Vector3 accel = new Vector3();
        if (IsButtonDOwn(Button.UP))
        {
            accel.z += 1.0f;
        }
        if (IsButtonDOwn(Button.DOWN))
        {
            accel.z -= 1.0f;
        }
        if (IsButtonDOwn(Button.LEFT))
        {
            accel.x -= 1.0f;
        }
        if (IsButtonDOwn(Button.RIGHT))
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
        #endregion

        #region 他プレイヤーとの衝突の処理
        if (mElapsedTimeFromSpawn >= 1.0f)
        {
            foreach (var info in mCollisionInfos)
            {
                mRigidbody.AddForceAtPosition(info.impluseDirection * info.impluseStrength, info.implusePos);
            }
        }
        mCollisionInfos.Clear();
        #endregion

        //落ちたら死ぬ
        if (transform.position.y < -50.0f)
        {
            InGameManager.instance.ExitGame();
            gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if(mStatusUI)
        {
            GameObject.Destroy(mStatusUI.gameObject);
        }
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(mElapsedTimeFromSpawn);
        }
        else
        {
            mElapsedTimeFromSpawn = (float)stream.ReceiveNext();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(mElapsedTimeFromSpawn < 1.0f)
        {
            return;
        }


        var me = (photonView.Owner != null) ? photonView.Owner.NickName : gameObject.name;

        var otherPun = collision.gameObject.GetComponent<MonoBehaviourPun>();
        var otherPlayer = collision.gameObject.GetComponent<PlayerCharacter>();
        var otherView = otherPun?.photonView;
        var otherOwner = otherPun?.photonView.Owner;
        var him = (otherOwner != null) ? otherOwner.NickName : gameObject.name;
        Debug.Log("Colloded object :" + me + ", collider : " + him);

        if (!IsMine())
        {
            return;
        }

        //ここでForceを与えるのではなく、相手側にRPCを送信してそこで飛んでもらう
        var contact = collision.contacts[0];
        var impluseStrength = attackPower;

        if (/*otherPlayer &&*/ otherOwner != null)
        {
            Debug.Log("Sent RPC_OnCollidedEventFromOtherPlayer", this);
            otherView.RPC("RPC_OnCollidedEventFromOtherPlayer", otherOwner, impluseStrength, contact.normal * -1.0f, contact.point);

            //Player相手の場合は自分にも反動を与える
            if (otherPlayer)
            {
                photonView.RPC("RPC_OnCollidedEventFromOtherPlayer", photonView.Owner, impluseStrength, contact.normal, contact.point);
            }
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
