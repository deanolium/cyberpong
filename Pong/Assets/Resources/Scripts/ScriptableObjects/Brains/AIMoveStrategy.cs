using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStrategy
{
    [CreateAssetMenu(menuName ="Move Strategy/Basic AI")]
    public class AIMoveStrategy : MoveStrategy
    {
        // Just follows the ball -- pretty dumb

        [MinMaxRange(-10, 10)]
        public RangedFloat paddleHeightRange;

        public override void Initialize(PaddleThinker thinker)
        {
            // Define the height the AI will regard the paddle -- including some randomness to basically change skill...
            // Smaller numbers will aim more for the center of the paddle 
            // Larger numbers will think the paddle is bigger than it is - so expect dumb play
            float newHeight = 10f + Random.Range(paddleHeightRange.minValue, paddleHeightRange.maxValue);
            thinker.Remember("paddleHeight", newHeight);

            Debug.Log("Setting Paddle Height to " + thinker.Remember<float>("paddleHeight"));
        }

        public override float GetMoveDirection(PaddleThinker thinker, GameObject paddle, GameObject ball)
        {
            float paddleHeight = thinker.Remember<float>("paddleHeight");
            // Only produce a result if we're playing
            if (GameState.IsState(GameState.State.Playing) || GameState.IsState(GameState.State.TitleScreen))
            {
                if (ball.transform.position.y < paddle.transform.position.y - paddleHeight)
                {
                    return -1;
                }

                if (ball.transform.position.y > paddle.transform.position.y + paddleHeight)
                {
                    return 1;
                }
                
                return 0;
            }
            else
            {
                return 0; // don't do nuffin'
            }
        }
    }

}