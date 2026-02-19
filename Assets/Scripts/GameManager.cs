using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int totalTurns;
    public int maxScore;
    [HideInInspector]
    public AudioSource audioSource;
    public AudioClip cardMatched;
    public AudioClip cardNotMatched;
    public AudioClip cardClickSound;
    public AudioClip gameWon;
    public AudioClip gameLoss;

    public event Action<int> OnScoreChanged;
    public event Action<int> OnTurnCountChanged;
    public event Action<int> OnMatchedCardCountChanged;
    public event Action<int> OnplaybleRowNumChanged;

    [HideInInspector] public Card card1;
    [HideInInspector] public Button button1;

    int _score;
    int _turnLeft;
    int _matchFound;
    int _playbleRowNum;

    public int playbleRowNum
    {
        get
        {
            return _playbleRowNum;
        }
        set
        {
            if (_playbleRowNum == value) return;

            _playbleRowNum = value;
            OnplaybleRowNumChanged?.Invoke(_playbleRowNum);
        }
    }
    public int score
    {
        get
        {
            return _score;
        }
        set
        {
            if(_score == value) return;

            _score = value;
            OnScoreChanged?.Invoke(_score);
        }
    }
    public int turnsLeft
    {
        get
        {
            return _turnLeft;
        }
        set
        {
            if(_turnLeft == value) return;

            _turnLeft = value;
            OnTurnCountChanged?.Invoke(_turnLeft);
        }
    }
    public int matchesFound
    {
        get
        {
            return _matchFound;
        }
        set
        {
            if(_matchFound == value) return;

            _matchFound = value;
            OnMatchedCardCountChanged?.Invoke(_matchFound);
        }
    }

    void Awake()
    {
        Instance = this;
        _turnLeft = totalTurns;
        _playbleRowNum = 2;
        audioSource = GetComponent<AudioSource>();
    }

    public void TryMatchCard(Card card2, Button button)
    {
        StartCoroutine(FlipCardUp(card2, button));
        if (card1 == null)
        {
            card1 = card2;
            button1 = button;
            return;
        }
        if (card1.cardId == card2.cardId)
        {
            audioSource.clip = cardMatched;
            audioSource.Play();
            card1.isMatched = true;
            card2.isMatched = true;
            score++;
            matchesFound++;
        }
        else
        {
            FlipBackCard(card2, button);
            audioSource.clip = cardNotMatched;
            audioSource.Play();
        }

        turnsLeft--;
        card1 = null;
        button1 = null;
    }

    void FlipBackCard(Card card2, Button button)
    {
        StartCoroutine(FlipCardDown(card1, button1));
        StartCoroutine(FlipCardDown(card2, button));
    }

    IEnumerator FlipCardDown(Card card, Button button)
    {
        Quaternion start = Quaternion.Euler(0, 0, 0);
        Quaternion end = Quaternion.Euler(0, 180, 0);

        yield return new WaitForSeconds(0.5f);
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.25f;
            if (t > 0.5f)
            {
                card.image.sprite = null;
            }
            card.image.transform.rotation = Quaternion.Slerp(start, end, t);
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
        button.interactable = true;
        button.interactable = true;
    }

    IEnumerator FlipCardUp(Card card, Button button)
    {
        Quaternion start = Quaternion.Euler(0, 180, 0);
        Quaternion end = Quaternion.Euler(0, 0, 0);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / 0.25f;
            if (t > 0.5f)
            {
                card.image.sprite = card.sprite;
            }
            card.image.transform.rotation = Quaternion.Slerp(start, end, t);
            yield return null;
        }
    }
    public IEnumerator FlipAllDown(List<Card> targetRotation)
    {
        Quaternion start = Quaternion.Euler(0, 0, 0);
        Quaternion end = Quaternion.Euler(0, 180, 0);

        maxScore = targetRotation.Count / 2;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / 0.4f;
            foreach (Card item in targetRotation)
            {
                if (t > 0.5f)
                {
                    item.image.sprite = null;
                }
                item.image.transform.rotation = Quaternion.Slerp(start, end, t);
            }
            yield return null;
        }

        foreach (Card item in targetRotation)
        {
            item.gameObject.GetComponent<Button>().interactable = true;
        }
    }
}
