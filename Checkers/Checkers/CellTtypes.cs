using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkers
{
    public enum CellTypes //типы клеток
    {
        Empty,
        Black,
        White,
        Highlighted,
        BlackSupreme, //black + 3
        WhiteSupreme  //white + 3
    }


}
