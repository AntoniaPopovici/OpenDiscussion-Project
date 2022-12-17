using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenDiscussion_AutentificareIdentity.Data;
using OpenDiscussion_AutentificareIdentity.Models;
using System.Data;

namespace OpenDiscussion_AutentificareIdentity.Controllers
{
    public class CommentsController : Controller
    {
        private ApplicationDbContext db;

        // GET: Comments
        public ActionResult Index()
        {
            return View();
        }

        /*
        //EDIT
        [Authorize(Roles = "User")]
        public IActionResult Edit(int id)
        {
            Comment comment = db.Comments.Find(id);

            * if (comentariu)
             * return View(comment);
             * else
             * {
             * TempData["message"] = "nope";
             * return Redirect("/Subjects/Show/" + comment.DiscussionId);
             * }
             

        }
        */

        //DELETE
        [Authorize(Roles = "User,Moderator")]
        public IActionResult Delete(int id)
        {
            Comment comment = db.Comments.Find(id);

            if (User.IsInRole("Moderator")) // mai trebuie daca ii apartine userului
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
                TempData["message"] = "yup";
                return Redirect("/Subjects/Show/" + comment.DiscussionId);
            }
            else
            {
                TempData["message"] = "nop";
                return Redirect("/Subjects/Show/" + comment.DiscussionId);
            }
        }
    }
}
