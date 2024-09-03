
public class EnemyUzi : BaseEnemy
{
    private void Update()
    {
        UpdateDistanceFromPlayer();
        AILogic();

    }
}
