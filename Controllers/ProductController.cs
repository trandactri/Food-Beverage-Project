using LoginandR.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;



namespace LoginandR.Controllers
{
    public class ProductController : Controller
    {
        private DB_Entities _db = new DB_Entities();
        
        public ActionResult Index()
        {
            IEnumerable<Product> productList = _db.Products;
            var product = productList.Select(p => p).Where(p => p.proStatus == true);
            return View(product.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "proID,tID,tName,supName,supID,proName,proPrice,proImg,proDescription,discount")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.proStatus = true;
                _db.Products.Add(product);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        public ActionResult Update(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update([Bind(Include = "proID,tID,tName,supName,supID,proName,proPrice,proImg,proDescription,discount")] Product product)
        {
            if (ModelState.IsValid)
            {
                product.proStatus = true;
                _db.Entry(product).State = EntityState.Modified;
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
            Product product = _db.Products.Find(id);
            product.proStatus = false;
            /*_db.Products.Remove(product);*/
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}