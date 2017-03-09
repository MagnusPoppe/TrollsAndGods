namespace Units
{
    public class StoneTroll
    {

        string name;
        int attack;
        int defence;
        int speed;
        int magic;
        int health;
        Move[] moves;
        Ability ability;

        public StoneTroll()
        //  :base(string name, int attack, int defence, int speed, int magic, int health, Move[] move, Ability ability) TODO: make parent
        {
            this.name = "Stone Troll";
            this.attack = 22;
            this.defence = 20;
            this.health = 100;
            this.speed = 11;
            this.magic = 5;

            moves = new Move[] { new TreeSwing(), new BoulderThrow()};
            //ability = new StoneSkin(); TODO: make ability
        }
    }
}
