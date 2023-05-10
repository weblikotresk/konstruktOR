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
            myGrid.RowDefinitions.Add(rowDef1);
            myGrid.RowDefinitions.Add(rowDef2);
            myGrid.RowDefinitions.Add(rowDef3);
            myGrid.RowDefinitions.Add(rowDef4);
            myGrid.RowDefinitions.Add(rowDef5);
            myGrid.RowDefinitions.Add(rowDef6);
            myGrid.RowDefinitions.Add(rowDef7);
            myGrid.RowDefinitions.Add(rowDef8);
            //написан кринж, переработать для оптимизации

            


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
