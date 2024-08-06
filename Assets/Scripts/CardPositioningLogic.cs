using System;
using UnityEngine;

namespace Dudostik.CardTestGame.GameLogic
{
    [Serializable]
    public class CardPositioningLogic
    {
        [SerializeField] private Transform centralCardPosition;
        [SerializeField] private float positionOffset;

        private int currentCardCount = 0;

        public void Reset()
        {
            currentCardCount = 0;
        }

        public Vector3 CalculatePosition(int cardIndex)
        {
            bool isFirstCard = cardIndex == 0;
            Vector3 startPosition = centralCardPosition.position;

            if (!isFirstCard)
            {
                bool isFromRight = cardIndex % 2 == 0;
                float modifier = isFromRight ? 1f : -1f;

                if (!isFromRight)
                    currentCardCount++;

                startPosition += Vector3.right * currentCardCount * positionOffset * modifier;
            }

            return startPosition;
        }
    }
}