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

namespace JoaoSantos.Runner3D.WorldElement
{
    [GenerateAuthoringComponent]
    public struct PlayerMovementComponentData : IComponentData
    {
        [Header("Values")]
        public float speed;
        public float maxVelocity;
        public float movementDistance;
        public float2 movementLimits;
        private int movementIndex;

        #region Propertu Method

        public bool CanMoveLeft
        {
            get { return this.movementIndex > this.movementLimits.x; }
        }

        public bool CanMoveRight
        {
            get { return this.movementIndex <= this.movementLimits.y; }
        }

        public int MovementIndex
        {
            get { return movementIndex; }
            set { movementIndex = value; }
        }

        #endregion

    }
    [Serializable]
    public struct TransversalMovementData
    {
        public float movementDistance;
        public float2 movementLimits;
    }
}