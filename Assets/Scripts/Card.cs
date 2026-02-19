using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int cardId;
    public Image image;
    public Sprite sprite;
    public bool isMatched;
    Button button;

    void Awake()
    {
        isMatched = false;
        button = GetComponent<Button>();
        button.onClick.AddListener(OnButtonClick);
    }

    public void OnButtonClick()
    {
        button.interactable = false;
        GameManager.Instance.audioSource.clip = GameManager.Instance.cardClickSound;
        GameManager.Instance.audioSource.Play();
        Debug.Log(cardId + " Clicked");
        if (!isMatched)
        GameManager.Instance.TryMatchCard(this, button);
    }



}
