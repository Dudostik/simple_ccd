using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    [SerializeField] private Card cardPrefab;

    [Space(5f)]
    [SerializeField] private CardData[] possibleCardDataCollection;

    [Space(5f)]
    [SerializeField] private int initialDeckCardsCount = 12;

    [Space(5f)]
    [SerializeField] private Transform deckCardPosition;

    private Stack<Card> currentCardDeckList = new Stack<Card>();
    public IReadOnlyCollection<Card> CurrentCardDeckList => currentCardDeckList;

    private void Start()
    {
        Dictionary<CardData, int> cardsCounter = new Dictionary<CardData, int>(16);
        int i = 0;

        while(i < initialDeckCardsCount)
        {
            Card cardInstance = Instantiate(cardPrefab);
            CardData randomCardData = possibleCardDataCollection[Random.Range(0, possibleCardDataCollection.Length)];

            if (cardsCounter.TryGetValue(randomCardData, out int count))
            {
                if (count >= randomCardData.MaxCount)
                {
                    continue;
                }
                else
                {
                    cardsCounter[randomCardData]++;
                }
            }
            else
            {
                cardsCounter.Add(randomCardData, 0);
            }

            cardInstance.SetCardData(randomCardData);
            currentCardDeckList.Push(cardInstance);

            Vector3 pos = deckCardPosition.position;
            cardInstance.transform.position = pos;
            cardInstance.SetDefaultPostition(pos);

            cardInstance.SetStateByName("HIDE_STATE");
            i++;
        }
    }

    /*
    public void CreateDeck()
    {
        for (int i = 0; i < initialDeckCardsCount; i++)
        {
            Card cardInstance = Instantiate(cardPrefab);
            CardData randomCardData = possibleCardDataCollection[Random.Range(0, possibleCardDataCollection.Length)];
            cardInstance.SetCardData(randomCardData);
            currentCardDeckList.AddFirst(cardInstance);
        }
    }*/

    public void AddCardInDeck(Card card)
    {
        currentCardDeckList.Push(card);
    }

    public Card GetCardFromDeck()
    {
        if(currentCardDeckList.TryPop(out var card))
            return card;
        else
            return null;
    }
}
