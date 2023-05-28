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
                disabledCellStyle.Setters.Add(new Setter { Property = Button.IsEnabledProperty, Value = false });

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

            public short[,] gameMatrix { get; set; }

            public gameBoard()
            {

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



            for (short i = 0; i < 8; i++)
            {
                myGrid.ColumnDefinitions.Add(new ColumnDefinition());
                myGrid.RowDefinitions.Add(new RowDefinition());
                for (short j = 0; j < 8; j++)
                {
                   
                    Button btn1 = new Button();
                    btn1.Style = ((i + j) % 2 == 0) ? whiteCellStyleIN : grayCellStyleIN;
                    

                    if ((i + j) % 2 != 0 && i <= 2)
                    {
                        btn1.Click += new RoutedEventHandler(pieceClicked);
                        btn1.Content = new Image
                        {
                            //TO-DO:
                            //fix the global path to local, it gives more flexibility
                            Width= myGrid.Width / 10,
                            Source = new BitmapImage(new Uri("C:\\Users\\space\\Downloads\\konstruktOR-kursova-working\\konstruktOR-kursova-working\\Checkers\\Checkers\\images\\white.png"))
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
                                Source = new BitmapImage(new Uri("C:\\Users\\space\\Downloads\\konstruktOR-kursova-working\\konstruktOR-kursova-working\\Checkers\\Checkers\\images\\black.png"))
                            };
                        }
                    }
                    myGrid.Children.Add(btn1);
                    Grid.SetColumn(btn1, j);
                    Grid.SetRow(btn1, i);
                }
            }

           
            return myGrid;
        }
        //--------CONSTRUCTION ZONE-----------//

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

        public void pieceClicked(object sender, EventArgs e)
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


    }
}

