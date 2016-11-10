using UnityEngine;

[System.Serializable]
[CreateAssetMenu()]
public class MapInfo :ScriptableObject{
    public Sprite MapImage;

    public Vector2 friendlyFront;
    public Vector2 friendlyTopWing;
    public Vector2 friendlyBottomWing;
    public Vector2 friendlyBack;

    public Vector2 enemyFront;
    public Vector2 enemyTopWing;
    public Vector2 enemyBottomWing;
    public Vector2 enemyBack;
}
