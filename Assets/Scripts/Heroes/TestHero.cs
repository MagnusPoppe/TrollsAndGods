class TestHero : Hero
{
    private const int LOCAL_SPRITE_ID = 0;
    private const int PORTRAIT_ID = 0;

    public TestHero(Player player, Point position) 
        : base(player, position, LOCAL_SPRITE_ID, PORTRAIT_ID, "", "")
    {
        Name = "Testhero";
        Description = "Cool dude, yo bro";
    }
}
