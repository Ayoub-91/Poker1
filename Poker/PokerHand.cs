using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public class PokerHand : IHand
    {
        public List<Card> cards;
        public PokerHand(List<Card> _cards)
        {
            if (_cards.Count != 5)
                throw new ArgumentException("invalid card count");

            cards = _cards;
        }

        public Hand GetHandValue()
        {
            var _jokers = FindJokers<Joker>(cards);

            if (_jokers.Any())
                return GetHighestValue(_jokers);

            return Evaluate();
        }

        private Hand GetHighestValue(IEnumerable<Joker> _jokers)
        {
            dynamic _possibleCombinations = PokerTemplate.PossibleCombinations();
            var _HandValues = new List<Hand>();
            var _possbleHand = new PokerHand(cards);    

            foreach (var _c in _possibleCombinations)
            {
                var _isPossible = _possbleHand.SetJokerValues(_c.face , _c.suite , 0);

                if (!_isPossible)
                    continue;

                if (_jokers.Count() == 2)
                    foreach (var _c1 in _possibleCombinations)
                    {
                       _isPossible = _possbleHand.SetJokerValues(_c1.face, _c1.suite, 1);
                        if (!_isPossible) continue;
                        _HandValues.Add(_possbleHand.Evaluate());
                    }
                else
                _HandValues.Add(_possbleHand.Evaluate());
            }

            var _highest = 1;
            foreach (Hand h in _HandValues)  _highest = (_highest < (int)h) ?  (int)h : _highest;

            return (Hand)_highest;
        }

        private bool FindCard(Card _card) => cards.Contains(_card);

        private bool SetJokerValues(string _face, string _suite, int _jkrIndex)
        {
            var _BadCard = FindCard(new Card(_face, _suite , new PokerTemplate()));
            if (_BadCard)
                return false;

            FindJokers<Joker>(cards).ElementAt(_jkrIndex).SetValue(_face, _suite);
            return true;
        }
       
        public static IEnumerable<Joker> FindJokers<Joker>(List<Card> _cards) => _cards.Where(c => c.IsJoker()).Select(c => (Joker)(object) c);

        private Hand Evaluate()
        {
            if (IsRoyalFlush())
                return Hand.RoyalFlush;

            int[] faceCount = new int[PokerTemplate.Faces.Count];
            long straight = 0, flush = 0;
            foreach (var card in cards)
            {
                int face = card.GetRank();
                straight |= (uint)(1 << face);

                faceCount[face]++;
                flush |= (uint)(1 << card.GetSuiteChar());
            }

            // shift the bit pattern to the right as far as possible
            while (straight % 2 == 0)
                straight >>= 1;

            // straight is 00011111; A-2-3-4-5 is 1111000000001
            bool hasStraight = straight == 0b11111 || straight == 0b1111000000001;

            // unsets right-most 1-bit, which may be the only one set
            bool hasFlush = (flush & (flush - 1)) == 0;

            if (hasStraight && hasFlush)
                return Hand.StraightFlush;

            int total = 0;
            foreach (var count in faceCount)
            {
                if (count == 4)
                    return Hand.FourOfAKind;
                if (count == 3)
                    total += 3;
                else if (count == 2)
                    total += 2;
            }

            if (total == 5)
                return Hand.FullHouse;

            if (hasFlush)
                return Hand.Flush;

            if (hasStraight)
                return Hand.Straight;

            if (total == 3)
                return Hand.ThreeOfAKind;

            if (total == 4)
                return Hand.TwoPair;

            if (total == 2)
                return Hand.Pair;

            return Hand.HighCard;
        }

        private bool IsRoyalFlush()
        {
            var _royalFaces = new List<int>() { 0, 1, 2, 3, 4 };
            var _isStraight = cards.Select(c => c.GetRank()).Intersect(_royalFaces).Count() == 5;
            var _sameSuite = cards.Select(c => c.GetSuiteChar()).Distinct().Count() == 1;
            return _isStraight;
        }
    }
}
