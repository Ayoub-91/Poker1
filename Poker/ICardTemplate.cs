using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public interface ICardTemplate
    {
        public bool IsConform(string _face, string _suite);
    }
}
