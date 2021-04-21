using UnityEngine;

using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Physics;

namespace JoaoSantos.General
{
    public static class RotationExtensions
    {
        public static void FreezeRotation(ref this Rotation rotation)
        {
            rotation.Value = Quaternion.Euler(float3.zero);
        }        

        public static void FreezeRotation(ref this Rotation rotation, float xValue, float yValue, float zValue)
        {
            float3 value = new float3(xValue, yValue, zValue);

            rotation.Value = Quaternion.Euler(value);
        }

        public static void FreezeRotation(ref this Rotation rotation, Quaternion value)
        {
            rotation.Value = value;
        }
    }
}