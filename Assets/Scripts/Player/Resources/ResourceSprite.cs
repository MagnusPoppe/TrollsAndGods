using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


class ResourceSprite : SpriteSystem
{

    private List<ResourceSprite> resourceSpriteTab;

    protected List<ResourceSprite> ResourceSpriteTab
    {
        get
        {
            return resourceSpriteTab;
        }

        set
        {
            resourceSpriteTab = value;
        }
    }

    public const IngameObjectLibrary.Category CATEGORY = IngameObjectLibrary.Category.Resources;

    public ResourceSprite(int localID) : base(localID, CATEGORY)
    {

    }
}
