namespace Dudostik.CardTestGame.EntitiesFeatures
{
    public interface IDamageable
    {
        void ReceiveDamage(int damage);
        bool IsDead();
    }
}