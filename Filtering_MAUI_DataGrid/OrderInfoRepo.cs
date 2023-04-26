using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filtering_MAUI_DataGrid
{
	public class OrderInfoRepo
	{
		private ObservableCollection<OrderInfo> _orders;
		public ObservableCollection<OrderInfo> Orders
		{
			get { return _orders; }
			set { _orders = value; }
		}

		public OrderInfoRepo()
		{
			Orders = new ObservableCollection<OrderInfo>();
			this.GenerateOrders();
		}

		public void GenerateOrders()
		{
			_orders.Add(new OrderInfo("1001", "ALFKI", "Maria Anders", "Germany", "Berlin"));
			_orders.Add(new OrderInfo("1002", "ANATR", "Ana Trujillo", "Mexico", "Mexico D.F."));
			_orders.Add(new OrderInfo("1003", "ANTON", "Ant Fuller", "Mexico", "Mexico D.F."));
			_orders.Add(new OrderInfo("1004", "AROUT", "Thomas Hardy", "UK", "London"));
			_orders.Add(new OrderInfo("1005", "BERGS", "Tim Adams", "UK", "London"));
			_orders.Add(new OrderInfo("1006", "BLAUS", "Hanna Moos", "Germany", "Mannheim"));
			_orders.Add(new OrderInfo("1007", "BLONP", "Andrew Fuller", "France", "Strasbourg"));
			_orders.Add(new OrderInfo("1008", "BOLID", "Martin King", "Spain", "Madrid"));
			_orders.Add(new OrderInfo("1009", "BONAP", "Lenny Lin", "France", "Marsiella"));
			_orders.Add(new OrderInfo("1010", "BOTTM", "John Carter", "Canada", "Lenny Lin"));
			_orders.Add(new OrderInfo("1011", "AROUT", "Laura King", "UK", "London"));
			_orders.Add(new OrderInfo("1012", "BLAUS", "Anne Wilson", "Germany", "Mannheim"));
			_orders.Add(new OrderInfo("1013", "BLONP", "Martin King", "France", "Strasbourg"));
			_orders.Add(new OrderInfo("1014", "AROUT", "Gina Irene", "UK", "London"));
		}
	}
}
