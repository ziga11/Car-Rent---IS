namespace web.Models;

public class Location {
    public int LocationID { get; set; }
    public int ZipCode { get; set; }
    public string City { get; set; }
    public string Address { get; set; } 
    
    public ICollection<Car> availableCars { get; set; }
}