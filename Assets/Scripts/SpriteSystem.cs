/// <summary>
/// Sprite system is a common parent for all classes that uses the ingame object library.
/// The class is made for ease of use, and for a common interface to connect with the 
/// library.
/// </summary>
public abstract class SpriteSystem
{
    int LocalSpriteID;

    protected int LocalSpriteId
    {
        set { LocalSpriteID = value; }
    }

    IngameObjectLibrary.Category SpriteCategory;

    /// <summary>
    /// Default constructor. 
    /// Needed to use the Ingame object library. 
    /// </summary>
    /// <param name="localID">Local ID of the sprite. </param>
    /// <param name="category">Category used in the IngameObjectLibrary</param>
    public SpriteSystem(int localID, IngameObjectLibrary.Category category)
    {
        LocalSpriteID = localID;
        SpriteCategory = category;
    }

    /// <summary>
    /// Gets the unique global sprite id of this object.
    /// </summary>
    /// <returns>Sprite ID to be used with the IngameObjectLibrary</returns>
    public int GetSpriteID()
    {
        return LocalSpriteID + IngameObjectLibrary.GetOffset(SpriteCategory);
    }
}