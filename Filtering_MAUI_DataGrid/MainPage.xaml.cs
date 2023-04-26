using Syncfusion.Maui.Data;
using System.Linq;

namespace Filtering_MAUI_DataGrid;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
		InitializeComponent();
	}

	public string FilterText = string.Empty;

	private void FilterTextChanged(object sender, TextChangedEventArgs e)
	{
		if (e.NewTextValue == null)
		{
			this.FilterText = string.Empty;
		}
		else
		{
			this.FilterText = e.NewTextValue;
		}
	}

	public void OnFilterChanged()
	{
		if (this.dataGrid!.View != null)
		{
			this.dataGrid.View.Filter = this.FilterRecords;
			this.dataGrid.View.RefreshFilter();
		}
	}

	public bool FilterRecords(object record)
	{
		OrderInfo orderInfo = record as OrderInfo;

		if (orderInfo != null)
		{
			if (columns.SelectedItem != null)
			{
				if (columns.SelectedItem.ToString() == "All columns")
				{
					if (conditions.SelectedItem != null)
					{
						if (conditions.SelectedItem.ToString() == "Contains")
						{
							var filterText = FilterText.ToLower();
							if (orderInfo.OrderID.ToString().ToLower().Contains(filterText) ||
								orderInfo.CustomerID.ToLower().Contains(filterText) ||
								orderInfo.Customer.ToLower().Contains(filterText) ||
								orderInfo.ShipCountry.ToLower().Contains(filterText) ||
								orderInfo.ShipCity.ToLower().Contains(filterText))
								return true;
							return false;
						}
						else if (conditions.SelectedItem.ToString() == "Equals")
						{
							if (FilterText.Equals(orderInfo.OrderID.ToString()) ||
								FilterText.Equals(orderInfo.CustomerID) ||
								FilterText.Equals(orderInfo.Customer) ||
								FilterText.Equals(orderInfo.ShipCountry) ||
								FilterText.Equals(orderInfo.ShipCity))
								return true;
							return false;
						}
						else
						{
							if (!FilterText.Equals(orderInfo.OrderID.ToString()) ||
							   !FilterText.Equals(orderInfo.CustomerID) ||
							   !FilterText.Equals(orderInfo.Customer) ||
							   !FilterText.Equals(orderInfo.ShipCountry) ||
							   !FilterText.Equals(orderInfo.ShipCity))
								return true;
							return false;
						}
					}
				}
				else
				{
					var property = record.GetType().GetProperty(columns.SelectedItem.ToString());
					var exactValue = property.GetValue(record, null);
					if (conditions.SelectedItem != null)
					{
						if (conditions.SelectedItem.ToString() == "Contains")
						{
							return exactValue.ToString().ToLower().Contains(FilterText.ToLower());
						}
						else if (conditions.SelectedItem.ToString() == "Equals")
						{
							return FilterText.Equals(exactValue.ToString());
						}
						else
						{
							return !FilterText.Equals(exactValue.ToString());
						}
					}
				}
			}

		}

		return false;

	}


	private void SearchButton_Pressed(object sender, EventArgs e)
	{
		OnFilterChanged();
	}

}

