using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dudostik.CardTestGame.Data
{
    [CreateAssetMenu(fileName = "heroData", menuName = "Heroes/heroData")]
    public class HeroData : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public int Health { get; private set; }
    }
}