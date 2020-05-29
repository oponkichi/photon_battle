using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PunBattle
{
    public class NetworkError : MenuBase
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void OnResetButtonPressed()
        {
            menuManager.ActivateMenu("Startup");
        }
    }
}