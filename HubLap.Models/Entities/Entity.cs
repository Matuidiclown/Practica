using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubLap.Models.Entities
{
    public abstract class Entity
    {   public int Id { get; set; }
        public DateTime UserRegistrationDate  { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
