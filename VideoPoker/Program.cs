namespace VideoPoker
{
    class Program
    {
        static void Main(string[] args)
        {
            Game game = new Game(Globals.startingCredits);
            while (true)
            {
                game.Play();
            }
        }
    }
}
