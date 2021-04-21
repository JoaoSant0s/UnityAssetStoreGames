using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

using JoaoSantos.General;

namespace JoaoSantos.Runner3D.WorldElement
{
    public class MoveCollectableSystem : SystemBase
    {
        protected override void OnUpdate()
        {
            var dt = Time.DeltaTime;

            Entities.ForEach((ref Translation translation, ref CollectableComponentData collectable) =>
            {
                var startY = collectable.StartPosition.y;

                var direction = collectable.invertDirection ? -1 : 1;

                var value = translation.Value.y;
                var incremental = dt * direction * collectable.moveSpeed;

                var result = value + incremental;

                if (result < collectable.moveYLimit.x + startY)
                {
                    collectable.invertDirection = false;
                    result = collectable.moveYLimit.x + startY;
                }
                else if (result > collectable.moveYLimit.y + startY)
                {
                    collectable.invertDirection = true;
                    result = collectable.moveYLimit.y + startY;
                }

                translation.Value.y = result;

            }).ScheduleParallel();
        }


    }
}