using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Dudostik.CardTestGame.GameLogic;
using Dudostik.CardTestGame.Data;
using Dudostik.CardTestGame.Services;
using Dudostik.CardTestGame.EntitiesFeatures;

namespace Dudostik.CardTestGame.Entities
{
    public class Hero : StateableObjectBase, IDamageable, ITurnable
    {
        [Space(5f)]
        [SerializeField] private GameObject actionState;
        [SerializeField] private GameObject deathState;

        [Space(5f)]
        [SerializeField] private SpriteRenderer[] sprites;
        [SerializeField] private TextMeshPro healtheText;
        [SerializeField] private TextMeshPro crystalText;
        [SerializeField] private SpriteRenderer turnIndicatorSprite;

        [Space(5f)]
        [SerializeField] private HeroData defaultHeroData;

        [Space(5f)]
        [SerializeField] private CardPositioningLogic handCardPositioningLogic;
        [SerializeField] private CardPositioningLogic tableCardPositioningLogic;

        [Space(5f)]
        [SerializeField] private int startCrystalCount = 1;
        [SerializeField] private string cardInHandState = "READY_STATE";

        [Space(5f)]
        [SerializeField] private Deck deck;
        public Deck Deck => deck;

        private int currentCrystalCount;
        private int maxCrystalCount;
        public int CurrentCrystalCount => currentCrystalCount;
        public int MaxCrystalCount => maxCrystalCount;

        private HeroData currentHeroData;

        private LinkedList<Card> currentCardHandList = new LinkedList<Card>();
        public IReadOnlyCollection<Card> CurrentCardHandList => currentCardHandList;

        private LinkedList<Card> currentCardTableList = new LinkedList<Card>();
        public IReadOnlyCollection<Card> CurrentTableCardList => currentCardTableList;

        private int currentHeroHealth;
        private bool isDead;
        private bool hasTurn;

        public event Action OnDead;

        protected override void Awake()
        {
            base.Awake();

            if (defaultHeroData != null)
                SetHeroData(defaultHeroData);

            SetMaxCrystalCount(startCrystalCount);
            SetCrystalCount(maxCrystalCount);
        }

        public void SetCrystalCount(int crystalCount)
        {
            currentCrystalCount = crystalCount;
            crystalText.SetText(currentCrystalCount.ToString());
        }

        public void SetMaxCrystalCount(int crystalCount)
        {
            maxCrystalCount = crystalCount;
        }

        protected override List<GameObject> GetAllPossibleStates()
        {
            return new List<GameObject> { actionState, deathState };
        }

        public void SetHeroData(HeroData heroData)
        {
            foreach (var spriteRender in sprites)
                spriteRender.sprite = heroData.Icon;

            healtheText.text = heroData.Health.ToString();

            currentHeroData = heroData;

            currentHeroHealth = currentHeroData.Health;
        }

        // Манипуляции с картами
        private void AddCard(Card card, ICollection<Card> cardCollection, CardPositioningLogic positioningLogic, string cardState)
        {
            int cardIndex = cardCollection.Count;

            Vector3 pos = positioningLogic.CalculatePosition(cardIndex);

            card.transform.position = pos;
            card.SetDefaultPostition(pos);
            card.name = $"CARD_{card.CurrentCardData.name}_{cardIndex}";
            card.SetStateByName(cardState);

            cardCollection.Add(card);
        }

        public void AddCardInHand(Card card)
        {
            AddCard(card, currentCardHandList, handCardPositioningLogic, cardInHandState);
            card.SetOnClickMethod((clickedCard) =>
            {
                PutCardOnTable(clickedCard);
                clickedCard.OnTurnStart();
            });
        }

        public void PutCardOnTable(Card card, bool isCheckDiamond = true)
        {
            if (isCheckDiamond && currentCrystalCount < card.CurrentCardData.Price) return;

            AddCard(card, currentCardTableList, tableCardPositioningLogic, "PLAY_STATE");
            card.SetOnClickMethod(null);
            currentCardHandList.Remove(card);
            ResetCardsPositionsInHand();

            if (isCheckDiamond)
                SetCrystalCount(currentCrystalCount - card.CurrentCardData.Price);

            IEnumerator waitAndSetControllable()
            {
                yield return new WaitForSeconds(0.2f);
                card.SetIsControllable(this == ObjectsByTurnOwnerController.LocalHero);
            }

            StartCoroutine(waitAndSetControllable());

            card.OnDeath -= OnCardDeath;
            card.OnDeath += OnCardDeath;

            card.IsPlayable = true;
        }

        private void ResetCardsPositionsOnTable()
        {
            List<Card> temp = new List<Card>(currentCardTableList);
            currentCardTableList.Clear();
            tableCardPositioningLogic.Reset();

            foreach (Card card in temp)
            {
                PutCardOnTable(card, false);
            }
        }

        private void OnCardDeath(Card deathCard)
        {
            currentCardTableList.Remove(deathCard);
            ResetCardsPositionsOnTable();
        }

        private void ResetCardsPositionsInHand()
        {
            List<Card> temp = new List<Card>(currentCardHandList);
            currentCardHandList.Clear();
            handCardPositioningLogic.Reset();

            foreach (Card card in temp)
            {
                AddCardInHand(card);
            }
        }

        public void Death()
        {
            // TODO:
        }
        /////////////////////////////////////////////////////////

        public void OnTurnStateChanged(bool isMyTurn)
        {
            turnIndicatorSprite.gameObject.SetActive(isMyTurn);

            if (isMyTurn)
            {
                foreach (Card cardOnDeck in currentCardTableList)
                {
                    cardOnDeck.OnTurnStart();
                }

                OnTurnStart();
            }
        }

        public void ReceiveDamage(int damage)
        {
            if (isDead) return;

            currentHeroHealth -= damage;
            currentHeroHealth = Mathf.Clamp(currentHeroHealth, 0, int.MaxValue);

            healtheText.SetText(currentHeroHealth.ToString());

            isDead = currentHeroHealth <= 0;

            if (isDead)
            {
                SetStateByName("DEATH_STATE");
                OnDead?.Invoke();
            }
        }

        public void OnTurnStart()
        {
            hasTurn = true;
        }

        public void OnTurnEnd()
        {
            hasTurn = false;
        }

        public bool IsDead()
        {
            return isDead;
        }
    }
}