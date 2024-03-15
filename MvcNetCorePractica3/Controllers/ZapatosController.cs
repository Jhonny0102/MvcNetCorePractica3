using Microsoft.AspNetCore.Mvc;
using MvcNetCorePractica3.Models;
using MvcNetCorePractica3.Repositories;

namespace MvcNetCorePractica3.Controllers
{
    public class ZapatosController : Controller
    {
        private RepositoryZapatos repo;
        public ZapatosController(RepositoryZapatos repo)
        {
            this.repo = repo;
        }
        public async Task<IActionResult> ZapatosIndex()
        {
            List<Zapato> zapatos = await this.repo.GetZapatosAsync();
            return View(zapatos);
        }

        public async Task<IActionResult> ZapasImagenesOut(int? posicion, int idproducto)
        {
            if (posicion == null)
            {
                posicion = 1;
            }

            ModelPaginacionZapas model = await this.repo.GetZapatosImagenes(posicion.Value, idproducto);

            int siguiente = posicion.Value + 1;

            if (siguiente > model.NumReigstro)
            {
                siguiente = model.NumReigstro;
            }
            int anterior = posicion.Value - 1;
            if (anterior < 1)
            {
                anterior = 1;
            }
            ViewData["IDPRODUCTO"] = idproducto;
            ViewData["ZAPATILLA"] = model.zapato;
            ViewData["ULTIMO"] = model.NumReigstro;
            ViewData["SIGUIENTE"] = siguiente;
            ViewData["ANTERIOR"] = anterior;
            ViewData["POSICION"] = posicion;
            return View(model.ImagenZapatos);
        }

        public async Task<IActionResult> CrearImagenes()
        {
            List<Zapato> zapatos = await this.repo.GetZapatosAsync();
            return View(zapatos);
        }
        [HttpPost]
        public async Task<IActionResult> CrearImagenes(int idproducto, int idimagen, List<string> imagen)
        {
            this.repo.InsertImagen(imagen, idproducto, idimagen);
            return RedirectToAction("ZapatosIndex");
        }
    }
}













//if (posicion == null)
//{
//    //PONEMOS LA POSICION EN EL PRIMER REGISTRO 
//    posicion = 1;
//}
////PRIMERO = 1 
////SIGUIENTE = 
////ANTERIOR = 
////ULTIMO = 
//int numeroRegistros = await this.repo.GetNumRegistrosZapatosAsync();

//int siguiente = posicion.Value + 1;

////DEBEMOS COMPROBAR QUE NO PASAMOS DEL NUMERO DE REGISTROS 
//if (siguiente > numeroRegistros)
//{
//    //EFECTO OPTICO 
//    siguiente = numeroRegistros;
//}

//int anterior = posicion.Value - 1;

//if (anterior < 1)
//{
//    anterior = 1;
//}

//Zapato zapas = await this.repo.GetZapatosPosicionAsync(posicion.Value);

//ViewData["ULTIMO"] = numeroRegistros;

//ViewData["SIGUIENTE"] = siguiente;

//ViewData["ANTERIOR"] = anterior;

//return View(zapas);
