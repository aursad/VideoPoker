using System;

namespace VideoPoker
{
    class Hand
    {
        public Card[] HandCards { get; set; }
        public HandType HandType { get; private set; }
        private HandTypeDelegate[] HandTypeDel { get; set; }
        private delegate Boolean HandTypeDelegate();
        
        public Hand(Deck deck)
        {
            HandCards = deck.Deal(Globals.handSize);
            HandTypeDel = new HandTypeDelegate[]{ new HandTypeDelegate(IsRoyal), new HandTypeDelegate(IsStraightFlush),
                        new HandTypeDelegate(IsFourOfAKind), new HandTypeDelegate(IsFullHouse), new HandTypeDelegate(IsFlush),
                        new HandTypeDelegate(IsStraight), new HandTypeDelegate(IsThreeOfAKind), new HandTypeDelegate(IsTwoPair),
                        new HandTypeDelegate(IsJacksOrBetter)};
        }

        public void Evaluate()
        {
            foreach(HandTypeDelegate del in HandTypeDel)
            {
                if (del())
                {
                    return;
                }
            }
            HandType = HandType.Other;
        }

        private Boolean IsRoyal()
        {
            if (HasIncementalRank() && HasSameSuit() && ContainsRank(Rank.Ace))
            {
                HandType = HandType.RoyalFlush;
                return true;
            }
            return false;
        }

        private Boolean IsStraightFlush()
        {
            if (HasIncementalRank() && HasSameSuit())
            {
                HandType = HandType.StraightFlush;
                return true;
            }
            return false;
        }

        private Boolean IsFourOfAKind()
        {
            if (HasTwoPairs(true))
            {
                HandType = HandType.FourOfAKind;
                return true;
            }
            return false;
        }

        private Boolean IsFullHouse()
        {
            if (HasTwoPairs(false) && IsThreeOfAKind())
            {
                HandType = HandType.FullHouse;
                return true;
            }
            return false;
        }

        private Boolean IsFlush()
        {
            if (HasSameSuit())
            {
                HandType = HandType.Flush;
                return true;
            }
            return false;
        }

        private Boolean IsStraight()
        {
            if (HasIncementalRank())
            {
                HandType = HandType.Stright;
                return true;
            }
            return false;
        }

        private Boolean IsThreeOfAKind()
        {
            Card match = null;
            for (int i = 0; i < HandCards.Length - 1; i++)
            {
                if(match == null)
                {
                    if (HasMatch(HandCards[i], i))
                    {
                        match = HandCards[i];
                    }
                }
                else
                {
                    if(HasMatch(HandCards[i], i) && HandCards[i].Rank == match.Rank)
                    {
                        HandType = HandType.ThreeOfAKind;
                        return true;
                    }
                }
            }
            return false;
        }

        private Boolean IsTwoPair()
        {
            if (HasTwoPairs(false))
            {
                HandType = HandType.TwoPair;
                return true;
            }
            return false;
        }

        private Boolean IsJacksOrBetter()
        {
            for(int i = 0; i < HandCards.Length - 1; i++)
            {
                int rank = (int)HandCards[i].Rank;
                if (rank > 9 || rank == 0)
                {
                    if (HasMatch(HandCards[i], i))
                    {
                        HandType = HandType.JacksOrBetter;
                        return true;
                    }
                }
            }
            return false;
        }



        private Boolean HasMatch(Card card, int index)
        {
            int rank = (int)card.Rank;
            for (int i = index + 1; i < Globals.handSize; i++)
            {
                if ((int)HandCards[i].Rank == rank)
                {
                    return true;
                }
            }
            return false;
        }

        private Boolean HasIncementalRank()
        {
            int[] ranks = new int[Globals.handSize];
            for(int i = 0; i < Globals.handSize; i++)
            {
                ranks[i] = (int)HandCards[i].Rank;
            }
            Array.Sort(ranks);
            if (ranks[1] + 1 == ranks[2] && ranks[2] + 1 == ranks[3] &&
                ranks[3] + 1 == ranks[4])
            {
                if (ContainsRank(Rank.Ace))
                {
                    if(ranks[4] == (int)Rank.King || ranks[1] == (int)Rank.Two)
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    if(ranks[0] + 1 == ranks[1])
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;
        }

        private Boolean HasSameSuit()
        {
            Suit suit = HandCards[0].Suit;
            for(int i = 1; i < Globals.handSize; i++)
            {
                if(HandCards[i].Suit != suit)
                {
                    return false;
                }
            }
            return true;
        }

        private Boolean HasTwoPairs(Boolean sameRank)
        {
            int pairRank = -1;
            int matches = 1;
            for (int i = 0; i < HandCards.Length - 1; i++)
            {
                int rank = (int)HandCards[i].Rank;
                if (HasMatch(HandCards[i], i))
                {
                    if (pairRank == -1)
                    {
                        pairRank = (int)HandCards[i].Rank;
                        matches++;
                    }
                    else
                    {
                        if (sameRank)
                        {
                            if (pairRank == (int)HandCards[i].Rank)
                            {
                                matches++;
                            }
                            if (matches == 4)
                            {
                                HandType = HandType.FourOfAKind;
                                return true;
                            }
                        }
                        else if(pairRank != (int)HandCards[i].Rank)
                        {
                            HandType = HandType.TwoPair;
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private Boolean ContainsRank(Rank rank)
        {
            foreach(Card card in HandCards)
            {
                if(card.Rank == rank)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
