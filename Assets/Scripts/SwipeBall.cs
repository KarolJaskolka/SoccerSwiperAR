using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SwipeBall : MonoBehaviour
{
    private Vector2 startTouch, swipeDelta;
    private bool isDraging = false;
    private bool tap = false;
    private float deadzone = 200f; // minimal swipe length to consider user input as swipe

    // public variables needed to control ball behaviour
    public bool swiped = false; // true when swipe surpassed deadzone
    public float swipeX = 0f; // if value > 0 swiped right else swiped left
    public float swipeY = 0f; // if value > 0 swiped up else swiped down
    public float swipeDistance = 0f; // length of swipe

    [SerializeField]
    ARRaycastManager m_RaycastManager;
    List<ARRaycastHit> m_Hits = new List<ARRaycastHit>();

    [SerializeField]
    GameObject spawnablePrefab;

    Camera arCam;
    GameObject spawnedObject;

    Vector2 startPos, endPos, direction;
    float touchTimeStart, touchTimeEnd, time;

    float forceXY = 1f;

    float forceZ = 50f;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        spawnedObject = null;
        arCam = GameObject.Find("Ar Camera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        tap = false;

        if (Input.touches.Length != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isDraging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDraging = false;
                Reset();
            }
        }

        swipeDelta = Vector2.zero;

        if (isDraging)
        {
            if (Input.touches.Length > 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            }
        }

        if (swipeDelta.magnitude > deadzone)
        {
            swiped = true;
            swipeX = swipeDelta.x;
            swipeY = swipeDelta.y;
            swipeDistance = swipeDelta.magnitude;

            Reset();
        }

        RaycastHit hit;
        Ray ray = arCam.ScreenPointToRay(Input.GetTouch(0).position);
        if(m_RaycastManager.Raycast(Input.GetTouch(0).position, m_Hits))
        {
            if(Physics.Raycast(ray,out hit))
            {
                if(hit.collider.gameObject.tag == "Spawnable")
                {
                    spawnedObject = hit.collider.gameObject;
                    
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        touchTimeStart = Time.time;
                        startPos = Input.GetTouch(0).position;
                    }

                    if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        touchTimeEnd = Time.time;

                        time = touchTimeEnd - touchTimeStart;

                        endPos = Input.GetTouch(0).position;

                        direction = startPos - endPos;

                        rb.isKinematic = false;

                        rb.AddForce(-direction.x * forceXY, -direction.y * forceXY, forceZ / time);

                        Destroy(spawnedObject, 3f);
                    }
                }
            }
        }
    }

    private void Reset()
    {
        startTouch = Vector2.zero;
        swipeDelta = Vector2.zero;
        isDraging = false;
    }
}
