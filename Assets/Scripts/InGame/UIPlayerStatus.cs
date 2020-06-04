using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerStatus : MonoBehaviour
{
    [SerializeField]
    Image pnlRoot;
    [SerializeField]
    Text txtPlayerName;
    [SerializeField]
    Text txtElapsedTime;

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
        if(mPlayer)
        {

            if (pnlRoot)
            {
                var scrPos = Camera.main.WorldToScreenPoint(mPlayer.transform.position);
                //txtPlayerName.rectTransform.position = scrPos;
                pnlRoot.rectTransform.parent.position = scrPos;
                pnlRoot.gameObject.SetActive(scrPos.z >= 0.0f);
            }

            if (txtElapsedTime)
            {
                var t = mPlayer.ElapsedTimeFromSpawn;
                txtElapsedTime.text = string.Format("{0:D2}:{1:D2}.{2:D3}", ((int)(t / 60.0f)) % 60, ((int)t)%60, ((int)(t*100.0f)) % 100);
            }
        }
    }
}
