using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using JoaoSantos.General;
using JoaoSantos.Runner3D.Common;

namespace JoaoSantos.Runner3D.WorldElement
{
    [Serializable]
    public class DestroyPlaneEvent : UnityEvent<Plane> { }

    [Serializable]
    public class Plane : MonoBehaviour
    {
        [SerializeField]
        private float size;

        public UnityEvent spawnNextPlaneEvent;
        public DestroyPlaneEvent destroyPlaneEvent;

        public float Size
        {
            get { return this.size; }
        }

        #region Unity Methods

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != Tags.PLAYERTAG) return;

            this.spawnNextPlaneEvent.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag != Tags.PLAYERTAG) return;

            this.destroyPlaneEvent.Invoke(this);
        }

        #endregion    

        #region Public Methods

        public void RemoveEvents()
        {
            this.spawnNextPlaneEvent.RemoveAllListeners();
            this.destroyPlaneEvent.RemoveAllListeners();
        }

        #endregion
    }
}
