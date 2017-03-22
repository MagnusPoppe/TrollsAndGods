using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    class Gold : SpriteSystem
    {
        private int LOCAL_SPRITE_ID = 0;
        private IngameObjectLibrary.Category CATEGORY = IngameObjectLibrary.Category.Resources;

        public Gold(int localID, IngameObjectLibrary.Category category) : base(localID, category)
        {
        }
    }
