using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject target
    {
        get { return mTarget; } 
        set
        {
            mTarget = value;
            mPrevLookAtPos = mTarget ? mTarget.transform.position : Vector3.zero;
        }
    }

    // Start is called before the first frame update
    Camera mCamera;
    Vector3 mOffset;
    GameObject mTarget;
    Vector3 mPrevLookAtPos;

    [SerializeField]
    float posSpring = 0.8f;
    [SerializeField]
    float lookAtSpring = 0.8f;

    void Start()
    {
        mCamera = GetComponent<Camera>();
        mOffset = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(target != null)
        {
            var curCamPos = transform.position;
            var tgtPos = target.transform.position;

            var tgtCamPos = tgtPos + mOffset;
            transform.position = Vector3.Lerp( curCamPos, tgtCamPos, posSpring );

            mPrevLookAtPos = Vector3.Lerp(mPrevLookAtPos, tgtPos, lookAtSpring);
            mCamera.transform.LookAt(mPrevLookAtPos);
            
        }
    }
}
