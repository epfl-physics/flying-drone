using UnityEngine;

[CreateAssetMenu(menuName = "Activities/Velocity Colors", fileName = "New Velocity Colors", order = 0)]
public class VelocityColors : ScriptableObject
{
    public Color unknown;
    public Color absolute;
    public Color platform;
    public Color relative;
    public Color tangential;

    public Color GetColorFromIndex(int index)
    {
        Color color = Color.black;
        switch (index)
        {
            case 0:
                color = absolute;
                break;
            case 1:
                color = platform;
                break;
            case 2:
                color = relative;
                break;
            case 3:
                color = tangential;
                break;
            default:
                break;
        }
        return color;
    }
}
