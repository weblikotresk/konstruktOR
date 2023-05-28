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

        public List<Button> buttonList = new List<Button>();
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }
        public void Init()
        {
            mainGrid = createGrid();
            mainWindow.Content = mainGrid;
        }


        public class buttonStyles
        {
            private static buttonStyles instance;

            public Style grayCellStyle { get; set; }
            public Style whiteCellStyle { get; set; }
            public Style clickedCellStyle { get; set; }
            public Style disabledCellStyle { get; set; }


            private buttonStyles()
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
                //clickedCellStyle.Triggers.Add(focusStyleTrigger);


                disabledCellStyle = new Style(typeof(Button), whiteCellStyle);
                disabledCellStyle.Setters.Add(new Setter { Property = Button.IsEnabledProperty, Value = false });
                disabledCellStyle.Setters.Add(new Setter { Property = Button.OpacityProperty, Value = 0.25});
            }

            public static buttonStyles Instance
            {
                get
                {
                    if (instance == null)
                        instance = new buttonStyles();
                    return instance;
                }
            }
        }


        public class gameBoard
        {
            private static gameBoard instance;

            public short[,] gameMatrix { get; set; }

            public gameBoard()
            {

                gameMatrix = new short[,]
                {
                //blacks are 1, whites are 2, empty cell is zero;
                //supreme pieces will be negative, i.e. -1 is supreme black piece

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
             1. ход на пустое место

                [i+1][j+1]=0 || [i-1][j-1]=0
            невозможность нажатия на пустые кнопки, кроме предлагаемых


            2. ход через шашку
             
             
             */


            public static gameBoard Instance
            {
                get
                {
                    if (instance == null)
                        instance = new gameBoard();
                    return instance;
                }
            }
        }


        public Grid createGrid()
        {
            // Create the Grid
            Grid myGrid = new Grid() {
                Width = 400,
                Height=400,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,

            };

            //to get rid of default hover behavior, we need to assign our own button style
            Style grayCellStyleIN = buttonStyles.Instance.grayCellStyle;
            Style whiteCellStyleIN = buttonStyles.Instance.whiteCellStyle;

            for (short i = 0; i < 8; i++)
            {
                myGrid.ColumnDefinitions.Add(new ColumnDefinition());
                myGrid.RowDefinitions.Add(new RowDefinition());
                for (short j = 0; j < 8; j++)
                {
                   
                    Button btn1 = new Button();
                    buttonList.Add(btn1);
                    buttonList[i * 8 + j].Style = ((i + j) % 2 == 0) ? whiteCellStyleIN : grayCellStyleIN;
                    

                    if ((i + j) % 2 != 0 && i <= 2)
                    {
                        buttonList[i * 8 + j].Click += new RoutedEventHandler(pieceClicked);
                        buttonList[i * 8 + j].LostFocus += new RoutedEventHandler(pieceDefocused);
                        buttonList[i * 8 + j].Content = new Image
                        {
                            //TO-DO:
                            //fix the global path to local, it gives more flexibility
                            Width= myGrid.Width / 10,
                            Source = new BitmapImage(new Uri("C:\\Users\\space\\Downloads\\konstruktOR-kursova-working\\konstruktOR-kursova-working\\Checkers\\Checkers\\images\\black.png"))
                        };
                    }
                    else
                    {
                        if ((i + j) % 2 != 0 && i > 4 && i <= 7)
                        {
                            buttonList[i * 8 + j].Click += new RoutedEventHandler(pieceClicked);
                            buttonList[i * 8 + j].LostFocus += new RoutedEventHandler(pieceDefocused);
                            buttonList[i * 8 + j].Content = new Image
                            {
                                Width = myGrid.Width / 10,
                                Source = new BitmapImage(new Uri("C:\\Users\\space\\Downloads\\konstruktOR-kursova-working\\konstruktOR-kursova-working\\Checkers\\Checkers\\images\\white.png"))
                            };
                        }
                    }
                    myGrid.Children.Add(buttonList[i * 8 + j]);
                    Grid.SetColumn(buttonList[i * 8 + j], j);
                    Grid.SetRow(buttonList[i * 8 + j], i);
                }
            }

           
            return myGrid;
        }



        UIElement GetGridElement(Grid g, int r, int c)
        {
            for (int i = 0; i < g.Children.Count; i++)
            {
                UIElement e = g.Children[i];
                if (Grid.GetRow(e) == r && Grid.GetColumn(e) == c)
                    return e;
            }
            return null;
        }

        public void pieceDefocused(object sender, RoutedEventArgs e)
        {
            Button lostButton = (Button)sender;
            lostButton.Style = buttonStyles.Instance.grayCellStyle;
            for (short i = 0; i < 8; i++)
            {
                for (short j = 0; j < 8; j++)
                {
                    buttonList[i * 8 + j].Opacity =  1;
                }
            }
        }

        public void pieceClicked(object sender, EventArgs e)
        {
            Button clickedButton = (Button)sender;

            clickedButton.Style = buttonStyles.Instance.clickedCellStyle;

            int rowIndex = Grid.GetRow(clickedButton);
            int colIndex = Grid.GetColumn(clickedButton);

            for (short i = 0; i < 8; i++)
            {
                for (short j = 0; j < 8; j++)
                {
                    buttonList[i * 8 + j].Opacity = (i != rowIndex || j != colIndex) ? 0.65 : 1;
                }
            }

            makeMove(gameBoard.Instance.gameMatrix, rowIndex, colIndex);

        }


        //work in progress down below
        public void makeMove(short [,] gameMatrix, int currRow, int currCol)
        {
            bool notOutOfPosRange = (currRow + 1 < 8 && currCol + 1 < 8);
            bool notOutOfNegRange = (currRow - 1 > 8 && currCol - 1 > 8);

            if(notOutOfNegRange && gameMatrix[currRow - 1, currCol - 1] == 0)
            {
                buttonList[(currRow-1) * 8 + (currCol-1)].Background = System.Windows.Media.Brushes.Blue;
            }

            if (notOutOfPosRange && gameMatrix[currRow + 1, currCol + 1] == 0)
            {
                buttonList[(currRow+1) * 8 + (currCol+1)].Background = System.Windows.Media.Brushes.Blue;
            }

            for (short i = 0; i < 8; i++)
            {
                for (short j = 0; j < 8; j++)
                {
                    if (gameMatrix[i,j] == 0)
                    {
                        buttonList[i*8+j].Style = buttonStyles.Instance.disabledCellStyle;
                    }
                    
                }
            }
        }


    }
}

