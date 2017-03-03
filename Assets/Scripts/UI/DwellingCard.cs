using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI
{
    class DwellingCard : SpriteSystem
    {

        const IngameObjectLibrary.Category category = IngameObjectLibrary.Category.UI;

        public DwellingCard(int localID, IngameObjectLibrary.Category category) : base(localID, category)
        {
        }
    }
}
