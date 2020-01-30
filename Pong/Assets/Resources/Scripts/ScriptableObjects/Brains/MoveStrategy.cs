using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStrategy
{
    public abstract class MoveStrategy: ScriptableObject
    {
        public abstract float GetMoveDirection(PaddleThinker thinker, GameObject paddle, GameObject ball);

        public virtual void Initialize(PaddleThinker thinker)
        {
        }

        public virtual void drawGizmos(PaddleThinker thinker)
        {

        }
    }
}