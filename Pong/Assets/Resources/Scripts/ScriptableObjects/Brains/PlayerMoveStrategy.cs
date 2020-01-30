using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStrategy
{
    [CreateAssetMenu(menuName ="Move Strategy/Player Controlled")]
    public class PlayerMoveStrategy : MoveStrategy
    {
        public int PlayerID;

        public PlayerMoveStrategy(int playerID)
        {
            this.PlayerID = playerID;     
        }

        public override float GetMoveDirection(PaddleThinker thinker, GameObject paddle, GameObject ball)
        {
            // Only produce a result if we're playing
            if (GameState.IsState(GameState.State.Playing))
            {
                // determine input based on player
                if (PlayerID == 0)
                {
                    return Input.GetAxisRaw("Vertical");
                }
                else
                {
                    return 0 - Input.GetAxisRaw("Vertical");
                }
            } else
            {
                return 0; // don't do nuffin'
            }
        }
    }

}