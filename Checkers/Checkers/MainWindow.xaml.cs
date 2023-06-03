using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Drawing;
using static Checkers.MainWindow;
using Image = System.Windows.Controls.Image;

namespace Checkers
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Grid MainGrid { get; set; }
        //public Gameboard Gameboard { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }
        public void Init()
        {
            MainGrid = CreateGrid();
            mainWindow.Content = MainGrid;
            //Gameboard = new Gameboard();
        }



        public Grid CreateGrid()
        {
            Grid myGrid = new Grid() {
                Width = 400,
                Height = 400,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,

            };
            for (short i = 0; i < 8; i++)
            {
                myGrid.ColumnDefinitions.Add(new ColumnDefinition());
                myGrid.RowDefinitions.Add(new RowDefinition());
                for (short j = 0; j < 8; j++)
                {
                    GridButton gridButton = new GridButton {
                        Row = i,
                        Column = j,
                        Content = new Button()
                    };
                    gridButton.Content.Style = ((i + j) % 2 == 0) ? ButtonStyles.Instance.whiteCellStyle : ButtonStyles.Instance.grayCellStyle;
                    gridButton.Content.Click += new RoutedEventHandler(CellClicked);
                    gridButton.Content.LostFocus += new RoutedEventHandler(PieceDefocused);

                    Gameboard.Instance.GridButtons.Add(gridButton);
                    if ((i + j) % 2 != 0 && i <= 2)
                    {
                        Gameboard.Instance.RenderImage(true, i * 8 + j, myGrid.Width / 10);
                        gridButton.Type = (int)CellTypes.Black;
                    }
                    else if ((i + j) % 2 != 0 && i > 4 && i <= 7)
                    {
                        Gameboard.Instance.RenderImage(false, i * 8+j, myGrid.Width / 10);
                        gridButton.Type = (int)CellTypes.White;
                    }

                    myGrid.Children.Add(gridButton.Content);
                    Grid.SetColumn(gridButton.Content, j);
                    Grid.SetRow(gridButton.Content, i);
                }
            }
            return myGrid;
        }

        

        public void PieceDefocused(object sender, RoutedEventArgs e)
        {
            Button lostButton = (Button)sender;
            int rowIndex = Grid.GetRow(lostButton);
            int colIndex = Grid.GetColumn(lostButton);
            
            bool IsPieceClicked = Gameboard.Instance.GetType(rowIndex, colIndex) == 1 ||
                                    Gameboard.Instance.GetType(rowIndex, colIndex) == 2;
            
            if (IsPieceClicked) lostButton.Style = ButtonStyles.Instance.grayCellStyle;
            

            foreach (GridButton gridButton in Gameboard.Instance.GridButtons)
            {
                //if button is highlighted
                if (Gameboard.Instance.GetType(gridButton.Row, gridButton.Column) == 3) gridButton.Content.Style = ButtonStyles.Instance.grayCellStyle;

                if (IsPieceClicked) gridButton.Content.Opacity = 1;
            }   


        }

        public void CellClicked(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            int rowIndex = Grid.GetRow(clickedButton);
            int colIndex = Grid.GetColumn(clickedButton);
            bool IsPiececlicked = Gameboard.Instance.GetType(rowIndex, colIndex) == 1 ||
                                    Gameboard.Instance.GetType(rowIndex, colIndex) == 2;

            if (IsPiececlicked) clickedButton.Style = ButtonStyles.Instance.clickedCellStyle;

            //the move
            if (Gameboard.Instance.GetType(rowIndex, colIndex) == 3)
            {
                Gameboard.Instance.SetType(rowIndex, colIndex, Gameboard.Instance.TypeActiveButton);

                Gameboard.Instance.RenderImage(Gameboard.Instance.TypeActiveButton == 1, rowIndex * 8 + colIndex, MainGrid.Width / 10);


                 Gameboard.Instance.RemovePiece(Gameboard.Instance.IndexActiveButton);

                if (Gameboard.Instance.IsToAttack) EatPiece(Gameboard.Instance.IndexToBeEaten);

                Gameboard.Instance.PlayerTurn = !Gameboard.Instance.PlayerTurn;
            }

            for (short i = 0; i < 8; i++)
                {
                    for (short j = 0; j < 8; j++)
                    {
                        if (Gameboard.Instance.GetType(i, j) == 3) Gameboard.Instance.SetType(i, j, 0);

                        if (IsPiececlicked) 
                            Gameboard.Instance.GridButtons[i*8+j].Content.Opacity = (i != rowIndex || j != colIndex) ? 0.65 : 1;
                    }
                }
           //предлагать ход только для шашки
            if (IsPiececlicked) SuggestMoves(Gameboard.Instance.GridButtons, rowIndex, colIndex);
        }
        public void SuggestMoves(List<GridButton> gridButtons, int currRow, int currCol)
        {
            Gameboard.Instance.IndexActiveButton = currRow * 8 + currCol;
            Gameboard.Instance.TypeActiveButton = Gameboard.Instance.GetType(currRow * 8 + currCol);
            bool notOutOfTopLeftRange = (currRow - 1 >= 0 && currCol - 1 >= 0);
            bool notOutOfTopRightRange = (currRow - 1 >= 0 && currCol + 1 <= 8);

            bool notOutOfTopLeftAttackRange = (currRow - 2 >= 0 && currCol - 2 >= 0);
            bool notOutOfTopRightAttackRange = (currRow - 2 >= 0 && currCol + 2 <= 8);

            bool notOutOfBottomLeftRange = (currRow + 1 <= 8 && currCol - 1 >= 0);
            bool notOutOfBottomRightRange = (currRow + 1 <= 8 && currCol + 1 <= 8);

            bool notOutOfBottomLeftAttackRange = (currRow + 2 <= 8 && currCol - 2 >= 0);
            bool notOutOfBottomRightAttackRange = (currRow + 2 <= 8 && currCol + 2 <= 8);

            if (notOutOfTopLeftAttackRange && notOutOfTopLeftRange && (Gameboard.Instance.GetType(currRow - 1, currCol - 1) != Gameboard.Instance.TypeActiveButton && Gameboard.Instance.GetType(currRow - 1, currCol - 1) != 0) && Gameboard.Instance.GetType(currRow - 2, currCol - 2) == 0)
            {
                gridButtons[(currRow - 2) * 8 + (currCol - 2)].Content.Style = ButtonStyles.Instance.highlightedCellStyle;
                Gameboard.Instance.SetType(currRow - 2, currCol - 2, 3);

                Gameboard.Instance.IndexToBeEaten = (currRow - 1) * 8 + (currCol - 1);
                Gameboard.Instance.TypeToBeEaten = Gameboard.Instance.GetType(currRow - 1, currCol - 1);

                Gameboard.Instance.IsToAttack = true;

            }
            if (notOutOfTopRightAttackRange && notOutOfTopRightRange && (Gameboard.Instance.GetType(currRow - 1, currCol + 1) != Gameboard.Instance.TypeActiveButton && Gameboard.Instance.GetType(currRow - 1, currCol + 1) != 0) && Gameboard.Instance.GetType(currRow - 2, currCol + 2) == 0)
            {
                gridButtons[(currRow - 2) * 8 + (currCol + 2)].Content.Style = ButtonStyles.Instance.highlightedCellStyle;
                Gameboard.Instance.SetType(currRow - 2, currCol + 2, 3);

                Gameboard.Instance.IndexToBeEaten = (currRow - 1) * 8 + (currCol + 1);
                Gameboard.Instance.TypeToBeEaten = Gameboard.Instance.GetType(currRow - 1, currCol + 1);
                Gameboard.Instance.IsToAttack = true;
            }
            if (notOutOfBottomLeftAttackRange && notOutOfBottomLeftRange && (Gameboard.Instance.GetType(currRow + 1, currCol - 1) != Gameboard.Instance.TypeActiveButton && Gameboard.Instance.GetType(currRow + 1, currCol - 1) != 0) && Gameboard.Instance.GetType(currRow + 2, currCol - 2) == 0)
            {
                gridButtons[(currRow + 2) * 8 + (currCol - 2)].Content.Style = ButtonStyles.Instance.highlightedCellStyle;
                Gameboard.Instance.SetType(currRow + 2, currCol - 2, 3);
                Gameboard.Instance.IndexToBeEaten = (currRow + 1) * 8 + (currCol - 1);
                Gameboard.Instance.TypeToBeEaten = Gameboard.Instance.GetType(currRow + 1, currCol - 1);
                Gameboard.Instance.IsToAttack = true;

            }
            if (notOutOfBottomRightAttackRange && notOutOfBottomRightRange && (Gameboard.Instance.GetType(currRow + 1, currCol + 1) != Gameboard.Instance.TypeActiveButton && Gameboard.Instance.GetType(currRow + 1, currCol + 1) != 0) && Gameboard.Instance.GetType(currRow + 2, currCol + 2) == 0)
            {
                gridButtons[(currRow + 2) * 8 + (currCol + 2)].Content.Style = ButtonStyles.Instance.highlightedCellStyle;
                Gameboard.Instance.SetType(currRow + 2, currCol + 2, 3);

                Gameboard.Instance.IndexToBeEaten = (currRow + 1) * 8 + (currCol + 1);
                Gameboard.Instance.TypeToBeEaten = Gameboard.Instance.GetType(currRow + 1, currCol + 1);

                Gameboard.Instance.IsToAttack = true;
            }

            if (Gameboard.Instance.PlayerTurn)
            {
                //upwards

                bool notOutOfNegRange = (currRow - 1 >= 0 && currCol - 1 >= 0);
                bool notOutOfPosRange = (currRow - 1 >= 0 && currCol + 1 < 8);


                

                //left position is empty and highlighted
                 if (!Gameboard.Instance.IsToAttack && notOutOfNegRange && Gameboard.Instance.GetType(currRow - 1, currCol - 1) == 0)
                {
                    gridButtons[(currRow - 1) * 8 + (currCol - 1)].Content.Style = ButtonStyles.Instance.highlightedCellStyle;
                    Gameboard.Instance.SetType(currRow - 1, currCol - 1, 3);
                }

                if (!Gameboard.Instance.IsToAttack && notOutOfPosRange && Gameboard.Instance.GetType(currRow - 1, currCol + 1) == 0)
                {
                    gridButtons[(currRow - 1) * 8 + (currCol + 1)].Content.Style = ButtonStyles.Instance.highlightedCellStyle;
                    Gameboard.Instance.SetType(currRow - 1, currCol + 1, 3);
                }


            }
            //downwards
            else
            {
                bool notOutOfNegRange = (currRow + 1 < 8 && currCol - 1 >= 0);//left
                bool notOutOfPosRange = (currRow + 1 < 8 && currCol + 1 < 8);//right
               
                //left position is empty and highlighted
                if (!Gameboard.Instance.IsToAttack && notOutOfNegRange && Gameboard.Instance.GetType(currRow + 1, currCol - 1) == 0)
                {
                    gridButtons[(currRow + 1) * 8 + (currCol - 1)].Content.Style = ButtonStyles.Instance.highlightedCellStyle;
                    Gameboard.Instance.SetType(currRow + 1, currCol - 1, 3);

                }//right position is empty and highlighted
                if (!Gameboard.Instance.IsToAttack && notOutOfPosRange && Gameboard.Instance.GetType(currRow + 1, currCol + 1) == 0)
                {
                    gridButtons[(currRow + 1) * 8 + (currCol + 1)].Content.Style = ButtonStyles.Instance.highlightedCellStyle;
                    Gameboard.Instance.SetType(currRow + 1, currCol + 1, 3);
                }

            }

           
        }

        public void EatPiece(int index)
        {
            Gameboard.Instance.IsToAttack = false;
            Gameboard.Instance.RemovePiece(index);
        }

    }
}

