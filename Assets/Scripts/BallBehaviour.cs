using UnityEngine;

public class BallBehaviour : MonoBehaviour
{
    public GameObject ballObject;
    public Rigidbody ballRigidbody;

    public SwipeBall swipeBall;
    public GameObject interaction;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (swipeBall.swiped)
        {
            // left / right
            // X value approximately between -500 to 500
            float xForce = swipeBall.swipeX / 250;
            // forward / backwards
            // Z value approximately lower than 750
            float zForce = swipeBall.swipeY / 250;
            // up
            // Distance value approximately lower than 900
            float yForce = swipeBall.swipeDistance / 250;

            // kick the ball
            ballRigidbody.AddForce(xForce, yForce, zForce, ForceMode.Impulse);

            // let controller know that action after swiped has been fired
            swipeBall.swiped = false;

            // Respawn ball 2 seconds after shot
            Invoke("RespawnBall", 2f);
        }
    }

    void RespawnBall()
    {
        // stop movement
        ballRigidbody.velocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;

        // reset position
        ballObject.transform.position = interaction.GetComponent<ARTapToPlaceObject>().ballPosition;
    }
}
