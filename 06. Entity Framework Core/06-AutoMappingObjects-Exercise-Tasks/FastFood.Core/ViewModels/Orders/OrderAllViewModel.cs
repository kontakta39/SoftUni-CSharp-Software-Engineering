namespace FastFood.Core.ViewModels.Orders;

public class OrderAllViewModel
{
    public int OrderId { get; set; }

    public string Customer { get; set; }

    public string Employee { get; set; }

    public string Item { get; set; }

    //This property is unnecessary, because we use DateTime.Now option in Orders View -> All
    //public string DateTime { get; set; }
}