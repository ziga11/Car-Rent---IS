namespace web.Models;

public class Customer {
    public int CustomerID { get; set; }
    public string UserID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    
    public User User { get; set; }
    public ICollection<Booking> Bookings { get; set; }
}