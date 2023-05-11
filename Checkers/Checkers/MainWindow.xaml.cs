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
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }
        public void Init()
        {
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

          
            for (short i = 0; i < 8; i++)
            {
                for (short j = 0; j < 8; j++)
                {
                    //backButton is interactive clickable button
                    //checkersIcon is an image hovering above buttons, is static and non-interactive with the user
                    Button backButton = new Button();

                    if ((i + j) % 2 == 0)
                    {
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
        
    }
}
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
