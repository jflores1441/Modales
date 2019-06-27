using Modales.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Xml.Linq;

namespace Modales.Controllers
{
    public class HomeController : Controller
    {
        private XDocument datos = XDocument.Load(HostingEnvironment.MapPath("~/Data/Datos.xml"));
        
        public ActionResult Index()
        {
            return View(datos);
        }

        public ActionResult Create()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(BookModel nuevo)
        {
            if (ModelState.IsValid)
            {
                XElement element = new XElement("book");
                element.SetAttributeValue("ISBN", nuevo.ISBN);
                element.SetAttributeValue("title", nuevo.Title);
                element.SetAttributeValue("price", nuevo.Price.ToString());
                datos.Root.Add(element);
                return Json(new { success = true });
            }
            else
            {
                return PartialView(nuevo);
            }
        }

        public ActionResult Edit(string isbn)
        {
            XElement libro = datos.Root.Elements().Where(e => e.Attribute("ISBN").Value.Equals(isbn)).FirstOrDefault();
            BookModel model = new BookModel() { 
                ISBN = libro.Attribute("ISBN").Value, 
                Title = libro.Attribute("title").Value, 
                Price = decimal.Parse(libro.Attribute("price").Value) 
            };
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult Edit(BookModel book)
        {
            XElement libro = datos.Root.Elements().Where(e => e.Attribute("ISBN").Value.Equals(book.ISBN)).FirstOrDefault();
            libro.Attribute("ISBN").Value = book.ISBN;
            libro.Attribute("title").Value = book.Title;
            libro.Attribute("price").Value = book.Price.ToString();
            return Json(new { success = true });
        }

        public ActionResult Delete(string isbn)
        {
            XElement libro = datos.Root.Elements().Where(e => e.Attribute("ISBN").Value.Equals(isbn)).FirstOrDefault();
            BookModel model = new BookModel()
            {
                ISBN = libro.Attribute("ISBN").Value,
                Title = libro.Attribute("title").Value,
                Price = decimal.Parse(libro.Attribute("price").Value)
            };
            return PartialView(model);
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(string isbn)
        {
            XElement libro = datos.Root.Elements().Where(e => e.Attribute("ISBN").Value.Equals(isbn)).FirstOrDefault();
            libro.Remove();
            return Json(new { success = true });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                datos.Save(HostingEnvironment.MapPath("~/Data/Datos.xml"));
            }
            base.Dispose(disposing);
        }
    }
}