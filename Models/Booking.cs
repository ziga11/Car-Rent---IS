namespace web.Models;

public class Booking {
    public int BookingID { get; set; }
    public int CarID { get; set; }
    public int CustomerID { get; set; }
    public DateTime startDate  { get; set; }
    public DateTime endDate { get; set; }

    public Customer Customer { get; set; }
    public Car Car { get; set; }
}