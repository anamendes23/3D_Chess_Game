using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    private SelectorController sc;
    private GameObject turnIndicator;
    private GameObject viewText;
    private GameObject gameOver;
    private GameObject checkMate;
    private Image image;
    private Text viewTxt;

    // Start is called before the first frame update
    void Start()
    {
        sc = FindObjectOfType<SelectorController>();
        turnIndicator = GameObject.Find("TeamColor");
        viewText = GameObject.Find("ViewTxt");
        viewTxt = viewText.GetComponent<Text>();
        image = turnIndicator.GetComponent<Image>();
        gameOver = GameObject.Find("GameOver");
        checkMate = GameObject.Find("CheckMate");
        sc.turnChange += TurnChange;
        sc.viewChange += ViewChange;
        sc.gc.gameOver += GameOver;
        gameOver.SetActive(false);
        checkMate.SetActive(false);
    }

    void TurnChange(int turn)
    {
        if (turn == -1)
        {
            image.color = Color.blue;
        }
        else
        {
            image.color = Color.red;
        }
    }

    void ViewChange(int view)
    {
        string[] viewLabel = { "View 1", "View 2", "View 3" };
        viewTxt.text = viewLabel[view];
    }

    void GameOver(string message)
    {
        Time.timeScale = 0;
        Text text = checkMate.GetComponent<Text>();
        text.text = message;
        gameOver.SetActive(true);
        checkMate.SetActive(true);

        WaitForSecondsRealtime wait = new WaitForSecondsRealtime(5);
        StartCoroutine(TimeDelay(wait));
    }

    private IEnumerator TimeDelay(WaitForSecondsRealtime wait)
    {
        wait.Reset();
        yield return wait;
        //Debug.Log("Waited for: " + wait.waitTime);
        SceneManager.LoadScene("MainMenu");
    }
}
