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
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{		
		public MainWindow()
		{
			InitializeComponent();
			LoadData();
			cbAccount.DropDownClosed += CbAccount_DropDownClosed; ;

			btnSum.Click += BtnSum_Click;
			btnOperationNew.Click += BtnOperationNew_Click;
			btnAccountAdd.Click += BtnAccountAdd_Click;
			btnAccountEdit.Click += BtnAccountEdit_Click;
		}

		private void BtnSum_Click(object sender, RoutedEventArgs e)
		{
			AccountBalance balanceWnd = new AccountBalance();
			balanceWnd.Account = cbAccount.SelectedItem as Account;
			balanceWnd.ShowDialog();
			LoadSum(cbAccount.SelectedItem as Account);
		}

		private void CbAccount_DropDownClosed(object sender, EventArgs e)
		{
			LoadSum(cbAccount.SelectedItem as Account);
		}

		private void LoadData()
		{
			SaverDefaultValue valSaver = new SaverDefaultValue();
			using (DataModelContainer db = new DataModelContainer())
			{
				cbAccount.ItemsSource = db.AccountSet.ToList();
				if (valSaver.dic.ContainsKey("account"))
				{
					int id = valSaver.dic["account"];
					var val = db.AccountSet.FirstOrDefault(a => a.Id == id);
					if (val != null)
					{
						cbAccount.SelectedItem = val;
					}
					else
					{
						cbAccount.SelectedIndex = 0;
					}
				}
				else
				{
					cbAccount.SelectedIndex = 0;
				}
			}
			LoadSum(cbAccount.SelectedItem as Account);
		}

		private void LoadSum(Account account)
		{
			if(account == null)
			{
				tblSum.Text = $"0 grn.";
				return;
			}
			using (DataModelContainer db = new DataModelContainer())
			{
				var entry = db.OperationSet.Where(o => o.Account.Id == account.Id).ToList();
				double sum = 0;
				if (entry.Count != 0) { 
					sum = entry.Sum(o => o.Sum);
				}
				tblSum.Text = $"{sum.ToString("N0")} grn.";
			}
		}

		private void BtnOperationNew_Click(object sender, RoutedEventArgs e)
		{
			OperationDoc doc = new OperationDoc();
			bool? result = doc.ShowDialog();
			LoadData();
			
		}
		private void BtnAccountAdd_Click(object sender, RoutedEventArgs e)
		{
			SaverDefaultValue valSaver = new SaverDefaultValue();
			EditValue wnd = new EditValue();
			wnd.ShowDialog();
			if (wnd.Value == null)
			{
				return;
			}
			if (wnd.DialogResult == true)
			{
				using (DataModelContainer db = new DataModelContainer())
				{
					Account val = new Account() { Name = wnd.Value };
					db.AccountSet.Add(val);
					db.SaveChanges();
					cbAccount.ItemsSource = db.AccountSet.ToList();
					cbAccount.SelectedItem = val;
					if (wnd.Default)
					{
						valSaver.dic["account"] = db.AccountSet.FirstOrDefault(p => p.Name == wnd.Value).Id;
						valSaver.Save();
					}
				}
			}
		}
		private void BtnAccountEdit_Click(object sender, RoutedEventArgs e)
		{
			SaverDefaultValue valSaver = new SaverDefaultValue();
			if (cbAccount.SelectedItem == null)
			{
				return;
			}
			Account account = cbAccount.SelectedItem as Account;
			EditValue wnd = new EditValue();
			wnd.Value = account.Name;
			wnd.ShowDialog();
			if (wnd.Value == null)
			{
				return;
			}
			if (wnd.DialogResult == true)
			{
				using (DataModelContainer db = new DataModelContainer())
				{
					Account val = db.AccountSet.FirstOrDefault(a => a.Id == account.Id);
					val.Name = wnd.Value;
					db.SaveChanges();
					cbAccount.ItemsSource = db.AccountSet.ToList();
					cbAccount.SelectedItem = val;
					if (wnd.Default)
					{
						valSaver.dic["account"] = val.Id;
						valSaver.Save();
					}
				}
			}
		}
	}
}
