using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityFrameworkClassLibrary.Models
{
    public abstract class Timestamps
    {
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }

    }
}
