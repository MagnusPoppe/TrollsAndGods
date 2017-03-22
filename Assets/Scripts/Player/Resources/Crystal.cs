using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class Crystal : SpriteSystem
{
    private int LOCAL_SPRITE_ID = 3;
    private IngameObjectLibrary.Category CATEGORY = IngameObjectLibrary.Category.Resources;

    public Crystal(int localID, IngameObjectLibrary.Category category) : base(localID, category)
    {
    }
}
