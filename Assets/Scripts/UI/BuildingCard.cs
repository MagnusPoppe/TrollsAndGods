namespace UI
{
    /// <summary>
    /// The UI Card for buildings in the Town View
    /// </summary>
    class BuildingCard : SpriteSystem
    {

        const IngameObjectLibrary.Category category = IngameObjectLibrary.Category.UI;

        public BuildingCard(int localID, IngameObjectLibrary.Category category) : base(localID, category)
        {
        }
    }
}
