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
        public Grid mainGrid { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            Init();
        }
        public void Init()
        {
            mainGrid = CreateGrid();
            mainWindow.Content = mainGrid;
        }


        public class ButtonStyles
        {
            private static ButtonStyles instance;

            public Style grayCellStyle { get; set; }
            public Style whiteCellStyle { get; set; }
            public Style clickedCellStyle { get; set; }

            public Style highlightedCellStyle { get; set; }
            // public Style disabledCellStyle { get; set; }


            private ButtonStyles()
            {
                grayCellStyle = new Style();
                ControlTemplate templateButton = new ControlTemplate(typeof(Button));

                //STARTS HERE <<-- have no idea why it works, but it does 
                FrameworkElementFactory elemFactory = new FrameworkElementFactory(typeof(Border));
                elemFactory.SetBinding(Border.BackgroundProperty, new Binding { RelativeSource = RelativeSource.TemplatedParent, Path = new PropertyPath("Background") });
                templateButton.VisualTree = elemFactory;
                elemFactory.AppendChild(new FrameworkElementFactory(typeof(ContentPresenter)));
                //ENDS HERE

                grayCellStyle.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.Gray });
                grayCellStyle.Setters.Add(new Setter { Property = Button.TemplateProperty, Value = templateButton });
                Trigger styleTrigger = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
                Trigger focusTrigger = new Trigger { Property = Button.IsKeyboardFocusedProperty, Value = true };
                styleTrigger.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.NavajoWhite });
                focusTrigger.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.NavajoWhite });
                grayCellStyle.Triggers.Add(styleTrigger);
                grayCellStyle.Triggers.Add(focusTrigger);




                whiteCellStyle = new Style(typeof(Button));
                ControlTemplate whiteTemplateButton = new ControlTemplate(typeof(Button));

                ////STARTS HERE <<-- have no idea why it works, but it does 
                FrameworkElementFactory whiteElemFactory = new FrameworkElementFactory(typeof(Border));
                whiteElemFactory.SetBinding(Border.BackgroundProperty, new Binding { RelativeSource = RelativeSource.TemplatedParent, Path = new PropertyPath("Background") });
                whiteTemplateButton.VisualTree = whiteElemFactory;
                whiteElemFactory.AppendChild(new FrameworkElementFactory(typeof(ContentPresenter)));
                ////ENDS HERE

                whiteCellStyle.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.White });
                whiteCellStyle.Setters.Add(new Setter { Property = Button.TemplateProperty, Value = whiteTemplateButton });
                Trigger whiteStyleTrigger = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
                whiteCellStyle.Triggers.Add(whiteStyleTrigger);



                clickedCellStyle = new Style(typeof(Button), grayCellStyle);
                Trigger clickedStyleTrigger = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
                Trigger keyFocusStyleTrigger = new Trigger { Property = Button.IsKeyboardFocusedProperty, Value = true };
                //Trigger focusStyleTrigger = new Trigger { Property = Button.IsFocusedProperty, Value = true };


                //focusStyleTrigger.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.Red });
                clickedStyleTrigger.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.Red });
                keyFocusStyleTrigger.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.Red });



                clickedCellStyle.Triggers.Add(clickedStyleTrigger);
                clickedCellStyle.Triggers.Add(keyFocusStyleTrigger);





                highlightedCellStyle = new Style(typeof(Button), grayCellStyle);
                highlightedCellStyle.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.Blue });
                Trigger clickedStyleTrigger1 = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
                Trigger keyFocusStyleTrigger1 = new Trigger { Property = Button.IsKeyboardFocusedProperty, Value = true };

                clickedStyleTrigger1.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.Yellow });
                keyFocusStyleTrigger1.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.Yellow });

                highlightedCellStyle.Triggers.Add(clickedStyleTrigger1);
                highlightedCellStyle.Triggers.Add(keyFocusStyleTrigger1);

            }

            public static ButtonStyles Instance
            {
                get
                {
                    if (instance == null)
                        instance = new ButtonStyles();
                    return instance;
                }
            }
        }


        public class ActiveButton
        {
            public short Row;
            public short Column;
            public short Type;
        }

        public class Buttons
        {
            private static Buttons instance;

            public List<Button> buttonElements;

            public ActiveButton ActiveButton;
            
            public void RenderPieceInButton(bool isBlack, int index, double width)
            {
                buttonElements[index].Content = new Image
                {
                    Width = width,
                    Source =
                    isBlack ? new BitmapImage(new Uri("C:\\Users\\space\\Downloads\\konstruktOR-kursova-working\\konstruktOR-kursova-working\\Checkers\\Checkers\\images\\black.png")) : new BitmapImage(new Uri("C:\\Users\\space\\Downloads\\konstruktOR-kursova-working\\konstruktOR-kursova-working\\Checkers\\Checkers\\images\\white.png"))
                };
            }

            public void RemoveActivePiece()
            {
                buttonElements[ActiveButton.Row * 8 + ActiveButton.Column].Content = null;
                GameBoard.Instance.GameMatrix[ActiveButton.Row, ActiveButton.Column] = 0;
            }


            public Buttons()
            {
                buttonElements = new List<Button>();
                ActiveButton = new ActiveButton();

            }


            public static Buttons Instance
            {
                get
                {
                    if (instance == null)
                        instance = new Buttons();
                    return instance;
                }
            }
        }

        public class GameBoard
        {
            private static GameBoard instance;

            public short[,] GameMatrix { get; set; }


           

            public GameBoard()
            {

                GameMatrix = new short[,]
                {
                //blacks are 1, whites are 2, empty cell is zero;
                //supreme pieces will be negative, i.e. -1 is supreme black piece
                // highlighted empty cells are 3, source of highlight is +3, i.e. 5 - white source of highlight
                {0,1,0,1,0,1,0,1},
                {1,0,1,0,1,0,1,0},
                {0,1,0,1,0,1,0,1},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {2,0,2,0,2,0,2,0},
                {0,2,0,2,0,2,0,2},
                {2,0,2,0,2,0,2,0}};



            }
            //i= rows, j=cols


            //виды ходов:
            /*
             +1. ход на пустое место

                [i+1][j+1]=0 || [i-1][j-1]=0
            невозможность нажатия на пустые кнопки, кроме предлагаемых


            2. ход через шашку
             
             
             */


            public static GameBoard Instance
            {
                get
                {
                    if (instance == null)
                        instance = new GameBoard();
                    return instance;
                }
            }
        }


        public Grid CreateGrid()
        {
            // Create the Grid
            Grid myGrid = new Grid() {
                Width = 400,
                Height = 400,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,

            };

            //to get rid of default hover behavior, we need to assign our own button style
            for (short i = 0; i < 8; i++)
            {
                myGrid.ColumnDefinitions.Add(new ColumnDefinition());
                myGrid.RowDefinitions.Add(new RowDefinition());
                for (short j = 0; j < 8; j++)
                {

                    Button btn1 = new Button();
                    Buttons.Instance.buttonElements.Add(btn1);
                    Buttons.Instance.buttonElements[i * 8 + j].Style = ((i + j) % 2 == 0) ? ButtonStyles.Instance.whiteCellStyle : ButtonStyles.Instance.grayCellStyle;
                    Buttons.Instance.buttonElements[i * 8 + j].Click += new RoutedEventHandler(CellClicked);
                    Buttons.Instance.buttonElements[i * 8 + j].LostFocus += new RoutedEventHandler(PieceDefocused);

                    if ((i + j) % 2 != 0 && i <= 2)
                    {//black
                        Buttons.Instance.RenderPieceInButton(true, i * 8 + j, myGrid.Width / 10);
                    }
                    else if ((i + j) % 2 != 0 && i > 4 && i <= 7)
                    {//white
                        Buttons.Instance.RenderPieceInButton(false,i * 8+j, myGrid.Width / 10);

                    }

                    myGrid.Children.Add(Buttons.Instance.buttonElements[i * 8 + j]);
                    Grid.SetColumn(Buttons.Instance.buttonElements[i * 8 + j], j);
                    Grid.SetRow(Buttons.Instance.buttonElements[i * 8 + j], i);
                }
            }


            return myGrid;
        }

        

        public void PieceDefocused(object sender, RoutedEventArgs e)
        {

            Button lostButton = (Button)sender;
            int rowIndex = Grid.GetRow(lostButton);
            int colIndex = Grid.GetColumn(lostButton);
            
            bool IsCellClicked = GameBoard.Instance.GameMatrix[rowIndex, colIndex] == 1 ||
                                    GameBoard.Instance.GameMatrix[rowIndex, colIndex] == 2;

            if(IsCellClicked) lostButton.Style = ButtonStyles.Instance.grayCellStyle;
            foreach (Button buttonChild in Buttons.Instance.buttonElements)
            {   //if button is highlighted
                if (GameBoard.Instance.GameMatrix[Grid.GetRow(buttonChild), Grid.GetColumn(buttonChild)] == 3)
                {
                    //GameBoard.Instance.gameMatrix[Grid.GetRow(buttonChild), Grid.GetColumn(buttonChild)] = 0;
                    buttonChild.Style = ButtonStyles.Instance.grayCellStyle;
                }

                if (IsCellClicked) buttonChild.Opacity = 1;
            }


        }

        public void CellClicked(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;
            int rowIndex = Grid.GetRow(clickedButton);
            int colIndex = Grid.GetColumn(clickedButton);
            bool IsCellClicked = GameBoard.Instance.GameMatrix[rowIndex, colIndex] == 1 ||
                                    GameBoard.Instance.GameMatrix[rowIndex, colIndex] == 2;

            if (IsCellClicked) clickedButton.Style = ButtonStyles.Instance.clickedCellStyle;


            if (GameBoard.Instance.GameMatrix[rowIndex, colIndex] == 3)
            {
                GameBoard.Instance.GameMatrix[rowIndex, colIndex] = Buttons.Instance.ActiveButton.Type;

                Buttons.Instance.RenderPieceInButton(Buttons.Instance.ActiveButton.Type == 1, rowIndex * 8 + colIndex, mainGrid.Width/10);

                Buttons.Instance.RemoveActivePiece();
            }

            for (short i = 0; i < 8; i++)
                {
                    for (short j = 0; j < 8; j++)
                    {
                        if (GameBoard.Instance.GameMatrix[i, j] == 3)
                        {
                        GameBoard.Instance.GameMatrix[i, j] = 0;
                        }

                        if (IsCellClicked) 
                            Buttons.Instance.buttonElements[i * 8 + j].Opacity = (i != rowIndex || j != colIndex) ? 0.65 : 1;
                    }
                }
           //предлагать ход только для шашки
            if (IsCellClicked) SuggestMoves(GameBoard.Instance.GameMatrix, rowIndex, colIndex);
        }

        public void SuggestMoves(short [,] gameMatrix, int currRow, int currCol)
        {
            bool notOutOfPosRange = (currRow - 1 > 0 && currCol + 1 < 8);
            bool notOutOfNegRange = (currRow - 1 > 0 && currCol - 1 >= 0);

            Buttons.Instance.ActiveButton.Row=(short)currRow;
            Buttons.Instance.ActiveButton.Column = (short)currCol;
            Buttons.Instance.ActiveButton.Type = gameMatrix[currRow, currCol];
            //left position is empty and highlighted
            if (notOutOfNegRange && gameMatrix[currRow - 1, currCol - 1] == 0)
            {
                Buttons.Instance.buttonElements[(currRow - 1) * 8 + (currCol - 1)].Style = ButtonStyles.Instance.highlightedCellStyle;
                gameMatrix[currRow - 1, currCol - 1] = 3;

            }
            //right position is empty and highlighted
            if (notOutOfPosRange && gameMatrix[currRow - 1, currCol + 1] == 0)
            {
                Buttons.Instance.buttonElements[(currRow - 1) * 8 + (currCol + 1)].Style = ButtonStyles.Instance.highlightedCellStyle;
                gameMatrix[currRow - 1, currCol + 1] = 3;
            }
        }


    }
}

