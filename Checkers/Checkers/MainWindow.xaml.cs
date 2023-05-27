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
using System.Data.Common;

namespace Checkers
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 

    public partial class MainWindow : Window
    {
<<<<<<< Updated upstream
=======

        public Grid mainGrid { get; set; }  
>>>>>>> Stashed changes
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }
        public void Init()
        {
<<<<<<< Updated upstream
            // Create the Grid
            Grid myGrid = new Grid();
            myGrid.Width = 800;
            myGrid.Height = 800;
            myGrid.HorizontalAlignment = HorizontalAlignment.Center;
            myGrid.VerticalAlignment = VerticalAlignment.Center;


            // Define the Columns
            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            ColumnDefinition colDef4 = new ColumnDefinition();
            ColumnDefinition colDef5 = new ColumnDefinition();
            ColumnDefinition colDef6 = new ColumnDefinition();
            ColumnDefinition colDef7 = new ColumnDefinition();
            ColumnDefinition colDef8 = new ColumnDefinition();
            myGrid.ColumnDefinitions.Add(colDef1);
            myGrid.ColumnDefinitions.Add(colDef2);
            myGrid.ColumnDefinitions.Add(colDef3);
            myGrid.ColumnDefinitions.Add(colDef4);
            myGrid.ColumnDefinitions.Add(colDef5);
            myGrid.ColumnDefinitions.Add(colDef6);
            myGrid.ColumnDefinitions.Add(colDef7);
            myGrid.ColumnDefinitions.Add(colDef8);

            // Define the Rows
            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();
            RowDefinition rowDef3 = new RowDefinition();
            RowDefinition rowDef4 = new RowDefinition();
            RowDefinition rowDef5 = new RowDefinition();
            RowDefinition rowDef6 = new RowDefinition();
            RowDefinition rowDef7 = new RowDefinition();
            RowDefinition rowDef8 = new RowDefinition();
            RowDefinition rowDef9 = new RowDefinition();
            myGrid.RowDefinitions.Add(rowDef1);
            myGrid.RowDefinitions.Add(rowDef2);
            myGrid.RowDefinitions.Add(rowDef3);
            myGrid.RowDefinitions.Add(rowDef4);
            myGrid.RowDefinitions.Add(rowDef5);
            myGrid.RowDefinitions.Add(rowDef6);
            myGrid.RowDefinitions.Add(rowDef7);
            myGrid.RowDefinitions.Add(rowDef8);
            myGrid.RowDefinitions.Add(rowDef9);
            //написан кринж, переработать для оптимизации
<<<<<<< Updated upstream

            
=======
            //create the black-n-white chess grid with buttons as grid childrens
            BitmapImage whiteCheckersBitmap = new BitmapImage
                (new Uri("images/white.png", UriKind.Relative));
            BitmapImage blackCheckersBitmap = new BitmapImage
                (new Uri("images/black.png", UriKind.Relative));
=======
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
                ControlTemplate whiteTemplateButton = new ControlTemplate(typeof(Button)); ;

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
                Trigger focusStyleTrigger = new Trigger { Property = Button.IsKeyboardFocusedProperty, Value = true };
                clickedStyleTrigger.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.Red });
                focusStyleTrigger.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.Red });

                clickedCellStyle.Triggers.Add(clickedStyleTrigger);
                clickedCellStyle.Triggers.Add(focusStyleTrigger);



                disabledCellStyle = new Style(typeof(Button), whiteCellStyle);
                disabledCellStyle.Setters.Add(new Setter { Property=Button.IsEnabledProperty, Value = false });

                disabledCellStyle.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.LightGray });
                //disabledCellStyle.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.LightGray });



                // Private constructor to prevent direct instantiation
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

            public short[,] gameMatrix { get;set; }

            public gameBoard() {

            gameMatrix = new short[,]
            {
                //whites are 1, blacks are 2, empty cell is zero;
                //supreme pieces will be negative, i.e. -2 is supreme black piece
                {0,1,0,1,0,1,0,1},
                {1,0,1,0,1,0,1,0},
                {0,1,0,1,0,1,0,1},
                {0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0},
                {2,0,2,0,2,0,2,0},
                {0,2,0,2,0,2,0,2},
                {2,0,2,0,2,0,2,0}
            };
            
            }
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
>>>>>>> Stashed changes

          
            for (short i = 0; i < 8; i++)
            {
                for (short j = 0; j < 8; j++)
                {
<<<<<<< Updated upstream
                    //backButton is interactive clickable button
                    //checkersIcon is an image hovering above buttons, is static and non-interactive with the user
                    Button backButton = new Button();
=======

                    Button btn1 = new Button
                    {
                        Style = ((i + j) % 2 == 0) ? whiteCellStyleIN : grayCellStyleIN
                    };

>>>>>>> Stashed changes

                    if ((i + j) % 2 == 0)
                    {
<<<<<<< Updated upstream
                        backButton.Background = System.Windows.Media.Brushes.White; //<-- initially this was made for color of the cell, temporarily is used rn for checkers pieces test
                    
                    }
                    else
                    {
                        backButton.Background = System.Windows.Media.Brushes.Black;

                        if (i < 3)
                        {//working over here -----------------------------------------------------------------------------
                            Image whiteCheckers = new Image();
                            whiteCheckers.Width = 200;
                            whiteCheckers.Source = whiteCheckersBitmap;
                            myGrid.Children.Add(whiteCheckers);
                            Grid.SetColumn(whiteCheckers, j);
                            Grid.SetRow(whiteCheckers, i);
=======
                        btn1.Click += new RoutedEventHandler(pieceClicked);
                        btn1.Content = new Image
                        {
                            //TO-DO:
                            //fix the global path to local, it gives more flexibility
                            Width= myGrid.Width / 10,
                            Source = new BitmapImage(new Uri("D:\\kursova\\Checkers\\Checkers\\images\\white.png"))
                        };
                    }
                    else
                    {
                        if ((i + j) % 2 != 0 && i > 4 && i <= 7)
                        {
                            btn1.Click += new RoutedEventHandler(pieceClicked);
                            btn1.Content = new Image
                            {
                                Width = myGrid.Width / 10,
                                Source = new BitmapImage(new Uri("D:\\kursova\\Checkers\\Checkers\\images\\black.png"))
                            };
>>>>>>> Stashed changes
                        }
                        else if(i>5)
                        {
                            Image blackCheckers = new Image();

                            blackCheckers.Source = blackCheckersBitmap;
                            //myGrid.Children.Add(blackCheckers);
                            //Grid.SetColumn(blackCheckers, j);
                            //Grid.SetRow(blackCheckers, i);
                        }
                    }
                    myGrid.Children.Add(backButton);
                    Grid.SetColumn(backButton, j);
                    Grid.SetRow(backButton, i);
                }
            }
            mainWindow.Content = myGrid;
        }
        public enum Pieces
        {
                WhitePiece,
                BlackPiece,
                WhiteKing,
                BlackKing
        }
        //Я не уверен, что поле делать через сетку кнопок хорошая идея. Типо инициализировали грид как поле, а потом опять инициализировать новый грид для пешек??? Странно
        public void Board()
        {
            // Open a Uri and decode a BMP image
            Uri myUri = new Uri("tulipfarm.bmp", UriKind.RelativeOrAbsolute);
            BmpBitmapDecoder decoder2 = new BmpBitmapDecoder(myUri, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            BitmapSource bitmapSource2 = decoder2.Frames[0];

            // Draw the Image
            Image myImage2 = new Image();
            myImage2.Source = bitmapSource2;
            myImage2.Stretch = Stretch.None;
            myImage2.Margin = new Thickness(20);
>>>>>>> Stashed changes


            BitmapImage whiteCheckers = new BitmapImage
                (new Uri("images\\white.png", UriKind.Relative));
            BitmapImage blackCheckers = new BitmapImage
                (new Uri("images\\black.png", UriKind.Relative));


            ImageBrush whiteCheckersBrush = new ImageBrush(whiteCheckers);
            ImageBrush blackCheckersBrush = new ImageBrush(blackCheckers);
            //create the black-n-white chess grid with buttons as grid childrens
            for (short i = 0; i < 8; i++)
            {
                for (short j = 0; j < 8; j++)
                {
                    Button btn1 = new Button();
                    
                    if((i+j) % 2 == 0)
                    {
                        btn1.Background = whiteCheckersBrush; //<-- initially this was made for color of the cell, temporarily is used rn for checkers pieces test

                    }
                    else
                    {
                        btn1.Background = blackCheckersBrush;
                    }
                    myGrid.Children.Add(btn1);
                    Grid.SetColumn(btn1, j);
                    Grid.SetRow(btn1, i);
                }
            }
            mainWindow.Content = myGrid;

        }
<<<<<<< Updated upstream
        
=======

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

        //--------CONSTRUCTION ZONE-----------//
        public void pieceClicked(object sender ,EventArgs e)
        {
            Button clickedButton = (Button)sender;

            clickedButton.Style = buttonStyles.Instance.clickedCellStyle;

            int rowIndex = Grid.GetRow(clickedButton);
            int colIndex = Grid.GetColumn(clickedButton);


            // Store the references to the child elements to be removed
            List<UIElement> elementsToRemove = new List<UIElement>();

            // Find the child elements to remove
            foreach (UIElement child in mainGrid.Children)
            {
                int childRowIndex = Grid.GetRow(child);
                int childColIndex = Grid.GetColumn(child);

                if (childRowIndex != rowIndex || childColIndex != colIndex)
                {
                    elementsToRemove.Add(child);
                }
                else
                {
                    MessageBox.Show("Try to");
                }
            }
            foreach (UIElement element in elementsToRemove)
            {
                mainGrid.Children.Remove(element);
            }


            MessageBox.Show(Convert.ToString(rowIndex));
            MessageBox.Show(Convert.ToString(colIndex));
            for (short i = 0; i < 8; i++)
            {
                for (short j = 0; j < 8; j++)
                {
                    if (i != rowIndex || j != colIndex)
                    {
                        Button disabledButton = new Button();
                        disabledButton.Content = mainGrid.Children[i * 8 + j];
                        disabledButton.Style = buttonStyles.Instance.disabledCellStyle;
                        mainGrid.Children.Insert(i * 8 + j, disabledButton);
                    }
                    else
                    {//the problem is right here
                        MessageBox.Show("Epic");
                    }

                }
            }

        }
>>>>>>> Stashed changes
    }
}
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
