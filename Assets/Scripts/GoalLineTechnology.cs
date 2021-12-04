using UnityEngine;
using UnityEngine.UI;

public class GoalLineTechnology : MonoBehaviour
{
    public Text goalText;
    public Text goalCounterText;
    private int goalCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetGoalCounterText();
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
            goalCounter = goalCounter + 1;
            SetGoalCounterText();
            Invoke("ClearGoalText", 1f);
        }
    }

    void ClearGoalText()
    {
        goalText.text = "";
    }

    void SetGoalCounterText()
    {
        goalCounterText.text = "GOALS: " + goalCounter;
    }
}
