using UnityEngine;
using System.Collections;

public class FixObjects : MonoBehaviour {

    private Rigidbody mBody;

    void Awake()
    {
        mBody = this.GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Get the velocity
        Vector3 horizontalMove = mBody.velocity;
        // Don't use the vertical velocity
        horizontalMove.y = 0;
        // Calculate the approximate distance that will be traversed
        float distance = horizontalMove.magnitude * Time.fixedDeltaTime;
        // Normalize horizontalMove since it should be used to indicate direction
        horizontalMove.Normalize();
        RaycastHit hit;

        // Check if the body's current velocity will result in a collision
        if (mBody.SweepTest(horizontalMove, out hit, distance))
        {
            // If so, stop the movement
            mBody.velocity = new Vector3(0, mBody.velocity.y, 0);
        }
    }
}
