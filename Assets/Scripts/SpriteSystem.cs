﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public abstract class SpriteSystem
{
    int LocalSpriteID;
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