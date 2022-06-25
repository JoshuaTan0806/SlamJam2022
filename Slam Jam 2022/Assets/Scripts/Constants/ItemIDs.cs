// By Luke Jones

using UnityEngine;
using Items;
/// <summary>
/// Contains contants specific to items
/// </summary>
public static class ItemIDs
{
    public const byte INVENTORY_SIZE = 3;

    //Error codes cannot be const sadly
    public static System.Exception NOT_INSTANCED_ERROR = new System.Exception("Equipping ItemData! Items should be instance using CreateInstance before equipping!");
    public static System.NotImplementedException NOT_IMPLEMENTED_CONNECTION = new System.NotImplementedException("Connection Type not supported");

    public static ConnectionDirection GetOppositeDirection(ConnectionDirection direction)
    {   //Get the opposite direction
        switch (direction)
        {
            case ConnectionDirection.NORTH:
                return ConnectionDirection.SOUTH;
            case ConnectionDirection.SOUTH:
                return ConnectionDirection.NORTH;
            case ConnectionDirection.EAST:
                return ConnectionDirection.WEST;
            case ConnectionDirection.WEST:
                return ConnectionDirection.EAST;
            default:
                throw NOT_IMPLEMENTED_CONNECTION;
        }    
    }

    public static Color ToColor(ConnectionType t)
    {
        switch (t)
        {
            case ConnectionType.RED:
                return Color.red;
            case ConnectionType.BLUE:
                return Color.blue;
            case ConnectionType.GREEN:
                return Color.green;
            default:
                throw NOT_IMPLEMENTED_CONNECTION;
        }
    }
}
