using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInstance : MonoBehaviour
{
    static GameInstance msInstance;
    public static GameInstance instance => msInstance;

    public CharacterDefList charaterDefList;


    public CharacterDef setectedCharacter {get; set;}

    // Start is called before the first frame update
    void Start()
    {
        if(msInstance != null)
        {
            Destroy(gameObject);
        }

        GameObject.DontDestroyOnLoad(gameObject);
        msInstance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
