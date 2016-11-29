using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UiManager : MonoBehaviour {

    public static UiManager Instance { get; private set; }

    [SerializeField]
    private Text pointText;

    [SerializeField]
    private Text timeText;

    [SerializeField]
    private Text pressAnyKeyText;
    [SerializeField]
    private float pressAnyKeyTextSpeed = 1.0f;

    [SerializeField]
    private GameObject mainMenuRoot;

    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button exitButton;

    private float seconds = 0.0f;

    private Color anyKeyColor = Color.white;

    private int minutesInt = 0;
    private int secondsInt = 0;

    private float time;

    public void ShowAnyKeyText(bool val)
    {
        this.pressAnyKeyText.gameObject.SetActive(val);
    }

    void Awake()
    {
        Debug.Assert(!Instance);

        Instance = this;

        playButton.onClick.AddListener(this.OnPlayButton);
        exitButton.onClick.AddListener(this.OnEndButton);

        UpdatePoints(GameManager.Instance.GetNumBananas());

        ShowInGame(false);
    }


    public void UpdatePoints(int num)
    {
        pointText.text = " Bananas left: " + num; 
    }

    public void UpdateTime()
    {
        timeText.text = "Time: " + (minutesInt < 10 ? ("0" + minutesInt.ToString()) : minutesInt.ToString())
                                + ":" + (secondsInt < 10 ? ("0" + secondsInt.ToString()) : secondsInt.ToString()) + " "; 
    }

    void Update()
    {

        if (GameManager.Instance.gameIsRunning)
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

        time += Time.deltaTime * pressAnyKeyTextSpeed;

        if(pressAnyKeyText.gameObject.activeSelf)
        {
            anyKeyColor.a = Mathf.Sin(time) * 0.5f + 0.5f;
            pressAnyKeyText.color = anyKeyColor;
        }
    }

    void OnPlayButton()
    {
        GameManager.Instance.StartGame();
    }

    void OnEndButton()
    {
        Application.Quit();
    }


    internal void ShowInGame(bool val)
    {
        if(val)
        {
            Reset();
        }

        this.pointText.gameObject.SetActive(val);

        this.timeText.gameObject.SetActive(val);

        mainMenuRoot.gameObject.SetActive(!val);
    }

    public void Reset()
    {
        this.minutesInt = 0;
        this.seconds = 0.0f;
        this.secondsInt = 0;

        UpdateTime();

        UpdatePoints(GameManager.Instance.GetNumBananas());
    }
}
