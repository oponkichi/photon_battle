using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public GameObject target { get; set; }
    // Start is called before the first frame update
    Camera mCamera;

    void Start()
    {
        mCamera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            mCamera.transform.LookAt(target.transform);
        }
    }
}
