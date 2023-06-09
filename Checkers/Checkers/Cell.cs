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
        public Position Position; //позиция клетки
        public CellTypes Type; //тип клетки
        public Button Content;//наполнение клетки - кнопка
        public bool IsToAttack;//флаг атаки
    }
}
