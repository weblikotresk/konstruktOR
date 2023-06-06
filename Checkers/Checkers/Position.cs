using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public class Position
    {
        public int Row;
        public int Column;
        public int Index;
        public Position(int x, int y) {
            Row = x; Column = y;
            Index = x * 8 + y;
        }

    }
}
