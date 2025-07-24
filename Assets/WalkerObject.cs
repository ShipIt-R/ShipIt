using UnityEngine;

public class WalkerObject
{
    public Vector2Int position;
    public Vector2Int direction = Vector2Int.left;
    public int range_value = 4;
    public float curve_change_value;
    public bool moving = true;
    public int detour;

    public WalkerObject(Vector2Int pos, float chanceToChange, int Detour)
    {
        position = pos;
        curve_change_value = chanceToChange;
        detour = Detour;
    }
}
