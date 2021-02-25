using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using JoaoSantos.General;
using JoaoSantos.General.Asset;
using System;

namespace JoaoSantos.Runner3D.WorldElement
{
    public class TrackGenerator : SingletonBehaviour<TrackGenerator>
    {
        [Header("Values")]
        [SerializeField]
        private float hidePreviouslyTrackDelay;

        [SerializeField]
        private Transform trackArea;

        #region Local Variable        

        private float nextTrackPosition;
        private float nextYPosition;

        private Track[] startTracks;

        #endregion

        private const float yPosition = -0.1f;

        #region  Unity Methods

        protected override void Awake()
        {
            base.Awake();
            InitVariables();
        }

        private void Start()
        {
            CalculateNextTrackPosition();
            SetLastTrigger();
        }

        #endregion

        #region Private Methods

        private void InitVariables()
        {
            this.startTracks = trackArea.GetComponentsInChildren<Track>();
        }

        private void CalculateNextTrackPosition()
        {
            for (int i = 0; i < this.startTracks.Length; i++)
            {
                this.nextTrackPosition += this.startTracks[i].Size;
                this.nextYPosition += yPosition;
            }
            Debugs.Log(this.nextTrackPosition, this.nextYPosition);
        }

        private void SetLastTrigger()
        {
            LevelSystem.Instance.SetValues();
            for (int i = 0; i < this.startTracks.Length; i++)
            {
                SetTrackTrigger(this.startTracks[i]);
            }
        }

        private void SetTrackTrigger(Track track)
        {
            track.spawnNextTrackEvent.AddListener(OnSpawnNextTrack);
            track.hideTrackEvent.AddListener(OnHideTrack);
        }

        public void OnSpawnNextTrack()
        {
            if (!LevelSystem.Instance.HasAsset()) return;
            var asset = LevelSystem.Instance.CurrentAsset;

            //TODO

            var track = PoolSelector.Instance.CreateOrSpawn<Track>(asset, this.trackArea);

            track.transform.localPosition = new Vector3(0, this.nextYPosition, this.nextTrackPosition);

            this.nextTrackPosition += track.Size;
            this.nextYPosition += yPosition;

            LevelSystem.Instance.UpdateToNextLevel();
            SetTrackTrigger(track);
        }

        public void OnHideTrack(Track track)
        {
            StartCoroutine(HideTrackRoutine(track));
        }

        private IEnumerator HideTrackRoutine(Track track)
        {
            yield return new WaitForSeconds(this.hidePreviouslyTrackDelay);

            if (track.IsToDestroy)
            {
                GameObject.Destroy(track.gameObject);
            }
            else
            {
                track.Disable();
            }
        }

        #endregion
    }
}