using System;

namespace VideoPoker
{
    class Card
    {
        public Suit Suit { get; private set; }
        public Rank Rank { get; private set; }
        public Boolean Locked { get; set; }

        public Card(Suit Suit, Rank Rank)
        {
            this.Suit = Suit;
            this.Rank = Rank;
            Locked = false;
        }
    }
}
