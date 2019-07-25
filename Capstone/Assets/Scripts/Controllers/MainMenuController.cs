using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    [SerializeField]
    private Button musicButton;
    [SerializeField]
    private Sprite[] musicIcons;

    // Start is called before the first frame update
    void Start()
    {

    }

    void CheckToPlayMusic()
    {
        if (GamePreferences.GetMusicState() == 1)
        {
            MusicController.instance.PlayMusic(true);
            musicButton.image.sprite = musicIcons[1];
        }
        else
        {
            MusicController.instance.PlayMusic(false);
            musicButton.image.sprite = musicIcons[0];
        }
    }

    public void StartGame()
    {
        //GameManager.instance.gameStartedFromMenu = true;
        //GameManager.instance.gameRestartedAfterPlayerDied = false;
        SceneManager.LoadScene("SceneKris");
        //SceneFader.instance.LoadLevel("GamePlay");
    }

    public void StartSingleGame()
    {
        //GameManager.instance.gameStartedFromMenu = true;
        //GameManager.instance.gameRestartedAfterPlayerDied = false;
        GameManager.toBeOrNotToBe = true;
        SceneManager.LoadScene("SceneKris");
        //SceneFader.instance.LoadLevel("GamePlay");
    }

    public void GoToScoreMenu()
    {
        SceneManager.LoadScene("ScoreMenu");
        //SceneFader.instance.LoadLevel("HighScore");
    }

    public void GoToOptionsMenu()
    {
        SceneManager.LoadScene("OptionsMenu");
        //SceneFader.instance.LoadLevel("OptionsMenu");
    }

    public void GoBackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MusicButton()
    {
        if (GamePreferences.GetMusicState() == 0)
        {
            GamePreferences.SetMusicState(1);
            CheckToPlayMusic();
        }
        else if (GamePreferences.GetMusicState() == 1)
        {
            GamePreferences.SetMusicState(0);
            CheckToPlayMusic();
        }
    }
}
