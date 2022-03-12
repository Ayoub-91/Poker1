using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    class PokerTemplate : ICardTemplate
    {
        public static readonly List<string> Faces = new (("A K Q J T 9 8 7 6 5 4 3 2").Split(" "));
        public static readonly List<string> Suites = new (("H D S C").Split(" "));
        
        public bool IsConform(string _face, string _suite) => (Faces.Contains(_face) && Suites.Contains(_suite)) || (_face == "J" && _suite == "K");
        public static object PossibleCombinations () => Faces.SelectMany(g => Suites.Select(c => new { suite = c, face = g }));
    }
}
