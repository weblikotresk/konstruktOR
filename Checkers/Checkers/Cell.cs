using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Checkers
{
    public class Cell
    {
        public Position Position;
        public CellTypes Type;
        public Button Content;
        public bool IsToAttack;
        //public bool IsActive;
        //public bool IsToBeEaten;
    }
}
