# Perform-Filtering-in-.NET-MAUI-DataGrid
In this article, we will show you how to configure and perform filtering in the Syncfusion .NET MAUI DataGrid (SfDataGrid). The sample UI provides a compact filter bar with two Picker controls (column and condition) and a SearchBar for the filter text. Behind the scenes, the grid leverages its data view’s Filter predicate and RefreshFilter to efficiently include or exclude rows. You can filter a specific column or all columns at once, and switch between Equals, Does Not Equal, and Contains conditions. For complete guidance, see the documentation: [Filtering in .NET MAUI DataGrid](https://help.syncfusion.com/maui/datagrid/filtering) and the control overview: [.NET MAUI DataGrid (SfDataGrid) Overview](https://www.syncfusion.com/maui-controls/maui-datagrid).

## xaml
```
<ContentPage.BindingContext>
    <local:OrderInfoRepo />
</ContentPage.BindingContext>

<StackLayout>

    <StackLayout>
        <StackLayout Orientation="Horizontal">
        <Label Text="Filter Options:  " VerticalOptions="Center" Margin="3"/>
        
        <Picker x:Name="columns" Margin="3" WidthRequest="100">
            <Picker.Items>
                <x:String>All columns</x:String>
                <x:String>OrderID</x:String>
                <x:String>Customer</x:String>
                <x:String>CustomerID</x:String>
                <x:String>ShipCountry</x:String>
                <x:String>ShipCity</x:String>
            </Picker.Items>
            <Picker.SelectedItem>
                <x:String>OrderID</x:String>
            </Picker.SelectedItem>
        </Picker>
        
        <Picker x:Name="conditions" Margin="3" WidthRequest="100">
            <Picker.Items>
                <x:String>Equals</x:String>
                <x:String>Does Not Equal</x:String>
                <x:String>Contains</x:String>
            </Picker.Items>
            <Picker.SelectedItem>
                <x:String>Equals</x:String>
            </Picker.SelectedItem>
        </Picker>
    </StackLayout>

    <SearchBar x:Name="filterText"
                Placeholder="Search here to filter"
                SearchButtonPressed="SearchButton_Pressed" 
                TextChanged="FilterTextChanged"/>

    <syncfusion:SfDataGrid x:Name="dataGrid" ItemsSource="{Binding Orders}"
                            AutoGenerateColumnsMode="None"
                            GridLinesVisibility="Both"
                            HeaderGridLinesVisibility="Both"
                            ColumnWidthMode="Auto">
        <syncfusion:SfDataGrid.Columns>
            <syncfusion:DataGridTextColumn MappingName="OrderID"
                                            HeaderText="Order ID"></syncfusion:DataGridTextColumn>
            <syncfusion:DataGridTextColumn MappingName="Customer"
                                            HeaderText="Name"></syncfusion:DataGridTextColumn>
            <syncfusion:DataGridTextColumn MappingName="ShipCountry"
                                            HeaderText="Country"></syncfusion:DataGridTextColumn>
            <syncfusion:DataGridTextColumn MappingName="CustomerID"
                                            HeaderText="Customer ID"></syncfusion:DataGridTextColumn>
            <syncfusion:DataGridTextColumn MappingName="ShipCity"
                                            HeaderText="City"></syncfusion:DataGridTextColumn>

        </syncfusion:SfDataGrid.Columns>

    </syncfusion:SfDataGrid>
</StackLayout>
```

## C#
The code-behind wires the SearchBar and Picker state to a reusable Filter predicate. When you press the search action, the grid’s view applies the predicate and refreshes.
```
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
```

## Add more filtering options
Extend the predicate to support other scenarios that suit your data model. Examples:
- Case-insensitive Equals by normalizing both sides with ToLower
- StartsWith/EndsWith on string fields
- Numeric comparisons for IDs or quantities
- Range filters by parsing two values (e.g., 100–500) and checking bounds
```
// Example: StartsWith
if (conditions.SelectedItem?.ToString() == "StartsWith")
    return value.StartsWith(FilterText, StringComparison.OrdinalIgnoreCase);
```
Tips
- Always null-check properties when using reflection and guard against empty FilterText.
- For large datasets, debounce frequent updates; apply filtering on SearchButtonPressed, not every keystroke.
- To clear filters, set dataGrid.View.Filter = null; then call dataGrid.View.RefreshFilter().
- Keep MappingName in XAML aligned with your data model so property reflection succeeds.

## Try it
1. Run the sample and confirm the grid displays orders from OrderInfoRepo.
2. Pick a column or choose All columns.
3. Pick the condition (Equals, Does Not Equal, Contains).
4. Type some text and press the search action on the keyboard. The view refreshes with matching rows.
5. Change the criteria and press search again to update. Clear text to remove filters.

Take a moment to explore this [documentation](https://help.syncfusion.com/maui/datagrid/filtering), where you can find more information about Syncfusion .NET MAUI DataGrid (SfDataGrid) filtering with code examples. Please refer to this [link](https://www.syncfusion.com/maui-controls/maui-datagrid) to learn about the essential features of Syncfusion .NET MAUI DataGrid (SfDataGrid).
 
##### Conclusion
 
I hope you enjoyed learning about how to perform filtering in .NET MAUI DataGrid (SfDataGrid).
 
You can refer to our [.NET MAUI DataGrid’s feature tour](https://www.syncfusion.com/maui-controls/maui-datagrid) page to learn about its other groundbreaking feature representations. You can also explore our [.NET MAUI DataGrid Documentation](https://help.syncfusion.com/maui/datagrid/getting-started) to understand how to present and manipulate data. 
For current customers, you can check out our .NET MAUI components on the [License and Downloads](https://www.syncfusion.com/sales/teamlicense) page. If you are new to Syncfusion, you can try our 30-day [free trial](https://www.syncfusion.com/downloads/maui) to explore our .NET MAUI DataGrid and other .NET MAUI components.
 
If you have any queries or require clarifications, please let us know in the comments below. You can also contact us through our [support forums](https://www.syncfusion.com/forums), [Direct-Trac](https://support.syncfusion.com/create) or [feedback portal](https://www.syncfusion.com/feedback/maui?control=sfdatagrid), or the feedback portal. We are always happy to assist you!
