using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleThinker : MonoBehaviour
{
    public MoveStrategy.MoveStrategy brain;
    MoveStrategy.MoveStrategy previousBrain; // used so we only initialize a brain once

    private Dictionary<string, object> memory;

    public T Remember<T>(string key)
    {
        object result;
        if (!memory.TryGetValue(key, out result))
            return default(T);
        return (T)result;
    }

    public void Remember<T>(string key, T value)
    {
        memory[key] = value;
    }

    public float getMoveDirection(GameObject paddle, GameObject ball)
    {
        if(!brain)
        {
            Debug.Log("No brain found");
            return 0;
        }

        if (memory == null)
        {
            Debug.Log("Creating Memory");
            memory = new Dictionary<string, object>();
        }

        if (brain != previousBrain)
        {
            Debug.Log("Implanting new Brain");
            memory.Clear();
            brain.Initialize(this);
            previousBrain = brain;
        }

        return brain.GetMoveDirection(this, paddle, ball);
    }

    private void OnDrawGizmos()
    {
        if (!brain || memory == null)
        {
            return;
        }

        Gizmos.color = Color.cyan;
        brain.drawGizmos(this);
    }
}
