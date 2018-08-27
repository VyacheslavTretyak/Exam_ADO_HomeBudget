using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
	/// Interaction logic for AccountBalance.xaml
	/// </summary>
	public class GridRow
	{		
		public int Id { get; set; }
		public DateTime Date { get; set; }
		public string Operation { get; set; }
		public double Sum { get; set; }		
		public string Person { get; set; }
		public string Comment{ get; set; }		
	}
	public partial class AccountBalance : Window
	{	
		public Account Account { get; set; }
		public AccountBalance()
		{
			InitializeComponent();			
		}
		public new void ShowDialog()
		{
			LoadGrid();
			base.ShowDialog();
		}

		private void LoadGrid()
		{
			if(Account == null)
			{
				return;
			}
			
			using (DataModelContainer db = new DataModelContainer()) {
				var grid = (from o in db.OperationSet
						join n in db.OperationNameSet on o.OperationName.Id equals n.Id
						join a in db.AccountSet on o.Account.Id equals a.Id
						join p in db.PersonSet on o.Person.Id equals p.Id
						where a.Id == Account.Id
						select new GridRow { Id = o.Id, Date = o.Date, Operation = n.Name, Person = p.Name, Sum = o.Sum, Comment = o.Comment }).ToList();
			
				dataGrid.ItemsSource = grid;
			}
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			YesNo wnd = new YesNo();
			wnd.ShowDialog();
			if (wnd.DialogResult == true)
			{
				GridRow row = dataGrid.SelectedItem as GridRow;
				using(DataModelContainer db = new DataModelContainer())
				{
					db.OperationSet.Remove(db.OperationSet.FirstOrDefault(o => o.Id == row.Id));
					db.SaveChanges();
					LoadGrid();
				}
			}
		}
	}
}
