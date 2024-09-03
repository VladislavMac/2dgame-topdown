
public class EnemyPistol : BaseEnemy
{
    private void Update()
    {
        UpdateDistanceFromPlayer();
        AILogic();
    }
}
