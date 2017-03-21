using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Ore : SpriteSystem
{
    private int LOCAL_SPRITE_ID = 2;
    private IngameObjectLibrary.Category CATEGORY = IngameObjectLibrary.Category.Resources;

    public Ore(int localID, IngameObjectLibrary.Category category) : base(localID, category)
    {
    }
}
