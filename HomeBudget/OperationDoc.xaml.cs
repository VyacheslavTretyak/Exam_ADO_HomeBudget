using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace HomeBudget
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class OperationDoc : Window
	{
		public List<string> accountList;		
		public OperationDoc()
		{
			InitializeComponent();			
			LoadData();
			dpDate.Text = DateTime.Now.ToString();
			tbSum.PreviewTextInput += TbSum_PreviewTextInput;
			btnSave.Click += BtnSave_Click;
			btnCancel.Click += BtnCancel_Click;
			btnAccountAdd.Click += BtnAccountAdd_Click;
			btnAccountEdit.Click += BtnAccountEdit_Click;

			btnOperationAdd.Click += BtnOperationAdd_Click;
			btnOperationEdit.Click += BtnOperationEdit_Click;

			btnPersonAdd.Click += BtnPersonAdd_Click;
			btnPersonEdit.Click += BtnPersonEdit_Click;

			operationToogle.Checked += OperationToogle_Checked;
			operationToogle.Unchecked += OperationToogle_Unchecked;
			operationToogle.IsChecked = true;
			operationToogle.IsChecked = false;

		}

		private void TbSum_PreviewTextInput(object sender, TextCompositionEventArgs e)
		{
			var textBox = sender as TextBox;
			e.Handled = !Regex.IsMatch(textBox.Text+e.Text, @"^(\d+\.?\d*)$");

		}

		private void BtnSave_Click(object sender, RoutedEventArgs e)
		{
			if(cbAccount.SelectedItem == null ||
				cbOperation.SelectedItem == null ||
				cbPerson.SelectedItem == null)
			{
				return;
			}
			using (DataModelContainer db = new DataModelContainer())
			{
				Operation op = new Operation();
				op.Date = DateTime.Parse(dpDate.Text);
				int id = (cbOperation.SelectedItem as OperationName).Id;
				op.OperationName = db.OperationNameSet.FirstOrDefault(o=>o.Id == id);
				id = (cbAccount.SelectedItem as Account).Id;
				op.Account = db.AccountSet.FirstOrDefault(o => o.Id == id);
				if (cbPerson.SelectedItem == null)
				{
					op.Person = null;
				}
				else
				{
					id = (cbPerson.SelectedItem as Person).Id;
					op.Person = db.PersonSet.FirstOrDefault(o => o.Id == id);
				}
				
				string pars = tbSum.Text;
				if (pars == "")
				{
					op.Sum = 0;
				}
				else
				{					
					op.Sum = double.Parse(pars, NumberStyles.Any, CultureInfo.InvariantCulture);
				}
				if (operationToogle.IsChecked == null || operationToogle.IsChecked == false)
				{
					op.Sum *= -1;
				}				
				op.Comment = CommentTextBox.Text;

				db.OperationSet.Add(op);
				db.SaveChanges();
				DialogResult = true;
			}
		}

		private void LoadData()
		{
			using(DataModelContainer db = new DataModelContainer())
			{
				SaverDefaultValue valSaver = new SaverDefaultValue();
				cbAccount.ItemsSource = db.AccountSet.ToList();
				cbOperation.ItemsSource = db.OperationNameSet.ToList();
				cbPerson.ItemsSource = db.PersonSet.ToList();

				if (valSaver.dic.ContainsKey("account"))
				{
					int id = valSaver.dic["account"];
					var val = db.AccountSet.FirstOrDefault(a => a.Id == id);
					if (val != null) {
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
				if (valSaver.dic.ContainsKey("operation"))
				{
					int id = valSaver.dic["operation"];
					var val = db.OperationNameSet.FirstOrDefault(a => a.Id == id);
					if (val != null)
					{
						cbOperation.SelectedItem = val;
					}
					else
					{
						cbOperation.SelectedIndex = 0;
					}
				}
				else
				{
					cbOperation.SelectedIndex = 0;
				}
				if (valSaver.dic.ContainsKey("person"))
				{
					int id = valSaver.dic["person"];
					var val = db.PersonSet.FirstOrDefault(a => a.Id == id);
					if (val != null)
					{
						cbPerson.SelectedItem = val;
					}
					else
					{
						cbPerson.SelectedIndex = 0;
					}
				}
				else
				{
					cbPerson.SelectedIndex = 0;
				}
			}
		}
		private void BtnPersonAdd_Click(object sender, RoutedEventArgs e)
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
					Person val = new Person() { Name = wnd.Value };
					db.PersonSet.Add(val);
					db.SaveChanges();
					cbPerson.ItemsSource = db.PersonSet.ToList();
					cbPerson.SelectedItem = val;
					if (wnd.Default)
					{
						valSaver.dic["person"] = db.PersonSet.FirstOrDefault(p => p.Name == wnd.Value).Id;
						valSaver.Save();
					}
				}
				
			}
		}
		private void BtnPersonEdit_Click(object sender, RoutedEventArgs e)
		{
			SaverDefaultValue valSaver = new SaverDefaultValue();
			if (cbPerson.SelectedItem == null)
			{
				return;
			}
			Person person = cbPerson.SelectedItem as Person;
			EditValue wnd = new EditValue();
			wnd.Value = person.Name;
			wnd.ShowDialog();
			if (wnd.Value == null)
			{
				return;
			}
			if (wnd.DialogResult == true)
			{
				using (DataModelContainer db = new DataModelContainer())
				{
					Person val = db.PersonSet.FirstOrDefault(a => a.Id == person.Id);
					val.Name = wnd.Value;
					db.SaveChanges();
					cbPerson.ItemsSource = db.PersonSet.ToList();
					cbPerson.SelectedItem = val;
					if (wnd.Default)
					{
						valSaver.dic["person"] = val.Id;
						valSaver.Save();
					}
				}
			}
		}


		private void BtnOperationAdd_Click(object sender, RoutedEventArgs e)
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
					OperationName val = new OperationName() { Name = wnd.Value };
					db.OperationNameSet.Add(val);
					db.SaveChanges();
					cbOperation.ItemsSource = db.OperationNameSet.ToList();
					cbOperation.SelectedItem = val;
					if (wnd.Default)
					{
						valSaver.dic["operation"] = db.OperationNameSet.FirstOrDefault(p => p.Name == wnd.Value).Id;
						valSaver.Save();
					}
				}
			}
		}
		private void BtnOperationEdit_Click(object sender, RoutedEventArgs e)
		{
			SaverDefaultValue valSaver = new SaverDefaultValue();
			if (cbOperation.SelectedItem == null)
			{
				return;
			}
			OperationName operationName = cbOperation.SelectedItem as OperationName;
			EditValue wnd = new EditValue();
			wnd.Value = operationName.Name;
			wnd.ShowDialog();
			if (wnd.Value == null)
			{
				return;
			}
			if (wnd.DialogResult == true)
			{
				using (DataModelContainer db = new DataModelContainer())
				{
					OperationName val = db.OperationNameSet.FirstOrDefault(a => a.Id == operationName.Id);
					val.Name = wnd.Value;
					db.SaveChanges();
					cbOperation.ItemsSource = db.OperationNameSet.ToList();
					cbOperation.SelectedItem = val;
					if (wnd.Default)
					{
						valSaver.dic["operation"] = val.Id;
						valSaver.Save();
					}
				}
			}
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

		private void BtnCancel_Click(object sender, RoutedEventArgs e)
		{
			DialogResult = false;
		}

		private void OperationToogle_Unchecked(object sender, RoutedEventArgs e)
		{
			costArrival.Text = "cost";
			costArrival.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD63636"));
			
		}

		private void OperationToogle_Checked(object sender, RoutedEventArgs e)
		{
			costArrival.Text = "arrival";
			costArrival.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00FF23"));
		}
	}
}
