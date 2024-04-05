using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrediV1_Prueba.Controllers
{
    public class InicioController : Controller
    {
        [HttpGet]
        public ActionResult InicioDeSesion()
        {
            return View();
        }

        // GET: InicioController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: InicioController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InicioController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InicioController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: InicioController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: InicioController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: InicioController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
