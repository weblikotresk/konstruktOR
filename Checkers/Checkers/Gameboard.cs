using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Image = System.Windows.Controls.Image;
namespace Checkers
{
    public class Gameboard
    {
        private static Gameboard instance;

        public List<GridButton> GridButtons;
        public bool PlayerTurn { get; set; }//temporary measure

        public bool IsToAttack { get; set; }//add multiple attacks simultaneously

        public int IndexActiveButton { get; set; } //we need to remove this somehow
        public int TypeActiveButton { get; set; }

        public int IndexToBeEaten { get; set; } //we need to remove this somehow
        public int TypeToBeEaten { get; set; }


        public void RenderImage(bool isBlack, int index, double width)
        {
            GridButtons[index].Content.Content = new Image
            {
                Width = width,
                Source =
                isBlack ? new BitmapImage(new Uri("C:\\Users\\space\\Downloads\\konstruktOR-kursova-working\\konstruktOR-kursova-working\\Checkers\\Checkers\\images\\black.png")) : new BitmapImage(new Uri("C:\\Users\\space\\Downloads\\konstruktOR-kursova-working\\konstruktOR-kursova-working\\Checkers\\Checkers\\images\\white.png"))
            };
        }

        public void RemovePiece(int index)
        {
            GridButtons[index].Content.Content = null;
            GridButtons[index].Type = (int)CellTypes.Empty;
        }


        public int GetType(int index)
        {
            return GridButtons[index].Type;
        }
        public int GetType(int row, int col)
        {
            return GridButtons[row * 8 + col].Type;
        }

        public void SetType(int index, int type)
        {
            GridButtons[index].Type = type;
        }
        public void SetType(int row, int col, int type)
        {
            GridButtons[row * 8 + col].Type = type;
        }

        public void RemoveElement(int index) //???? для чего - непонятно
        {
            GridButtons[index].Type = (int)CellTypes.Empty;
        }

        public void MoveElement(int oldIndex, int newIndex)
        {
            GridButtons[newIndex].Type = GridButtons[oldIndex].Type;
            GridButtons[oldIndex].Type = (int)CellTypes.Empty;
        }

        public Gameboard()
        {


            //blacks are 1, whites are 2, empty cell is zero;
            //supreme pieces will be negative, i.e. -1 is supreme black piece
            // highlighted empty cells are 3

            //{0,1,0,1,0,1,0,1},
            //{1,0,1,0,1,0,1,0},
            //{0,1,0,1,0,1,0,1},
            //{0,0,0,0,0,0,0,0},
            //{0,0,0,0,0,0,0,0},
            //{2,0,2,0,2,0,2,0},
            //{0,2,0,2,0,2,0,2},
            //{2,0,2,0,2,0,2,0}};

            PlayerTurn = true;
            IsToAttack = false;

            GridButtons = new List<GridButton>();

        }
        //i= rows, j=cols


        //виды ходов:
        /*
         +1. ход на пустое место

            [i+1][j+1]=0 || [i-1][j-1]=0
        невозможность нажатия на пустые кнопки, кроме предлагаемых


        2. ход через шашку


         */


        public static Gameboard Instance
        {
            get
            {
                if (instance == null)
                    instance = new Gameboard();
                return instance;
            }
        }
    }
}
