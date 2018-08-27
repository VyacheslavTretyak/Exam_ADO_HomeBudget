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
using System.Windows.Shapes;

namespace HomeBudget
{
    /// <summary>
    /// Interaction logic for EditValue.xaml
    /// </summary>
    public partial class EditValue : Window
    {
		public string Value { get; set; }
		public bool Default { get; set; }
        public EditValue()
        {
            InitializeComponent();
			defToogle.Checked += DefToogle_Checked;
			defToogle.Unchecked += DefToogle_Unchecked; ;
			btnAccept.Click += BtnAccept_Click;
        }

		private void DefToogle_Unchecked(object sender, RoutedEventArgs e)
		{
			Default = false;
		}

		private void DefToogle_Checked(object sender, RoutedEventArgs e)
		{
			Default = true;
		}

		public new bool? ShowDialog()
		{
			tbName.Text = Value;
			return base.ShowDialog();
		}

		private void BtnAccept_Click(object sender, RoutedEventArgs e)
		{
			Value = tbName.Text;
			DialogResult = true;
		}
	}
}
