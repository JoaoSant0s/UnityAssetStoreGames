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

        private Track[] startTracks;

        #endregion

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
            }
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

        private void OnSpawnNextTrack()
        {
            if (!LevelSystem.Instance.HasAsset()) return;
            var asset =  LevelSystem.Instance.CurrentAsset;

            var track = PoolSelector.Instance.CreateOrSpawn<Track>(asset, this.trackArea);

            track.transform.localPosition = new Vector3(0, 0, this.nextTrackPosition);

            this.nextTrackPosition += track.Size;
            LevelSystem.Instance.UpdateToNextLevel();
            SetTrackTrigger(track);
        }

        private void OnHideTrack(Track track)
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