using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeBudget
{
	class SaverDefaultValue
	{
		private string path = "default.ini";
		public Dictionary<string, int> dic;
		public SaverDefaultValue()
		{
			Load();
		}
		public void Load()
		{
			dic = new Dictionary<string, int>();
			FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
			using (StreamReader reader = new StreamReader(fs))
			{
				while (!reader.EndOfStream)
				{
					var arr = reader.ReadLine().Split(':');
					dic.Add(arr[0], int.Parse(arr[1]));
				}
			}
		}
		public void Save()
		{
			if(dic.Count == 0)
			{
				return;
			}
			using (StreamWriter writer = new StreamWriter(path))
			{
				foreach(var val in dic)
				{
					writer.WriteLine($"{val.Key}:{val.Value}");
				}
			}
		}
	}
}
