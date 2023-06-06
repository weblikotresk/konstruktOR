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
        public MainWindow()
        {
            InitializeComponent();
            Init();
        }
        //entry point for the program
        public void Init()
        {
            Gameboard Gameboard = new Gameboard();
            MainGrid = Gameboard.CreateGrid();
            mainWindow.Content = MainGrid;
            
        }
    }
}

