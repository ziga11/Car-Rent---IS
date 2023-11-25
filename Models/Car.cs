namespace web.Models;

public class Car {
    public int CarID { get; set; }
    public int LocationID { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public string Color { get; set; }
    public string RegistrationNumber { get; set; }
    public string? imageURL { get; set; }
    public int DailyRate { get; set; }

    public Location Location { get; set; }
    public ICollection<Booking> Bookings { get; set; }
}