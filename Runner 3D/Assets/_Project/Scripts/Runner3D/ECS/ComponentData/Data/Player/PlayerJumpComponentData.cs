using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

using Unity.Entities;
using Unity.Collections;

using Unity.Transforms;
using Unity.Rendering;
using Unity.Mathematics;

using Unity.Jobs;
using Unity.Burst;

using Unity.Physics;
using Unity.Physics.Authoring;
using JoaoSantos.General;

namespace JoaoSantos.Runner3D.WorldElement
{
    [GenerateAuthoringComponent]
    public struct PlayerJumpComponentData : IComponentData
    {
        [Header("Values")]
        public float jumpForce;
        public float3 raycastJumpOffset;
        public float resetJumpDelay;

        private bool jumping;
        private float startJumpTime;

        #region Property Methods

        public bool Jumping
        {
            get { return jumping; }
            set { jumping = value; }
        }

        public float StartJumpTime
        {
            get { return startJumpTime; }
            set { startJumpTime = value; }
        }

        #endregion
    }
}