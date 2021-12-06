using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using U3RazasPerros.Models;

namespace U3RazasPerros.Areas.Admin.Models.ViewModels
{
    public class RazasAgregarViewModel
    {
        public IEnumerable<Paises> Paises { get; set; }
        public Razas Razas { get; set; }
    }
}
