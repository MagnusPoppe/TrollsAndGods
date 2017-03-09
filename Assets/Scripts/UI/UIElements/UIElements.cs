using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI
{
    class UIElements : SpriteSystem
    {
        public const IngameObjectLibrary.Category CATEGORY = IngameObjectLibrary.Category.UI;

        public UIElements(int localID) : base(localID, CATEGORY)
        {
        }
    }
}
