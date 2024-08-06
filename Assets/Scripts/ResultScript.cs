using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Dudostik.CardTestGame.Entities;

namespace Dudostik.CardTestGame.GameLogic
{
    public class ResultScript : MonoBehaviour
    {
        [SerializeField] private Hero localHero;
        [SerializeField] private Hero enemyHero;

        [Space(5f)]
        [SerializeField] private GameObject resultContainer;
        [SerializeField] private TextMeshProUGUI resultScreenText;
        [SerializeField] private Button startAgainButton;
        [SerializeField] private string sceneToLoad;

        private void Awake()
        {
            localHero.OnDead += OnLocalHeroDead;
            enemyHero.OnDead += OnEnemyHeroDead;

            startAgainButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(sceneToLoad);
            });
        }

        private void OnDestroy()
        {
            localHero.OnDead -= OnLocalHeroDead;
            enemyHero.OnDead -= OnEnemyHeroDead;
        }

        private void OnLocalHeroDead()
        {
            resultContainer.SetActive(true);
            resultScreenText.SetText("You died");
            resultScreenText.color = Color.red;
        }

        private void OnEnemyHeroDead()
        {
            resultContainer.SetActive(true);
            resultScreenText.SetText("You won");
            resultScreenText.color = Color.green;
        }
    }
}