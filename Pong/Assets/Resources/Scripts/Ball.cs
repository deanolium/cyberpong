using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    [SerializeField]
    public Vector2 SpeedVec;

    [SerializeField]
    private float StartingSpeed;
    public float Speed;

    [SerializeField]
    private float AngularVelocity;

    [SerializeField]
    private float BounceJitter;

    TrailRenderer Trail;

	// Use this for initialization
	void Start () {
        Trail = GetComponentInChildren<TrailRenderer>();
        Reset();
    }

    public void Reset()
    {
        transform.position = new Vector2(0, 0);
        SpeedVec = new Vector2(Random.value > 0.5 ? StartingSpeed : -StartingSpeed, 0);
        Speed = SpeedVec.magnitude;
        AngularVelocity = 0f;

        transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void FixedUpdate () {
        Move();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ContactPoint2D[] collisionPoints = new ContactPoint2D[1];
        // let's alter the speed vec and see what that does
        if (collision.GetContacts(collisionPoints) > 0)
        {
            foreach (ContactPoint2D collisionPoint in collisionPoints)
            {
                switch (collisionPoint.collider.tag)
                {
                    case "paddle":
                        //HandlePaddleBounce(collisionPoint);
                        //DoPossibleTrail();
                        break;
                    case "border":
                        HandleBorderBounce(collisionPoint);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void HandlePaddleBounce(ContactPoint2D paddleCollision)
    {
        SpeedVec = Vector2.Reflect(SpeedVec, paddleCollision.normal);
        // Add some bounce jitter -- need to ensure we don't go backwards!
        SpeedVec = Quaternion.Euler(0, 0, Random.Range(BounceJitter, -BounceJitter)) * SpeedVec;

        // Add the momentum of the paddle
        float scaledPaddleMomentum = paddleCollision.collider.GetComponent<Paddle>().Momentum * 60f;

        // We want to preserve the velocity (mainly)
        float previousMagnitude = SpeedVec.magnitude;
        SpeedVec.y += scaledPaddleMomentum;
        SpeedVec = SpeedVec.normalized * previousMagnitude;

        // Speed up the ball over time
        SpeedVec *= 1.01f;
        Speed = SpeedVec.magnitude;

        // Add some angular velocity
        if (scaledPaddleMomentum > 0)
            AngularVelocity += Random.Range(0f, 2 * scaledPaddleMomentum);
        else 
            AngularVelocity += Random.Range(2 * scaledPaddleMomentum, 0f);
    }

    private void HandleBorderBounce(ContactPoint2D borderCollision)
    {
        SpeedVec = Vector2.Reflect(SpeedVec, borderCollision.normal);
    }

    private void HandleRayPaddleBounce(RaycastHit2D hit)
    {
        // figure out the new vector
        Vector2 newSpeedVec = Vector2.Reflect(SpeedVec, hit.normal);
        newSpeedVec = Quaternion.Euler(0, 0, Random.Range(BounceJitter, -BounceJitter)) * newSpeedVec;

        // Add the momentum of the paddle
        float scaledPaddleMomentum = hit.collider.GetComponent<Paddle>().Momentum * 60f;

        // We want to preserve the velocity (mainly)
        float previousMagnitude = newSpeedVec.magnitude;
        newSpeedVec.y += scaledPaddleMomentum;
        newSpeedVec = newSpeedVec.normalized * previousMagnitude;

        // Speed up the ball over time
        newSpeedVec *= 1.01f;
        if (newSpeedVec.magnitude >= 1000f)
        {
            newSpeedVec = newSpeedVec.normalized * 1000f;
        }

        Speed = newSpeedVec.magnitude;

        // Add some angular velocity
        if (scaledPaddleMomentum > 0)
            AngularVelocity += Random.Range(0f, 2 * scaledPaddleMomentum);
        else
            AngularVelocity += Random.Range(2 * scaledPaddleMomentum, 0f);

        // now figure out where the new position should be
        float fullIntendedDistance = SpeedVec.magnitude * Time.deltaTime;
        float distanceBeforeHit = (new Vector2(transform.position.x, transform.position.y) - hit.point).magnitude;
        float distanceRemaining = fullIntendedDistance - distanceBeforeHit;
        float distanceRemainingMult = distanceRemaining / fullIntendedDistance;


        Vector3 newPosition = new Vector3(hit.point.x, hit.point.y, transform.position.z) + (Vector3)newSpeedVec * Time.deltaTime * distanceRemainingMult;

        SpeedVec = newSpeedVec;
        transform.position = newPosition;
    }

    void Move()
    {
        // check to see if it hits and paddle (done this way so ball doesn't warp through the paddle)
        float distanceThisFrame = SpeedVec.magnitude * Time.deltaTime;
        
        RaycastHit2D hit = Physics2D.Raycast(transform.position, SpeedVec, distanceThisFrame, 1 << LayerMask.NameToLayer("Paddles"));

        if (hit.collider != null && hit.collider.tag == "paddle")
        {
            // we have to handle the bounce
            HandleRayPaddleBounce(hit);
            DoPossibleTrail();
        }
        else
        {
            transform.position += (Vector3)SpeedVec * Time.deltaTime;
        }
        transform.Rotate(0, 0, AngularVelocity * Time.deltaTime, Space.Self);

        Debug.DrawLine(transform.position, transform.position + (Vector3)SpeedVec * Time.deltaTime, Color.red);
    }

    private void DoPossibleTrail()
    {
        if (Speed > StartingSpeed * 1.05f)
            StartCoroutine(BlipTrail());
    }

    // Turns the trail on for a small bit of time, before turning it off again, so it fades away
    public IEnumerator BlipTrail()
    {
        Trail.emitting = true;
        float trailTime = (Speed - StartingSpeed) / StartingSpeed;
        yield return new WaitForSeconds(trailTime);
        Trail.emitting = false;
    }
}
