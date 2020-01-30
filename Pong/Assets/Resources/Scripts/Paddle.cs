using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoveStrategy;

public class Paddle : MonoBehaviour {
    [SerializeField]
    float PaddleSpeed;

    [SerializeField]
    float TopBound;

    [SerializeField]
    float BottomBound;

    public int PlayerID;

    public float Momentum; // Store previous input so we can deal with momentum
    BoxCollider2D CollisionBox;

    PaddleThinker brain;
    // public MoveStrategy.MoveStrategy MoveStrat;

	// Use this for initialization
	void Start () {
        CollisionBox = GetComponent<BoxCollider2D>();

        brain = GetComponent<PaddleThinker>();
	}

    public void Reset()
    {
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
    }

    public void SetMoveStrategy(MoveStrategy.MoveStrategy newStrategy)
    {
        //MoveStrat = newStrategy;
    }
	
	// Update is called once per frame
	void Update () {
        HandleControls();
	}

    void HandleControls ()
    {
        if (brain)
        {
            Move(brain.getMoveDirection(gameObject, GameObject.Find("Ball")));
        }
        //Move(MoveStrat.GetMoveDirection(gameObject, GameObject.Find("Ball")));       
    }

    public void Move (float axisAmount)
    {
        // If we are moving, then add some momentum for the ball
        if (axisAmount != 0f)
            Momentum = Mathf.Clamp(Momentum + (axisAmount / 32), -1f, 1f);
        else
            Momentum = 0f;

        transform.position += Vector3.up * PaddleSpeed * Time.deltaTime * axisAmount;

        if (transform.position.y + (CollisionBox.size.y / 2) > TopBound)
        {
            Momentum = 0f;
            transform.position = new Vector3(transform.position.x, TopBound - (CollisionBox.size.y / 2), transform.position.z);
        }

        if (transform.position.y - (CollisionBox.size.y / 2) < BottomBound)
        {
            Momentum = 0f;
            transform.position = new Vector3(transform.position.x, BottomBound + (CollisionBox.size.y / 2), transform.position.z);
        }
    }
}
