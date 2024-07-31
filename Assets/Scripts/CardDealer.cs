using UnityEngine;

public class CardDealer : MonoBehaviour
{
    [SerializeField] private int initialHandCardsCount = 4;

    [Space(5f)]
    [SerializeField] private Hero[] heroesCollection;

    private void Start()
    {
        DealCardsInHand();
    }

    public void DealCardsInHand()
    {
        foreach (Hero hero in heroesCollection) 
        {
            for(int i = 0; i < initialHandCardsCount; i++)
            {
                Card card = hero.Deck.GetCardFromDeck();
                hero.AddCardInHand(card);
            }
        }
    }
}
