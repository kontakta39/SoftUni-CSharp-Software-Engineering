using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Trucks.Data.Models;

public class ClientTruck
{
    [Key]
    public int ClientId { get; set; }

    [ForeignKey(nameof(ClientId))]
    public Client Client { get; set; }

    [Key]
    public int TruckId { get; set; }

    [ForeignKey(nameof(TruckId))]
    public Truck Truck { get; set; }
}