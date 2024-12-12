using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BikeRentalManagement.Database.Entities;

public class Notification
{
    [Key]
    public int NotificationId { get; set; }

    [ForeignKey("Email")]
    public int EmailId { get; set; }

    [ForeignKey("User")]
    public int UserId { get; set; }

    [Required]
    public DateTime Date { get; set; }


    public Email Email { get; set; }
    public User User { get; set; }
}
