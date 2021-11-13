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
    /// <summary>
    /// Controller for product
    /// </summary>
    public class ProductController : Controller
    {
        /// <summary>
        /// Variable used for activities on DB_Entities
        /// </summary>
        private DB_Entities _db = new DB_Entities();

        /// <summary>
        /// Action used to view products
        /// </summary>
        /// <returns>a view of product list</returns>
        public ActionResult Index()
        {
            IEnumerable<Product> productList = _db.Products;
            var product = productList.Select(p => p).Where(p => p.proStatus && p.Supplier.supStatus);
            return View(product.ToList());
        }

        /// <summary>
        /// Action used to return product 
        /// </summary>
        /// <param name="id">product id</param>
        /// <returns>a detail view match id</returns>
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = _db.Products.Find(id);
            if (product == null || !product.Supplier.supStatus)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        /// <summary>
        /// Action used to return a view
        /// </summary>
        /// <returns>Create view</returns>
        public ActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Action used to create a product
        /// </summary>
        /// <param name="product">product object after create</param>
        /// <returns>save changes to database and return to index</returns>
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

        /// <summary>
        /// Action used to return view product
        /// </summary>
        /// <param name="id">product id</param>
        /// <returns>Return update view</returns>
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

        /// <summary>
        /// Action used to edit the information of product
        /// </summary>
        /// <param name="product">product object</param>
        /// <returns>save changes to database and return to index</returns>
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

        /// <summary>
        /// Action used to return a view product
        /// </summary>
        /// <param name="id">product id</param>
        /// <returns>a specific product view</returns>
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

        /// <summary>
        /// Action used to delete the product by id and transfer the proStatus from "true" to "false"
        /// </summary>
        /// <param name="id">product id</param>
        /// <returns>After delete 1 product, return to Index view</returns>
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

        /// <summary>
        /// Function used to restore unneccessary or unused resources
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            // need to alway test if disposing pass else reallocations could occur during Finalize pass
            // also good practice to test resource was created
            if (disposing && _db != null)
                _db.Dispose();
            base.Dispose(disposing);
        }
    }
}