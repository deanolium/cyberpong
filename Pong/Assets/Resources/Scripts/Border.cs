using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Border : MonoBehaviour {
    [SerializeField]
    ParticleSystem Particles;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.name == "Ball")
        {
            Particles.transform.position = new Vector2(collision.transform.position.x, Particles.transform.position.y);
            Particles.Play();
        }
    }
}
