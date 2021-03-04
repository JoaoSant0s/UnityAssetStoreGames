using UnityEngine;


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
    public struct CollectableComponentData : IComponentData
    {
        [Header("Rotation")]
        public float rotationSpeed;

        [Header("Move")]
        public float moveSpeed;
        public bool invertDirection;
        public float2 moveYLimit;

        private float3 startPosition;

        public float3 StartPosition
        {
            get { return this.startPosition; }
            set { this.startPosition = value; }
        }
    }
}
