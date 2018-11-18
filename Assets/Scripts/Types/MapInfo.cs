//Mapinfo gives information about how the slots line up on the image
//All doubles represent percentages of the screen, not pixels
//For instance a skyline of 0.5 will make the sky go halfway down the screen, a skyline of 0.1 will only go 10% down
using UnityEngine;

[System.Serializable]
[CreateAssetMenu()]
public class MapInfo :ScriptableObject{
    public Sprite MapImage;

    public double skyline;
    public double groundLine;
    public double frontlineLine;
    public double backlineLine;
    public double topWingLine;
    public double bottomWingLine;
}
