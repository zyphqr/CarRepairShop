using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using CarRepairShop.Models;
using Microsoft.AspNetCore.Identity;

namespace CarRepairShop.Areas.Identity.Data;

// Add profile data for application users by adding properties to the Employee class
[Table("Mechanics")]
public class Mechanic : IdentityUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public ICollection<RepairCard> RepairCards { get; set; }
}

