using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {
    public int PlayerID;
    GameController GC;
    ParticleSystem Particles;

    // Use this for initialization
    void Start () {
        GC = FindObjectOfType<GameController>();
        Particles = GetComponentInChildren<ParticleSystem>();
	}
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "ball")
        {
            // Tell the game we hit the goal
            GC.BallHitEdge(this.gameObject, PlayerID);
            // Fire off the pretty particles
            Particles.transform.position = new Vector2(Particles.transform.position.x, other.transform.position.y);
            Particles.Play();
        }
    }
}
