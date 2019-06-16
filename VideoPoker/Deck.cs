using System;

namespace VideoPoker
{
    class Deck
    {
        public Card[] Cards { get; private set;  }
        public int LastLocked { get; private set; }

        public Deck()
        {
            Cards = new Card[Globals.deckSize];
            int index = 0;
            foreach (Suit suit in (Suit[])Enum.GetValues(typeof(Suit)))
            {
                foreach (Rank rank in (Rank[])Enum.GetValues(typeof(Rank)))
                {
                    Cards[index] = new Card(suit, rank);
                    index++;
                }
            }
        }

        public void Shuffle()
        {
            Random rand = new Random();

            for (int i = 0; i < Cards.Length - 1; i++)
            {
                int j = rand.Next(i, Cards.Length);
                Card temp = Cards[i];
                Cards[i] = Cards[j];
                Cards[j] = temp;
            }
        }

        public Card[] Deal(int numCards)
        {
            Card[] DealtCards = new Card[numCards];
            for(int i = 0; i< numCards; i++)
            {
                DealtCards[i] = Cards[LastLocked + i + 1];
                Cards[LastLocked + i].Locked = true;
            }
            LastLocked += numCards;

            return DealtCards;
        }

        public void Reset()
        {
            for(int i = 0; i < LastLocked; i++)
            {
                Cards[i].Locked = false;
            }
            LastLocked = 0;
        }
    }
}
