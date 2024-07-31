using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeroAI : MonoBehaviour
{
    [SerializeField] private TurnsManager turnsManager;
    [SerializeField] private Hero aiHero;
    [SerializeField] private Hero enemyHero;

    private void Awake()
    {
        turnsManager.OnNextTurn += HandleOnTurnOwnerChanged;
    }

    private void OnDestroy()
    {
        turnsManager.OnNextTurn -= HandleOnTurnOwnerChanged;
    }

    private void HandleOnTurnOwnerChanged(Hero hero)
    {
        if (hero != aiHero)
            return;

        StartCoroutine(DoTurnCoroutine());
    }

    private IEnumerator DoTurnCoroutine()
    {
        bool isCardBought = false;
        foreach (Card cardInHand in new List<Card>(aiHero.CurrentCardHandList))
        {
            if (cardInHand.CurrentCardData.Price <= aiHero.CurrentCrystalCount)
            {
                aiHero.PutCardOnTable(cardInHand);
                cardInHand.OnTurnStart();
                isCardBought = true;
            }
        }

        float delay = isCardBought ? 1.25f : 0.5f;

        yield return new WaitForSeconds(delay);

        Card cardToAttack = enemyHero.CurrentTableCardList.FirstOrDefault();

        foreach(Card cardInDeck in aiHero.CurrentTableCardList) 
        {
            if (cardToAttack && !cardToAttack.IsDead())
            {
                cardInDeck.Attack(cardToAttack);
                cardInDeck.ReceiveDamage(cardToAttack.GetDamage());
            }
            else
            {
                cardInDeck.Attack(enemyHero);
            }

            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(delay);

        turnsManager.NextTurn();

    }
}
