using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunBattle
{
    public class MenuBase : MonoBehaviourPunCallbacks
    {
        // Start is called before the first frame update
        public MenuManager menuManager { set; get; }


        public virtual void ActivateMenu()
        {
            gameObject.SetActive(true);
        }
        public virtual void DeactivateMenu()
        {
            gameObject.SetActive(false);
        }
    }

}