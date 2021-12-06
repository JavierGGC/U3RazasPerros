using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace U3RazasPerros.Models.ViewModels
{
    public class RazaViewModel
    {
        public uint Id { get; set; }
        public string Nombre { get; set; }
        public Razas Raza { get; set; }
        public IEnumerable<Paises> Paises { get; set; }
        public IFormFile Archivo { get; set; }
        public string Imagen { get; set; }
    }
}
