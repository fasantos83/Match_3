using UnityEngine;

/// <summary>
/// Holds the information of a object(piece or tile) and position it starts at.
/// </summary>
[System.Serializable]
public class StartingObject {

    /// <summary>
    /// Index of the piece on the enum of possible objects (StartingObjectEnum)
    /// </summary>
    public int enumIndex;
    public GameObject prefab;
    public int x;
    public int y;
    public int z;

}