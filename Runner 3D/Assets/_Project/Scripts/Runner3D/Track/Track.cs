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
        [Header("Components")]
        [SerializeField]
        private SpawnCollectables spawnCollectables;

        [Header("Values")]
        
        [SerializeField]
        private float size;

        [SerializeField]
        private bool isToDestroy;

        public UnityEvent spawnNextTrackEvent;
        public HideTrackEvent hideTrackEvent;

        public static int trackCounter;

        #region Property Methods

        public float Size
        {
            get { return this.size; }
        }

        public bool IsToDestroy
        {
            get { return this.isToDestroy; }
        }

        #endregion

        #region Unity Methods

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag != Tags.PLAYERTAG) return;

            this.spawnNextTrackEvent.Invoke();

            trackCounter++;
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag != Tags.PLAYERTAG) return;

            this.hideTrackEvent.Invoke(this);
        }

        #endregion        

        #region Private Methods

        public override void Enable()
        {
            base.Enable();
            SpawnCollectablesManager.Instance.Spawn(this.spawnCollectables);
        }

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
