using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using U3RazasPerros.Models;

namespace U3RazasPerros.Areas.Admin.Models.ViewModels
{
    public class RazasIndexViewModel
    {
        public int IdPais { get; set; }
        public IEnumerable<Paises> Paises { get; set; }
        public IEnumerable<Razas> Razas { get; set; }
    }
}
