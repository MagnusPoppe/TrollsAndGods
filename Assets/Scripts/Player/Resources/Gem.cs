using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Gem : SpriteSystem
{
    private int LOCAL_SPRITE_ID = 4;
    private IngameObjectLibrary.Category CATEGORY = IngameObjectLibrary.Category.Resources;

    public Gem(int localID, IngameObjectLibrary.Category category) : base(localID, category)
    {
    }
}
