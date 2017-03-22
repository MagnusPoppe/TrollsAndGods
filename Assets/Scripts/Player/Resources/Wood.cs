using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Wood : SpriteSystem
{
    private int LOCAL_SPRITE_ID = 1;
    private IngameObjectLibrary.Category CATEGORY = IngameObjectLibrary.Category.Resources;

    public Wood(int localID, IngameObjectLibrary.Category category) : base(localID, category)
    {
    }
}
