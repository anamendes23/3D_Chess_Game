using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GamePreferences
{
    public static string easyDifficulty = "EasyDifficulty";
    public static string mediumDifficulty = "MediumDifficulty";
    public static string hardDifficulty = "HardDifficulty";

    public static string easyDifficultyHighScore = "EasyDifficultyHighScore";
    public static string mediumDifficultyHighScore = "MediumDifficultyHighScore";
    public static string hardDifficultyHighScore = "HardDifficultyHighScore";

    public static string easyDifficultyCoinScore = "EasyDifficultyCoinScore";
    public static string mediumDifficultyCoinScore = "MediumDifficultyCoinScore";
    public static string hardDifficultyCoinScore = "HardDifficultyCoinScore";

    public static string isMusicOn = "IsMusicOn";

    //we are going to use integers to represent boolean variables
    // 0 - false, 1 - true

    public static void SetMusicState(int state)
    {
        PlayerPrefs.SetInt(GamePreferences.isMusicOn, state);
    }

    public static int GetMusicState()
    {
        return PlayerPrefs.GetInt(GamePreferences.isMusicOn);
    }

    public static void SetEasyDifficulty(int state)
    {
        PlayerPrefs.SetInt(GamePreferences.easyDifficulty, state);
    }

    public static int GetEasyDifficulty()
    {
        return PlayerPrefs.GetInt(GamePreferences.easyDifficulty);
    }

    public static void SetMediumDifficulty(int state)
    {
        PlayerPrefs.SetInt(GamePreferences.mediumDifficulty, state);
    }

    public static int GetMediumDifficulty()
    {
        return PlayerPrefs.GetInt(GamePreferences.mediumDifficulty);
    }

    public static void SetHardDifficulty(int state)
    {
        PlayerPrefs.SetInt(GamePreferences.hardDifficulty, state);
    }

    public static int GetHardDifficulty()
    {
        return PlayerPrefs.GetInt(GamePreferences.hardDifficulty);
    }

    public static void SetEasyDifficultyHighScore(int score)
    {
        PlayerPrefs.SetInt(GamePreferences.easyDifficultyHighScore, score);
    }

    public static int GetEasyDifficultyHighScore()
    {
        return PlayerPrefs.GetInt(GamePreferences.easyDifficultyHighScore);
    }

    public static void SetMediumDifficultyHighScore(int score)
    {
        PlayerPrefs.SetInt(GamePreferences.mediumDifficultyHighScore, score);
    }

    public static int GetMediumDifficultyHighScore()
    {
        return PlayerPrefs.GetInt(GamePreferences.mediumDifficultyHighScore);
    }

    public static void SetHardDifficultyHighScore(int score)
    {
        PlayerPrefs.SetInt(GamePreferences.hardDifficultyHighScore, score);
    }

    public static int GetHardDifficultyHighScore()
    {
        return PlayerPrefs.GetInt(GamePreferences.hardDifficultyHighScore);
    }

    public static void SetEasyDifficultyCoinScore(int score)
    {
        PlayerPrefs.SetInt(GamePreferences.easyDifficultyCoinScore, score);
    }

    public static int GetEasyDifficultyCoinScore()
    {
        return PlayerPrefs.GetInt(GamePreferences.easyDifficultyCoinScore);
    }

    public static void SetMediumDifficultyCoinScore(int score)
    {
        PlayerPrefs.SetInt(GamePreferences.mediumDifficultyCoinScore, score);
    }

    public static int GetMediumDifficultyCoinScore()
    {
        return PlayerPrefs.GetInt(GamePreferences.mediumDifficultyCoinScore);
    }

    public static void SetHardDifficultyCoinScore(int score)
    {
        PlayerPrefs.SetInt(GamePreferences.hardDifficultyCoinScore, score);
    }

    public static int GetHardDifficultyCoinScore()
    {
        return PlayerPrefs.GetInt(GamePreferences.hardDifficultyCoinScore);
    }
}
