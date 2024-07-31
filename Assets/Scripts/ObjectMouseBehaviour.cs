using System;
using UnityEngine;

public class ObjectMouseBehaviour
{
    private Func<bool> isMouseDragEnabled;
    private Func<bool> isMouseUpEnabled;
    private Func<Vector3> getDefaultPosition;
    private Transform transform;
    private int damage;
    private IDamageable selfDamageable;
    private ITurnable selfTurnable;

    public ObjectMouseBehaviour(Func<bool> isMouseDragEnabled, Func<bool> isMouseUpEnabled, Func<Vector3> getDefaultPosition, Transform transform,
        int damage, IDamageable selfDamageable, ITurnable selfTurnable)
    {
        this.isMouseDragEnabled = isMouseDragEnabled;
        this.isMouseUpEnabled = isMouseUpEnabled;
        this.getDefaultPosition = getDefaultPosition;
        this.transform = transform;
        this.damage = damage;
        this.selfDamageable = selfDamageable;
        this.selfTurnable = selfTurnable;
    }

    public void OnMouseDrag()
    { 
        if (isMouseDragEnabled() == false) return;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        transform.position = mousePos;
    }

    public void OnMouseUp()
    {
        if (isMouseDragEnabled() == false) return;

        Collider2D[] collidersCollection = Physics2D.OverlapBoxAll(transform.position, Vector2.one * 0.05f, 2f);

        foreach (Collider2D catchedCollider in collidersCollection)
        {
            if (catchedCollider.gameObject.GetInstanceID() == transform.gameObject.GetInstanceID()) continue;

            IPlayableObject playable = catchedCollider.GetComponent<IPlayableObject>();

            if (playable != null && !playable.IsPlayable) continue;

            IDamageable target = catchedCollider.GetComponent<IDamageable>();

            if (target != null)
            {
                target.ReceiveDamage(damage);

                IAttackable attackable = catchedCollider.GetComponent<IAttackable>();

                if (attackable != null)
                {
                    selfDamageable.ReceiveDamage(attackable.GetDamage());
                }

                selfTurnable.OnTurnEnd();
            }
            break;
        }

        transform.position = getDefaultPosition();
    }
}
