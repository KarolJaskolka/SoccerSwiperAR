using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalLineTechnology : MonoBehaviour
{
    public Text goalText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            goalText.text = "GOAL";
            Invoke("ClearGoalText", 2f);
        }
    }

    void ClearGoalText()
    {
        goalText.text = "";
    }
}
