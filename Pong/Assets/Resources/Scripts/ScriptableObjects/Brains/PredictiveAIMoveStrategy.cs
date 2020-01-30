using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoveStrategy
{
    [CreateAssetMenu(menuName ="Move Strategy/Predictive AI")]
    public class PredictiveAIMoveStrategy : MoveStrategy
    {
        [Range(0f,45f)]
        public float possibleAngleError; // error in predicting the angle in degrees

        public int maxHits = 5;

        public string goalTag = "goal";
        public string wallTag = "wall";

        public override void Initialize(PaddleThinker thinker)
        {
            thinker.Remember("state", "calculating");

        }

        public override float GetMoveDirection(PaddleThinker thinker, GameObject paddle, GameObject ball)
        {

            float angleError;
            if (thinker.Remember<string>("state").Equals("calculating"))
            {
                angleError = Random.Range(-possibleAngleError, possibleAngleError);
                thinker.Remember("angleError", angleError);
                thinker.Remember("state", "waiting");
            } else
            {
                angleError = thinker.Remember<float>("angleError");
            }

            // bx + vx  makes px - bx larger 
            // px - bx < px - bx - vx
            // px < px - vx
            // Only predict if the ball is coming towards us
            float px = paddle.transform.position.x;
            float bx = ball.transform.position.x;
            float vx = ball.GetComponent<Ball>().SpeedVec.x;

            if (((px > bx) && vx < 0) || ((px < bx) && vx > 0))
            {
                if (thinker.Remember<string>("state").Equals("running"))
                {
                    thinker.Remember("state", "calculating");
                }
                
                if (paddle.transform.position.y > 1)
                {
                    return -1;
                }
                if (paddle.transform.position.y < -1)
                {
                    return 1;
                }
                return 0;
                
            }

            if (thinker.Remember<string>("state").Equals("waiting"))
            {
                thinker.Remember("state", "running");
            }

            thinker.Remember("gizmo_lines", new List<Vector3[]>());

            Vector2 targetPos = Predict(ball.transform.position, ball.GetComponent<Ball>().SpeedVec, 0, angleError, thinker);

            if (targetPos.y > paddle.transform.position.y + 1)
            {
                return 1;
            }

            if (targetPos.y < paddle.transform.position.y - 1)
            {
                return -1;
            }

            return 0;
        }

        private Vector2 Predict(Vector2 position, Vector2 velocity, int numHits, float angleError, PaddleThinker thinker)
        {
            if (numHits == maxHits)
            {
                Debug.Log("Hit Limit");
                return position;
            }

            Vector3[] correct_gizmo_line = new Vector3[] { position, position + velocity };
            List<Vector3[]> currentGizmoLines = thinker.Remember<List<Vector3[]>>("gizmo_lines");
            currentGizmoLines.Add(correct_gizmo_line);

            if (numHits > 0)
            {
                velocity = Quaternion.Euler(0, 0, angleError) * velocity;
                Vector3[] bad_gizmo_line = new Vector3[] { position, position + velocity };
                currentGizmoLines.Add(bad_gizmo_line);
            }


            thinker.Remember("gizmo_lines", currentGizmoLines);


            RaycastHit2D[] hits = Physics2D.RaycastAll(position + (velocity.normalized * 0.1f), velocity.normalized);
            RaycastHit2D hit;

            if (hits.Length == 0)
            {
                return position;
            }

            foreach(RaycastHit2D h in hits)
            {
                string tag = h.collider.tag;

                if (tag.Equals(goalTag))
                {
                    // What we are looking for!
                    position = h.point;
                    return position;
                }

                if (tag.Equals(wallTag))
                {
                    hit = h;
                    position = hit.point;
                    Vector2 u = hit.normal;

                    u *= Vector2.Dot(velocity, hit.normal);
                    Vector2 w = velocity - u; 
                    velocity = w - u;
                    break;
                }
            }

            return Predict(position, velocity, numHits + 1, angleError, thinker);
        }

        public override void drawGizmos(PaddleThinker thinker)
        {
            if (thinker == null)
            {
                return;
            }

            List<Vector3[]> gizmoLines = thinker.Remember<List<Vector3[]>>("gizmo_lines");

            if (gizmoLines == null)
            {
                return;
            }

            foreach (Vector3[] linePoints in gizmoLines)
            {
                Gizmos.DrawLine(linePoints[0], linePoints[1]);
            }
        }
    }


}
