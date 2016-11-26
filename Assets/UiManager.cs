using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    public static UiManager Instance { get; private set; }

    [SerializeField]
    private Text pointText;

    [SerializeField]
    private Text timeText;

    private float seconds = 0.0f;

    private int minutesInt = 0;
    private int secondsInt = 0;

    void Awake()
    {
        Debug.Assert(!Instance);

        Instance = this;

        UpdatePoints(0);
    }


    public void UpdatePoints(int num)
    {
        pointText.text = " Points: " + num; 
    }

    public void UpdateTime()
    {
        timeText.text = "Time: " + (minutesInt < 10 ? ("0" + minutesInt.ToString()) : minutesInt.ToString())
                                + ":" + (secondsInt < 10 ? ("0" + secondsInt.ToString()) : secondsInt.ToString()) + " "; 
    }

    void Update()
    {
        seconds += Time.deltaTime;

        if (seconds >= 1.0f)
        {
            seconds = 0.0f;

            secondsInt++;

            UpdateTime();

            if (secondsInt == 60)
            {
                secondsInt = 0;
                minutesInt++;
            }
        }
    }
}
