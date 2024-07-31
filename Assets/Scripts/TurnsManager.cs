using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnsManager : MonoBehaviour
{
    [SerializeField] private Hero[] heroesOnScene;
    private int currentPlayerIndex = -1;
    private Hero currentTurnHero;

    public event Action<Hero> OnNextTurn;

    private void Awake()
    {
        NextTurn();
    }

    public void NextTurn()
    {
        if(currentTurnHero != null)
            currentTurnHero.OnTurnStateChanged(false);

        currentPlayerIndex++;

        if(currentPlayerIndex >= heroesOnScene.Length)
            currentPlayerIndex = 0; 

        Hero hero = heroesOnScene[currentPlayerIndex];

        if (hero.MaxCrystalCount < 10)
        {
            hero.SetMaxCrystalCount(hero.MaxCrystalCount + 1);
            hero.SetCrystalCount(hero.MaxCrystalCount);
        }
        else
        {
            hero.SetMaxCrystalCount(10);
        }

        currentTurnHero = hero;
        currentTurnHero.OnTurnStateChanged(true);

        Card card = hero.Deck.GetCardFromDeck();

        if (card != null)
        {
            hero.AddCardInHand(card);
            OnNextTurn?.Invoke(currentTurnHero);
        }
        else
        {
            OnNextTurn?.Invoke(currentTurnHero);
        }
    }
}
