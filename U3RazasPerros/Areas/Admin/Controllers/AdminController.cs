using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using U3RazasPerros.Areas.Admin.Models.ViewModels;
using U3RazasPerros.Models;
using U3RazasPerros.Models.ViewModels;

namespace U3RazasPerros.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    { 
        public perrosContext Context { get; }
        public IWebHostEnvironment Host { get; }

        public AdminController(perrosContext context, IWebHostEnvironment host)
        {
            Context = context;
            Host = host;
        }
        [Route("admin/Index")]
        [Route("admin/Admin/Index")]
        [Route("admin/")]
        [HttpGet]
        public IActionResult Index()
        {
            RazasIndexViewModel vm = new RazasIndexViewModel();
            vm.Paises = Context.Paises.OrderBy(x => x.Nombre);
            vm.Razas = Context.Razas.Include(x => x.IdPaisNavigation).OrderBy(x => x.Nombre);
            return View(vm);
        }

        [HttpGet]
        public IActionResult Agregar()
        {
            RazasAgregarViewModel vm = new RazasAgregarViewModel();
            vm.Paises = Context.Paises.OrderBy(x => x.Nombre);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Agregar(RazasAgregarViewModel vm, IFormFile archivo1)
        {
            if (vm.Razas.Id <= 0)
            {
                ModelState.AddModelError("", "Es necesario un ID");
            }

            if (string.IsNullOrWhiteSpace(vm.Razas.Nombre))
            {
                ModelState.AddModelError("", "El nombre no pude estar en blanco");
            }

            if (archivo1.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("", "Solo permitimos la carga de archivos JPG");
                return View(vm);
            }
            if (archivo1.Length > 1024 * 1024 * 5)
            {
                ModelState.AddModelError("", "Solo permitimos JPGmenores a 5MB");
                return View(vm);
            }
            try
            {
                Context.Add(vm.Razas);
                Context.SaveChanges();

            var path = Host.WebRootPath + "/imgs_perros/" + vm.Razas.Id + "_0.jpg";
            FileStream fs = new FileStream(path, FileMode.Create);
            archivo1.CopyTo(fs);
            fs.Close();

            return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                vm.Paises = Context.Paises.OrderBy(x => x.Nombre);
                return View(vm);
            }
        }

        [HttpGet]
        public IActionResult Editar(int id)
        {
            RazasAgregarViewModel vm = new RazasAgregarViewModel();

            var raza = Context.Razas.FirstOrDefault(x => x.Id == id);
            if (raza == null)
            {
                return RedirectToAction("Index", "Admin");
            }

            vm.Paises = Context.Paises.OrderBy(x => x.Nombre);
            return View(vm);
        }

        [HttpPost]
        public IActionResult Editar(RazasAgregarViewModel vm, IFormFile archivo1)
        {
            if (vm.Razas.Id <= 0)
            {
                ModelState.AddModelError("", "Es necesario un ID");
            }

            if (string.IsNullOrWhiteSpace(vm.Razas.Nombre))
            {
                ModelState.AddModelError("", "El nombre no pude estar en blanco");
            }

            if (archivo1.ContentType != "image/jpeg")
            {
                ModelState.AddModelError("", "Solo permitimos la carga de archivos JPG");
                return View(vm);
            }
            if (archivo1.Length > 1024 * 1024 * 5)
            {
                ModelState.AddModelError("", "Solo permitimos JPGmenores a 5MB");
                return View(vm);
            }

            var raza = Context.Razas.FirstOrDefault(x => x.Id == vm.Razas.Id);
            if (raza == null)
            {
                return RedirectToAction("Index", "Admin");
            }

            raza.IdPais = vm.Razas.IdPais;
            raza.Nombre = vm.Razas.Nombre;
            raza.Descripcion = vm.Razas.Descripcion;
            raza.OtrosNombres = vm.Razas.OtrosNombres;
            try
            {
                Context.Update(raza);
                Context.SaveChanges();

                if (archivo1 != null)
                {
                    var path = Host.WebRootPath + "/imgs_perros/" + vm.Razas.Id + "_0.jpg";
                    FileStream fs = new FileStream(path, FileMode.Create);
                    archivo1.CopyTo(fs);
                    fs.Close();
                }

                return RedirectToAction("Index", "Admin");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                vm.Paises = Context.Paises.OrderBy(x => x.Nombre);
                return View(vm);
            }
        }

        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            var raza = Context.Razas.FirstOrDefault(x => x.Id == id);

            if (raza == null)
            {
                return RedirectToAction("Index", "Admin");
            }
            return View(raza);
        }

        [HttpPost]
        public IActionResult Eliminar(Razas r)
        {
            var raza = Context.Razas.FirstOrDefault(x => x.Id == r.Id);

            if (raza == null)
            {
                ModelState.AddModelError("", "La raza no existe o ya ha sido eliminada.");
            }
            else
            {
                Context.Remove(raza);
                Context.SaveChanges();

                var path = Host.WebRootPath + "/imgs_perros/" + raza.Id + "_0.jpg";
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            return View();
        }
    }
}
