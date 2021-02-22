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
        public float speed;
        public float maxVelocity;
        
        public TransversalMovementData transversalMovement;
        private int movementIndex;

        public bool CanMoveLeft
        {
            get { return this.movementIndex > this.transversalMovement.movementLimits.x; }
        }

        public bool CanMoveRight
        {
            get { return this.movementIndex <= this.transversalMovement.movementLimits.y; }
        }

        public int MovementIndex
        {
            get { return movementIndex; }
            set { movementIndex = value; }
        }

    }
    [Serializable]
    public struct TransversalMovementData
    {
        public float movementDistance;
        public float2 movementLimits;
    }
}