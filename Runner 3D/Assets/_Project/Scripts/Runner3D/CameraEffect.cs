using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using JoaoSantos.Runner3D.WorldElement;


namespace JoaoSantos.Runner3D
{
    [Serializable]
    public struct ShakeEffect
    {
        public float intensity;
        public double time;

        public long StartTicks { get; set; }
        public bool Actived { get; set; }
    }

    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraEffect : MonoBehaviour
    {
        [SerializeField]
        private CinemachineVirtualCamera virtualCamera;

        private CinemachineBasicMultiChannelPerlin multiChannerPerlin;

        [Header("Properties")]

        [SerializeField]
        private ShakeEffect shakeEffect;

        private long Tick => DateTime.Now.Ticks;


        #region Unity Methods

        private void Awake()
        {
            this.multiChannerPerlin = this.virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            PickupCollectableSystem.PickupCollectable += ShakeCamera;
        }

        private void OnDestroy()
        {
            PickupCollectableSystem.PickupCollectable -= ShakeCamera;
        }

        private void Update()
        {
            var elapsedTime = Tick;

            CheckToStopCameraShake(elapsedTime);
        }

        #endregion

        #region Private Methods

        private void ShakeCamera()
        {
            this.multiChannerPerlin.m_AmplitudeGain = shakeEffect.intensity;

            shakeEffect.StartTicks = Tick;
            shakeEffect.Actived = true;
        }

        private void CheckToStopCameraShake(long elapsedTime)
        {
            if (!shakeEffect.Actived) return;

            long elapsedTicks = elapsedTime - shakeEffect.StartTicks;
            TimeSpan elapsedSpan = new TimeSpan(elapsedTicks);            

            if (elapsedSpan.TotalSeconds < shakeEffect.time) return;            

            shakeEffect.Actived = false;
            shakeEffect.StartTicks = 0;
            multiChannerPerlin.m_AmplitudeGain = 0;
        }

        #endregion
    }
}
