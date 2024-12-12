using System;
using System.ComponentModel.DataAnnotations;

namespace BikeRentalManagement.Database.Entities;

public class Email
{
    [Key]
    public int EmailId { get; set; }

    [Required]
    public EmailType EmailType { get; set; }

 
    public List<Notification> Notifications { get; set; } = new List<Notification>();
}
