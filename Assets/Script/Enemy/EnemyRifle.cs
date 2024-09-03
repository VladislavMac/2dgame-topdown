
namespace Enemy
{
    public class EnemyRifle : BaseEnemy
    {
        private void Update()
        {
            UpdateDistanceFromPlayer();
            AILogic();

        }
    }
}