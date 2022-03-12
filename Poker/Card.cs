using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public class Card
    {
        protected ICardTemplate template;
        protected string suite;
        protected string face;
        
        public Card(ICardTemplate _template)
        {
            template = _template;
        }

        public Card(string _face, string _suite , ICardTemplate _template) : this(_template)
        {
            var isValid =  template.IsConform(_face, _suite);

            if (!isValid)
                throw new ArgumentException($"non existing face or suite{_face}{_suite}");

            face = _face;
            suite = _suite;
        }
        public bool IsJoker()
        {
            return this is Joker;
        }

        public override bool Equals(object obj)
        {
            return obj is Card && obj.GetHashCode() == this.GetHashCode();
        }

        public override int GetHashCode()
        {
            if (suite == null || face == null)
                return -1;

            return suite.GetHashCode() + face.GetHashCode();
        }

        public int GetRank() => PokerTemplate.Faces.IndexOf(face);
        public char GetSuiteChar() => suite.ToCharArray()[0];
    }
}
