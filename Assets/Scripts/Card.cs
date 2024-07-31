using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class Card : StateableObjectBase, IDamageable, IAttackable, IPlayableObject, ITurnable
{
    [SerializeField] private GameObject actionState;
    [SerializeField] private GameObject deathState;
    [SerializeField] private GameObject hideState;
    [SerializeField] private GameObject readyState;

    [Space(5f)]
    [SerializeField] private CardData defaultCardData;

    [Space(5f)]
    [SerializeField] private SpriteRenderer[] sprites;
    [SerializeField] private TextMeshPro priceText;
    [SerializeField] private TextMeshPro readyDamageText;
    [SerializeField] private TextMeshPro readyHealthText;
    [SerializeField] private TextMeshPro playDamageText;
    [SerializeField] private TextMeshPro playHealthText;

    [Space(5f)]
    [SerializeField] private ECardGameplayState cardState;

    private CardData currentCardData;
    public CardData CurrentCardData => currentCardData;

    private Action<Card> onClick;
    public event Action<Card> OnDeath;

    private bool isControllable;

    private Vector3 defaultPosition;

    private int currentHealth;

    private bool isDead = false;

    public bool IsPlayable { get; set; } = false;
    private ObjectMouseBehaviour objectMouseBehaviour;
    private bool hasTurn;

    public enum ECardGameplayState : byte
    {
        NONE, ON_HAND, ON_TABLE
    }

    protected override void Awake()
    {
        base.Awake();

        if (defaultCardData != null )
            SetCardData(defaultCardData);
    }

    public void Attack(IDamageable damageable)
    {
        damageable.ReceiveDamage(CurrentCardData.Damage);
    }

    public void SetCardGameplayState(ECardGameplayState cardState)
    {
        this.cardState = cardState;
    }

    public void SetIsControllable(bool isControllable) 
    {
        this.isControllable = isControllable;
    }

    public void SetDefaultPostition(Vector3 defaultPosition)
    {
        this.defaultPosition = defaultPosition;
    }

    public void SetCardData(CardData cardData)
    {
        foreach (var spriteRender in sprites)
            spriteRender.sprite = cardData.Icon;

        // damageText.SetText(cardData.Damage.ToString());

        readyHealthText.text = cardData.Health.ToString();
        readyDamageText.text = cardData.Damage.ToString();
        playHealthText.text = cardData.Health.ToString();
        playDamageText.text = cardData.Damage.ToString();
        priceText.text = cardData.Price.ToString();

        currentCardData = cardData;
        currentHealth = cardData.Health;

        objectMouseBehaviour = new ObjectMouseBehaviour(
            () => !isDead && isControllable && hasTurn,
            () => !isDead && isControllable && hasTurn,
            () => defaultPosition,
            transform,
            GetDamage(),
            this,
            this
            );
    }

    protected override List<GameObject> GetAllPossibleStates()
    {
        return new List<GameObject>
        {
            actionState, deathState, hideState, readyState
        };
    }

    public void SetOnClickMethod(Action<Card> onClick)
    {
        this.onClick = onClick;
    }

    private void OnMouseDrag()
    {
        objectMouseBehaviour.OnMouseDrag();
    }

    private void OnMouseUp()
    {
        objectMouseBehaviour.OnMouseUp();
    }

    private void OnMouseDown()
    {
        if (isDead) return;
        onClick?.Invoke(this);
    }

    public void ReceiveDamage(int damage)
    {
        if(isDead) return;

        currentHealth -= damage;
        currentHealth = Math.Clamp(currentHealth, 0, int.MaxValue);

        if(currentHealth == 0)
        {
            SetStateByName("DEATH_STATE");
            StartCoroutine(DeathCoroutine());
            isDead = true;
        }

        playHealthText.SetText(currentHealth.ToString());
    }

    private IEnumerator DeathCoroutine()
    {
        yield return new WaitForSeconds(2f);
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }

    public int GetDamage()
    {
        return currentCardData.Damage;
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
