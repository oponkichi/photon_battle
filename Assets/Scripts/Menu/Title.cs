using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace PunBattle
{
    public class Title : MenuBase
    {
        string KeyPlayerName = "PlayerName";
        string KeySelectedCharacterIndex = "SelectedChrIdx";

        string playerName = null;

        [SerializeField]
        InputField inputPlayerName;

        [SerializeField]
        ScrollRect svCharacterSelect;
        [SerializeField]
        GameObject characterIconPrefab;
        [SerializeField]
        Text txtCurrentChrName;
        [SerializeField]
        Text txtCurrentChrDesc;

        List<UICharacterSelectItem> mCharacterIcons = new List<UICharacterSelectItem>();


        // Start is called before the first frame update
        public override void ActivateMenu()
        {
            base.ActivateMenu();

            playerName = PlayerPrefs.GetString(KeyPlayerName);
            inputPlayerName.text = playerName; 
            PhotonNetwork.NickName = playerName;

            SetupCharacterSelector();

            var chrIdx = Mathf.Min( PlayerPrefs.GetInt(KeySelectedCharacterIndex), mCharacterIcons.Count - 1);
            //var icon = FindIconByDef(GameInstance.instance.setectedCharacter);

            OnCharacterSelected(mCharacterIcons[chrIdx]);

            svCharacterSelect.horizontalNormalizedPosition = (float)chrIdx / (mCharacterIcons.Count - 1);
        }

        // Update is called once per frame
        void Update()
        {

        }


        public void OnStartButtonPressed()
        {
            Debug.Log("Pressed S T A R T", this);
            menuManager.ActivateMenu("JoinToRoom");
        }

        public void OnPlayerNameChanged()
        {
            playerName = inputPlayerName.text;
            PlayerPrefs.SetString(KeyPlayerName, playerName);
            Debug.Log("PlayerName updated : " + playerName, this);
            PhotonNetwork.NickName = playerName;
        }


        public void SetupCharacterSelector()
        {
            if(svCharacterSelect.content.childCount > 0)
            {
                return;
            }

            if(svCharacterSelect != null)
            {
                foreach(var def in GameInstance.instance.charaterDefList.characterDefs)
                {
                    var item = Instantiate(characterIconPrefab);
                    item.GetComponent<RectTransform>().SetParent(svCharacterSelect.content.transform);
                    //svCharacterSelect.content.

                    var icon = item.GetComponent<UICharacterSelectItem>();
                    icon.SetCharacterDef(def);
                    icon.Event_OnSelected += OnCharacterSelected;
                    mCharacterIcons.Add(icon);

                }
            }
        }


        UICharacterSelectItem FindIconByDef(CharacterDef def)
        {
            foreach (var chrIcon in mCharacterIcons)
            {
                if(chrIcon.characterDef == def)
                {
                    return chrIcon;
                }
            }
            return null;
        }
        void OnCharacterSelected(UICharacterSelectItem icon)
        {
            foreach(var chrIcon in mCharacterIcons)
            {
                bool selected = icon == chrIcon;
                chrIcon.SetSelected(selected);
                if(selected)
                {
                    GameInstance.instance.setectedCharacter = icon.characterDef;
                    var chrIdx = mCharacterIcons.FindIndex(a => a == chrIcon);

                    txtCurrentChrName.text = icon.characterDef.displayName;
                    txtCurrentChrDesc.text = icon.characterDef.description;

                    PlayerPrefs.SetInt(KeySelectedCharacterIndex, chrIdx);
                }

            }

        }
    }

}