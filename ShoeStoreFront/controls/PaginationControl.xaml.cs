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

namespace ShoeStoreFront.controls
{
    /// <summary>
    /// Interaction logic for PaginationControl.xaml
    /// </summary>
    public partial class PaginationControl : UserControl
    {
        public event EventHandler ItemsPerPageChanged;
        public event EventHandler PreviousClicked;
        public event EventHandler NextClicked;

        public int ItemPerPage
        {
            get { return (int)GetValue(MiNumeroProperty); }
            set { SetValue(MiNumeroProperty, value); }
        }
        public static readonly DependencyProperty MiNumeroProperty =
        DependencyProperty.Register(nameof(ItemPerPage), typeof(int), typeof(PaginationControl), new PropertyMetadata(0));

       
        public PaginationControl()
        {
            InitializeComponent(); 
            cbItemsPerPage.SelectedIndex = 0;
            loadComboBox();
            ItemPerPage = (int)cbItemsPerPage.SelectedValue; 
            //ItemsPerPageChanged?.Invoke(this, null);
        }

        private void loadComboBox()
        {
            cbItemsPerPage.Items.Add(1);
            cbItemsPerPage.Items.Add(6);
            cbItemsPerPage.Items.Add(12);
            cbItemsPerPage.Items.Add(24);
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            NextClicked?.Invoke(this, null);
        }



        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            PreviousClicked?.Invoke(this, null);
        }

        private void cbItemsPerPage_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //if (cbItemsPerPage.SelectedItem is ComboBoxItem selectedItem)
            //{

            ItemPerPage = (int)cbItemsPerPage.SelectedValue;
            ItemsPerPageChanged?.Invoke(this, null);
            //}
        }
    }
}
