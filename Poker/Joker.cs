using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class Joker : Card
    {
        public Joker() : base(new PokerTemplate()) { HasValue = false; }

        public bool HasValue { get; private set; }

        public void SetValue(string _face , string _suite)
        {
            face = _face;
            suite = _suite;
            HasValue = true;
        }
    }
}
