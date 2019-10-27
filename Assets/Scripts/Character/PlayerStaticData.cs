using UnityEngine;

public class PlayerStaticData
{
    private static Material sP1Material;
    private static Material sP2Material;

    public static Material P1Material
    {
        get
        {
            return sP1Material;
        }
        set
        {
            sP1Material = value;
        }
    }

    public static Material P2Material
    {
        get
        {
            return sP2Material;
        }
        set
        {
            sP2Material = value;
        }
    }
}
