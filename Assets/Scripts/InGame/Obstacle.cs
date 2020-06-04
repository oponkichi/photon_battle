using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    Color[] colors;

    // Start is called before the first frame update
    void Start()
    {
        if (colors != null)
        {
            var color = colors[UnityEngine.Random.Range(0, colors.Length - 1)];
            GetComponent<MeshRenderer>().material.color = color;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.position.y <= -10.0)
        {
            GameObject.Destroy(gameObject);
        }
    }
}
