using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterDef
{
    public string displayName;
    public string description;
    public Texture thumbnail;
    public GameObject prefab;
}

[CreateAssetMenu(menuName = "Defs/Create CharacterDefList")]
public class CharacterDefList : ScriptableObject
{
    public CharacterDef[] characterDefs;
}
