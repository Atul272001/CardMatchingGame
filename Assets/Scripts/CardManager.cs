using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardManager : MonoBehaviour
{
    [SerializeField] List<CardData> cardDataList;
    [SerializeField] List<GameObject> rows;

    public List<Card> cards;
    public List<CardData> cardData;
    public int rowNum;

    List<Card> tempcard = new List<Card>();

    private void Start()
    {
        //if(rowNum > 5)
        //{
        //    rowNum = rows.Count;
        //}
        cardData = new List<CardData>(cardDataList);
        CreateDeck();
    }

    void CreateDeck()
    {
        foreach (GameObject item in rows)
        {
            item.gameObject.SetActive(false);
        }

        for (int i = 0; i < GameManager.Instance.playbleRowNum; i++)
        {
            rows[i].gameObject.SetActive(true);
            Card[] card = rows[i].gameObject.GetComponentsInChildren<Card>();
            for (int j = 0; j < 4; j++)
            {
                cards.Add(card[j]);
                tempcard.Add(card[j]);
            }
        }

        int temp = cards.Count / 2;
        for (int i = 0; i < temp; i++)
        {
            int randomCardData = Random.Range(0, cardData.Count);
            AssignCard(cards[Random.Range(0, cards.Count)], cardData[randomCardData]);
            AssignCard(cards[Random.Range(0, cards.Count)], cardData[randomCardData]);
            cardData.RemoveAt(randomCardData);
        }

        Invoke("FlipDownAllCards", 2f);
    }

    void AssignCard(Card card, CardData randomCardData)
    {
        card.gameObject.GetComponent<Button>().interactable = false;
        card.cardId = randomCardData.cardId;
        card.image.sprite = randomCardData.sprite;
        card.sprite = randomCardData.sprite;
        card.isMatched = false;
        cards.Remove(card);
    }

    void FlipDownAllCards()
    {
        StartCoroutine(GameManager.Instance.FlipAllDown(tempcard));
    }

}
