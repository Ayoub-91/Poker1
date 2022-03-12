using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker
{
    public class PokerHandAnalyzer
    {
        public Hand AnalyzeHand(string[] hand)
        {
            var _cards = new List<Card>();

            foreach(var card in hand)
            {
                var _face = card[0].ToString();
                var _suite = card[1].ToString();

                Card _cardInstance;

                if ( IsInputCardJoker(_face,_suite) )
                {
                    if (PokerHand.FindJokers<Joker>(_cards).Count() >= 2)
                        throw new ArgumentException("Cant have more than two jokers");

                    _cardInstance = new Joker();
                    _cards.Add(_cardInstance);
                    continue;
                }
                _cardInstance = new Card(_face , _suite , new PokerTemplate());

                var isDuplicate = _cards.Contains(_cardInstance);

                if (isDuplicate)
                    throw new ArgumentException($"duplicate card : {card}");

                _cards.Add(_cardInstance);
            }

            var _hand = new PokerHand(_cards);

            return _hand.GetHandValue();
        }

        public static bool IsInputCardJoker(string _face , string _suite) => _face == "J" && _suite == "K";

    }
}
