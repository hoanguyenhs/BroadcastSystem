using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BroadcastSystem.Models;

namespace BroadcastSystem.Controllers
{
    public class MessagesController : Controller
    {
        private broadcastsystemEntities db = new broadcastsystemEntities();

        //Chen's function 
        public JsonResult GetAll()
        {
            var jsonResult = Json(
                db.Messages.ToList()
                .Where(x => x.IsActived == true)
                .OrderByDescending(x => x.ID),
                JsonRequestBehavior.AllowGet
                );
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        // Hayden's function
        public JsonResult GetBroadcasting()
        {
            var jsonResult = Json(
                db.Messages.ToList()
                .Where(x => x.IsActived == true && x.IsBroadcasting == true &&
                    x.To >= DateTime.Now && x.From <= DateTime.Now)
                .OrderByDescending(x => x.ID),
                JsonRequestBehavior.AllowGet
                );
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        // GET: Messages no model
        public ActionResult Index()
        {
            return View();
        }

        // GET: Messages/Details/5
        public JsonResult Details(int? id)
        {
            Message message = db.Messages.Find(id);
            return Json(message, JsonRequestBehavior.AllowGet);
        }

        // GET: Messages/Create
        public ActionResult Create()
        {
            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "Username");
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,AuthorID,Title,Content,From,To,IsBroadcasting,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy,IsActived")] Message message)
        {
            if (ModelState.IsValid)
            {
                message.CreatedOn = DateTime.Now;
                message.UpdatedOn = DateTime.Now;
                message.IsActived = true;
                message.IsBroadcasting = true;
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "Username", message.AuthorID);
            return View(message);
        }

        // GET: Messages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "Username", message.AuthorID);
            return View(message);
        }

        // POST: Messages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,AuthorID,Title,Content,From,To,IsBroadcasting,CreatedOn,CreatedBy,UpdatedOn,UpdatedBy,IsActived")] Message message)
        {
            if (ModelState.IsValid)
            {
                message.UpdatedOn = DateTime.Now;
                db.Entry(message).State = EntityState.Modified;
                db.Entry(message).Property(m => m.CreatedOn).IsModified = false;
                db.Entry(message).Property(m => m.IsActived).IsModified = false;
                db.Entry(message).Property(m => m.IsBroadcasting).IsModified = false;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AuthorID = new SelectList(db.Authors, "ID", "Username", message.AuthorID);
            return View(message);
        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            message.UpdatedOn = DateTime.Now;
            message.IsActived = false;
            db.Entry(message).State = EntityState.Modified;
            db.Entry(message).Property(m => m.CreatedOn).IsModified = false;
            db.Entry(message).Property(m => m.IsBroadcasting).IsModified = false;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public int Broadcast(int? id, bool value)
        {
            if (id == null)
            {
                return 0;
            }
            Message message = db.Messages.Find(id);
            message.UpdatedOn = DateTime.Now;
            message.IsBroadcasting = value;
            db.Entry(message).State = EntityState.Modified;
            db.Entry(message).Property(m => m.CreatedOn).IsModified = false;
            db.Entry(message).Property(m => m.IsActived).IsModified = false;
            db.SaveChanges();
            return 1;
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
