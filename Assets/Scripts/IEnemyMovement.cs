using UnityEngine;

public interface IEnemyMovement
{
    MovementPattern GetPattern();
    void SetPattern(MovementPattern pattern);
}