using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JoaoSantos.General
{
    public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        protected static T instance = null;
        protected virtual void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = GetComponent<T>();
            }
        }

        public static T Instance
        {
            get { return instance; }
        }
    }
}