using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI
{
    class BuildingCard : SpriteSystem
    {

        const IngameObjectLibrary.Category category = IngameObjectLibrary.Category.UI;

        public BuildingCard(int localID, IngameObjectLibrary.Category category) : base(localID, category)
        {
        }
    }
}
