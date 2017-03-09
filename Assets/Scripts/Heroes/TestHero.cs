class TestHero : Hero
{
    private const int LOCAL_SPRITE_ID = 0;
    const IngameObjectLibrary.Category SPRITE_CATEGORY = IngameObjectLibrary.Category.ResourceBuildings;

    public TestHero(Player player, Point position) 
        : base(player, position)
    {
        Name = "Testhero";
        CurMovementSpeed = MovementSpeed = 12;
    }
}
