using UnityEngine;

[System.Serializable]
public struct HexCoordinates
{
    [SerializeField] private int m_x;
    [SerializeField] private int m_z;
    
    public int X
    {
        get { return this.m_x; }
    }
    public int Y
    {
        get  { return -this.X - this.Z; } 
        
    }
    public int Z
    {
        get { return this.m_z; }
    }

    public HexCoordinates (int x, int z)
    {
        this.m_x = x;
        this.m_z = z;
    }

    public static HexCoordinates FromPosition (Vector3 position)
    {
        float x = position.x / (HexMetrics.InnerRadius * 2f);
        float y = -x;
        float offset = position.z / (HexMetrics.OuterRadius * 3f);
        x -= offset;
        y -= offset;

        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x -y);
        
        if (iX + iY + iZ != 0) {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x -y - iZ);

            if (dX > dY && dX > dZ) {
                iX = -iY - iZ;
            }
            else if (dZ > dY) {
                iZ = -iX - iY;
            }
        }
        
        return new HexCoordinates(iX, iZ);
    }
    
    public static HexCoordinates FromOffsetCoordinates (int x, int z)
    {
        return new HexCoordinates(x - z / 2, z);
    }

    public override string ToString ()
    {
        return "(" + this.X + ", " + this.Y + ", " + this.Z + ")";
    }

    public string ToStringOnSeparateLines ()
    {
        return this.X + "\n" + this.Y + "\n" + this.Z;
    }
}