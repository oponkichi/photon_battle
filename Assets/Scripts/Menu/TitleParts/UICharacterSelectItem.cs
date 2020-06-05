using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterSelectItem : MonoBehaviour
{
    [SerializeField]
    Image root;
    [SerializeField]
    Button btn;
    [SerializeField]
    RawImage thumbnail;
    [SerializeField]
    Text displayName;

    [SerializeField]
    Color selectedColor;
    [SerializeField]
    Color deselectedColor;

    CharacterDef m_Def;

    public CharacterDef characterDef => m_Def;

    public delegate void Delegate_OnSelected(UICharacterSelectItem icon);
    public Delegate_OnSelected Event_OnSelected;

    private void Start()
    {
        if(btn)
        {
            btn.onClick.AddListener( OnButtonClicked );
        }
    }

    public void SetCharacterDef(CharacterDef def)
    {
        m_Def = def;
        thumbnail.texture = def.thumbnail;
        displayName.text = def.displayName;
    }

    void OnButtonClicked()
    {
        Event_OnSelected(this);
    }

    public void SetSelected(bool bSelected)
    {
        root.color = bSelected ? selectedColor : deselectedColor;
    }


}
