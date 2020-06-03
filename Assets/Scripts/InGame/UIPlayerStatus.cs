using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerStatus : MonoBehaviour
{
    [SerializeField]
    Text txtPlayerName;

    PlayerCharacter mPlayer;


    public void SetPlayer(PlayerCharacter target)
    {
        mPlayer = target;
        if(txtPlayerName)
        {
            if (mPlayer.photonView.Owner != null)
            {
                txtPlayerName.text = mPlayer.photonView.Owner.NickName;
            }
            else
            {
                txtPlayerName.text = "Dummy";
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(mPlayer && txtPlayerName)
        {
            var scrPos = Camera.main.WorldToScreenPoint(mPlayer.transform.position);
            txtPlayerName.rectTransform.position = scrPos;
        }
    }
}
