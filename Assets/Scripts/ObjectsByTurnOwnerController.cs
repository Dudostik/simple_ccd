using UnityEngine;
using Dudostik.CardTestGame.Entities;

namespace Dudostik.CardTestGame.Services
{
    public class ObjectsByTurnOwnerController : MonoBehaviour
    {
        [SerializeField] private TurnsManager turnsManager;
        [SerializeField] private Hero ownerHero;
        [SerializeField] private GameObject[] gameObjectsToControlCollection;

        public static Hero LocalHero { get; private set; }

        private void Awake()
        {
            LocalHero = ownerHero;
            turnsManager.OnNextTurn += HandleOnNextTurn;
        }

        private void OnDestroy()
        {
            turnsManager.OnNextTurn -= HandleOnNextTurn;
        }

        private void HandleOnNextTurn(Hero hero)
        {
            foreach (GameObject go in gameObjectsToControlCollection)
                go.SetActive(hero == ownerHero);
        }
    }
}