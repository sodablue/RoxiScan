using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RoxiScan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }

        private void Initialize()
        {
            if (this.DataContext == null) this.DataContext = new ViewModel();
        }

        private void ScanButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext == null) this.DataContext = new ViewModel();
            var viewModel = this.DataContext as ViewModel;

            viewModel.Scan();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (this.DataContext == null) this.DataContext = new ViewModel();
            var viewModel = this.DataContext as ViewModel;

            viewModel.GetScannerList();            
        }
    }
}
