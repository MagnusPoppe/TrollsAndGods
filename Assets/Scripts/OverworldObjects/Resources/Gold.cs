namespace OverworldObjects
{
    public class Gold : ResourcePickup
    {
        static Earn VALUE = new Earn(1000, 0, 0, 0, 0);
        private const int LOCAL_SPRITE_ID = 0;

        public Gold() : base(VALUE, LOCAL_SPRITE_ID)
        {}

        public Gold(int goldValue) : base(LOCAL_SPRITE_ID)
        {
            value = new Earn(goldValue, 0, 0, 0, 0);
        }
    }
}