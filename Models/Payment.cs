namespace web.Models;

public enum Type {
    Cash, Card, Crypto
}

public class Payment {
    public int PaymentID { get; set; }
    public int BookingID { get; set; }
    public DateTime? PaymentDate { get; set; }
    public decimal PayAmount { get; set; }
    public Type? PaymentMethod { get; set; }
    public Boolean Paid { get; set; }
    
    public Booking Booking { get; set; }
}