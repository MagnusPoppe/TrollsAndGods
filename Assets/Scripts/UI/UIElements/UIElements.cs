
namespace UI
{
    /// <summary>
    /// Class to hold UI element sprites
    /// </summary>
    class UIElements : SpriteSystem
    {
        public const IngameObjectLibrary.Category CATEGORY = IngameObjectLibrary.Category.UI;

        public UIElements(int localID) : base(localID, CATEGORY)
        {
        }
    }
}
