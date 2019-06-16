using System;
using System.Linq;

namespace VideoPoker
{
    class Game
    {
        private GameStates GameState { get; set; }
        private int Credits { get; set; }
        private Deck Deck { get; set; }
        private Hand Hand { get; set; }
        private int HandsWon { get; set; }
        private int HandsLost { get; set; }

        public Game(int startingCredits)
        {
            GameState = GameStates.Uninitialized;
            Credits = startingCredits;
            Deck = new Deck();
            Deck.Shuffle();
            Hand = new Hand(Deck);
        }

        public void Play()
        {
            Console.Clear();
            Console.WriteLine("Credits: {0}\nHands won: {1}\nHands lost: {2}\n", Credits, HandsWon, HandsLost);
            switch ((int)GameState)
            {
                case 0:
                    Console.WriteLine("Press any key to play a hand\nPress Esc to exit");
                    Console.WriteLine("\nRules:\nThe player is given 5 cards and has the opportunity to discard one or more of them in exchange for new ones drawn from the deck." +
                        " After the draw, the machine pays out if the hand or hands played match one of the winning combinations, which are posted in the pay table.\n");
                    Console.WriteLine("        PAY TABLE");
                    Console.WriteLine("-------------------------");
                    Console.WriteLine("Hand            |   Prize");
                    Console.WriteLine("-------------------------");
                    Console.WriteLine(String.Format("{0,-15} | {1,7}", "Royal Flush", "800"));
                    Console.WriteLine(String.Format("{0,-15} | {1,7}", "Straight Flush", "50"));
                    Console.WriteLine(String.Format("{0,-15} | {1,7}", "Four of a kind", "25"));
                    Console.WriteLine(String.Format("{0,-15} | {1,7}", "Full House", "9"));
                    Console.WriteLine(String.Format("{0,-15} | {1,7}", "Flush", "6"));
                    Console.WriteLine(String.Format("{0,-15} | {1,7}", "Straight", "4"));
                    Console.WriteLine(String.Format("{0,-15} | {1,7}", "Three of a kind", "3"));
                    Console.WriteLine(String.Format("{0,-15} | {1,7}", "Two Pair", "2"));
                    Console.WriteLine(String.Format("{0,-15} | {1,7}", "Jacks or Better", "1"));
                    Console.WriteLine(String.Format("{0,-15} | {1,7}", "All Other ", "0"));
                    Console.WriteLine("-------------------------");
                    var key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Bet();
                    }
                    break;
                case 1:
                    int counter = 1;
                    Console.WriteLine("            HAND");
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("Position |              Card");
                    Console.WriteLine("----------------------------");
                    foreach (Card card in Hand.HandCards)
                    {
                        Console.WriteLine(String.Format("{0,-8} | {1,17}", counter, card.Rank + " of " + card.Suit + "s"));
                        counter++;
                    }
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("\n\n\n\n\n\nEnter positions of cards to discard\nPress Enter to continue");
                    string discardCards = Console.ReadLine();
                    int[] cardsIndexes = new int[Globals.handSize];
                    counter = 0;
                    int index;
                    foreach(char c in discardCards)
                    {
                        int.TryParse(c.ToString(), out index);
                        var temp = cardsIndexes.Contains(index-1);
                        if (index > 0 && index < 6 && counter < 5 && !cardsIndexes.Contains(index-1))
                        {
                            cardsIndexes[counter] = index - 1;
                            counter++;
                        }
                    }
                    DiscardCards(cardsIndexes, counter);
                    break;
                case 2:
                    counter = 1;
                    Console.WriteLine("          NEW HAND");
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("Position |              Card");
                    Console.WriteLine("----------------------------");
                    foreach (Card card in Hand.HandCards)
                    {
                        Console.WriteLine(String.Format("{0,-8} | {1,17}", counter, card.Rank + " of " + card.Suit + "s"));
                        counter++;
                    }
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("\n\n\n\n\n\nPress any key to evaluate the hand");
                    Console.ReadKey();
                    EvaluateHand();
                    break;
                case 3:
                    counter = 1;
                    Console.WriteLine("          NEW HAND");
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("Position |              Card");
                    Console.WriteLine("----------------------------");
                    foreach (Card card in Hand.HandCards)
                    {
                        Console.WriteLine(String.Format("{0,-8} | {1,17}", counter, card.Rank + " of " + card.Suit + "s"));
                        counter++;
                    }
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("\nHAND LOST\n\n\n\n\nPress any key to play a hand\nPress Esc to exit");
                    key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Bet();
                    }
                    break;
                case 4:
                    counter = 1;
                    Console.WriteLine("          NEW HAND");
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("Position |              Card");
                    Console.WriteLine("----------------------------");
                    foreach (Card card in Hand.HandCards)
                    {
                        Console.WriteLine(String.Format("{0,-8} | {1,17}", counter, card.Rank + " of " + card.Suit + "s"));
                        counter++;
                    }
                    Console.WriteLine("----------------------------");
                    Console.WriteLine("\nHAND WON");
                    Console.WriteLine("\nHand type: {0}\nCredits won: {1}", Hand.HandType, (int)Hand.HandType);
                    Console.WriteLine("\nPress any key to play a hand\nPress Esc to exit");
                    key = Console.ReadKey();
                    if (key.Key == ConsoleKey.Escape)
                    {
                        Environment.Exit(0);
                    }
                    else
                    {
                        Bet();
                    }
                    break;
                case 5:
                    Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\nGAME OVER");
                    Console.WriteLine("\nBalance too low to continue\n");
                    Console.WriteLine("\nPress any key to exit");
                    Console.ReadKey();
                    Environment.Exit(0);
                    break;
            }
        }

        private void Bet()
        {
            if(Credits > 0)
            {
                Credits--;
                Deck.Reset();
                Deck.Shuffle();
                Hand.HandCards = Deck.Deal(5);
                GameState = GameStates.FirstDeal;
            }
            else
            {
                GameState = GameStates.GameOver;
            }
        }

        private void DiscardCards(int [] cardsIndexes, int cardCount)
        {
            var dealtCards = Deck.Deal(cardsIndexes.Length);
            for (int i = 0; i < cardCount; i++)
            {
                Hand.HandCards[cardsIndexes[i]] = dealtCards[i];
            }
            GameState = GameStates.SecondDeal;
        }

        private void EvaluateHand()
        {
            Hand.Evaluate();
            if (Hand.HandType == 0)
            {
                HandsLost++;
                GameState = GameStates.HandLost;
            }
            else
            {
                HandsWon++;
                Credits += (int)Hand.HandType;
                GameState = GameStates.HandWon;
            }
        }
    }
}
