/// <summary>
/// Holds the information of the level based on the json file Levels.jon
/// </summary>
[System.Serializable]
public class Level {
    /// <summary>
    /// Level index ordered. Used as an ID to retrieve desired level
    /// </summary>
    public int index;

    /// <summary>
    /// Name of the scene the level refers to. Can be LevelScored, LevelTimed or LevelCollectible
    /// </summary>
    public string sceneName;

    /// <summary>
    /// Width of the board for this level
    /// </summary>
    public int boardWidth;

    /// <summary>
    /// Height of the board for this level
    /// </summary>
    public int boardHeight;

    /// <summary>
    /// Array of integers of the score goals that grants stars
    /// </summary>
    public int[] scoreGoals;

    /// <summary>
    /// Array of booleans that represents which pieces the level will have.
    /// Blue, Cyan, Green, Magenta, Purple, Red, Teal, Yellow
    /// </summary>
    public bool[] pieces;

    /// <summary>
    /// Array of booleans that represents which collectibles the level will have.
    /// Blocker, Collectible
    /// </summary>
    public bool[] collectibles;

    /// <summary>
    /// Array of starting pieces. indicates which piece and position it starts at.
    /// </summary>
    public StartingObject[] startingObjects;

    /// <summary>
    /// Starting moves on the level
    /// </summary>
    public int moves;

    /// <summary>
    /// Starting time for the level in seconds
    /// </summary>
    public int time;

    /// <summary>
    /// Index (ID) of the next level. Use -1 to go to Level Selection, -2 to Main Menu
    /// </summary>
    public int nextLevelIndex;
}
