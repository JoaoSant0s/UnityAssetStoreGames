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
    public class HideTrackEvent : UnityEvent<Track> { }

    [Serializable]
    public class Track : PoolBehaviour
    {
        [SerializeField]
        private float size;

        [SerializeField]
        private bool isToDestroy;

        public UnityEvent spawnNextTrackEvent;
        public HideTrackEvent hideTrackEvent;

        public static int trackCounter;

        public float Size
        {
            get { return this.size; }
        }

        public bool IsToDestroy
        {
            get { return this.isToDestroy; }
        }

        #region Unity Methods

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != Tags.PLAYERTAG) return;

            this.spawnNextTrackEvent.Invoke();

            Debugs.Log("OnTriggerEnter", trackCounter);
            trackCounter++;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag != Tags.PLAYERTAG) return;

            this.hideTrackEvent.Invoke(this);

            Debugs.Log("OnTriggerExit", trackCounter);
        }

        #endregion        

        #region Private Methods

        public override void Disable()
        {
            RemoveEvents();
            base.Disable();
        }

        private void RemoveEvents()
        {
            this.spawnNextTrackEvent.RemoveAllListeners();
            this.hideTrackEvent.RemoveAllListeners();
        }

        #endregion
    }
}
