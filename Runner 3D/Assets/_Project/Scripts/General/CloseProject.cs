using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JoaoSantos.General
{

    public class CloseProject : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }
}