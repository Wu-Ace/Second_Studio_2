using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Manager
{

    public class CharacterController : MonoBehaviour
    {
        static CharacterController _instance;
        private void Awake()
        {
            if (_instance !=  null)
            {
                Destroy(this);
            }
            _instance = this;
        }


    }
}