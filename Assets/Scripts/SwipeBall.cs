using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class SwipeBall : MonoBehaviour
{
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
}
