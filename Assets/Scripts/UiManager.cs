using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UiManager;

public class UiManager : MonoBehaviour
{
    [SerializeField] GameObject menuPannel;
    [SerializeField] GameObject gamePlayPannel;
    [SerializeField] GameObject gameOverPannel;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text matchedText;
    [SerializeField] private Text turnText;
    [SerializeField] private Dropdown selectDifficultyDropdown;

    public enum Difficulty
    {
        easy,
        medium,
        hard
    }

    public Difficulty selectDifficulty = Difficulty.medium;

    private void Awake()
    {
        menuPannel.SetActive(true);
        gamePlayPannel.SetActive(false);
        gameOverPannel.SetActive(false);
        List<string> options = new List<string>(Enum.GetNames(typeof(Difficulty)));
        selectDifficultyDropdown.ClearOptions();
        selectDifficultyDropdown.AddOptions(options);
        selectDifficultyDropdown.value = (int)selectDifficulty;

        selectDifficultyDropdown.onValueChanged.AddListener(OnDifficultyChanged);
    }

    void OnDifficultyChanged(int index)
    {
        selectDifficulty = (Difficulty)index;
        switch (selectDifficulty)
        {
            case Difficulty.easy:
                GameManager.Instance.turnsLeft = 15;
                GameManager.Instance.playbleRowNum = 2;
                break;
            case Difficulty.medium:
                GameManager.Instance.turnsLeft = 12;
                GameManager.Instance.playbleRowNum = 3;
                break;
            case Difficulty.hard:
                GameManager.Instance.turnsLeft = 20;
                GameManager.Instance.playbleRowNum = 5;
                break;
        }
    }

    private void OnEnable()
    {
        Invoke(nameof(BindEvents), 0.5f);

    }

    void BindEvents()
    {
        GameManager.Instance.OnScoreChanged += UpdateScore;
        GameManager.Instance.OnTurnCountChanged += UpdateTurnCount;
        GameManager.Instance.OnMatchedCardCountChanged += UpdateMatchedCardCount;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnScoreChanged -= UpdateScore;
        GameManager.Instance.OnTurnCountChanged -= UpdateTurnCount;
        GameManager.Instance.OnMatchedCardCountChanged -= UpdateMatchedCardCount;
    }

    public void PlayGame()
    {
        menuPannel.SetActive(false);
        gamePlayPannel.SetActive(true);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    void GameOver()
    {
        gameOverPannel.SetActive(true);
    }

    void UpdateScore(int value)
    {
        scoreText.text = value.ToString();
        if(value == GameManager.Instance.maxScore)
        {
            GameOver();
            GameManager.Instance.audioSource.clip = GameManager.Instance.gameWon;
            GameManager.Instance.audioSource.Play();
        }
    }
    void UpdateMatchedCardCount(int value)
    {
        matchedText.text = value.ToString();
    }
    void UpdateTurnCount(int value)
    {
        turnText.text = value.ToString();
        if(value <= 0)
        {
            GameOver();
            GameManager.Instance.audioSource.clip = GameManager.Instance.gameLoss;
            GameManager.Instance.audioSource.Play();
        }
    }

}
