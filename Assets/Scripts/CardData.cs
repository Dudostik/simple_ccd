using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dudostik.CardTestGame.Data
{
    [CreateAssetMenu(fileName = "cardData", menuName = "Cards/cardData")]
    public class CardData : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public int Health { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public int MaxCount { get; private set; }
    }
}