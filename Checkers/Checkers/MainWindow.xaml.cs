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

            Style grayCellStyle = new Style();
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




            Style whiteCellStyle = new Style();
            ControlTemplate whiteTemplateButton = new ControlTemplate(typeof(Button));

            //STARTS HERE <<-- have no idea why it works, but it does 
            FrameworkElementFactory whiteElemFactory = new FrameworkElementFactory(typeof(Border));
            whiteElemFactory.SetBinding(Border.BackgroundProperty, new Binding { RelativeSource = RelativeSource.TemplatedParent, Path = new PropertyPath("Background") });
            whiteTemplateButton.VisualTree = whiteElemFactory;
            whiteElemFactory.AppendChild(new FrameworkElementFactory(typeof(ContentPresenter)));
            //ENDS HERE

            whiteCellStyle.Setters.Add(new Setter { Property = Button.BackgroundProperty, Value = System.Windows.Media.Brushes.White });
            whiteCellStyle.Setters.Add(new Setter { Property = Button.TemplateProperty, Value = whiteTemplateButton });
            Trigger whiteStyleTrigger = new Trigger { Property = Button.IsMouseOverProperty, Value = true };
            whiteCellStyle.Triggers.Add(whiteStyleTrigger);


            for (short i = 0; i < 8; i++)
            {
                myGrid.ColumnDefinitions.Add(new ColumnDefinition());
                myGrid.RowDefinitions.Add(new RowDefinition());
                for (short j = 0; j < 8; j++)
                {
                   
                    Button btn1 = new Button();
                    btn1.Style = ((i + j) % 2 == 0) ? whiteCellStyle : grayCellStyle;
                    

                    if ((i + j) % 2 != 0 && i <= 2)
                    {
                        btn1.Click += new RoutedEventHandler(pieceClicked);
                        btn1.Content = new Image
                        {
                            //TO-DO:
                            //fix the global path to local, it gives more flexibility
                            Width= myGrid.Width / 10,
                            Source = new BitmapImage(new Uri("D:\\kursova\\konstruktOR\\Checkers\\Checkers\\images\\white.png"))
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
                                Source = new BitmapImage(new Uri("D:\\kursova\\konstruktOR\\Checkers\\Checkers\\images\\black.png"))
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
        public void pieceClicked(object a ,EventArgs e)
        {
            MessageBox.Show(Convert.ToString(a));
            foreach (RowDefinition i in mainGrid.RowDefinitions)
            {
                foreach (ColumnDefinition j in mainGrid.ColumnDefinitions)
                {
                    //short rowPos, short colPos
                }
            }
        }

       
        
    }
}

