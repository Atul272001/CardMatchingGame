using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Data/card", order = 1)]
public class CardData : ScriptableObject
{
    public Sprite sprite;
    public int cardId;
}
