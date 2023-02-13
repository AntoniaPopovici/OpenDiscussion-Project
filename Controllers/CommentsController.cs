using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenDiscussion_AutentificareIdentity.Data;
using OpenDiscussion_AutentificareIdentity.Models;


namespace OpenDiscussion_AutentificareIdentity.Controllers
{
    public class CommentsController : Controller
    {
        private readonly ApplicationDbContext db;
		
        // GET: Comments
		public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "User, Editor, Admin")]
        // GET: Edit
        public ActionResult Edit(int id)
        {
            Comment reply = db.Comments.Find(id);
            SetAccessRights();

			if (reply.UserId == User.Identity.GetUserId())
            {
                return View(reply);
            }
            else
            {
                TempData["message"] = "You don't have access to edit this comment!";
                return Redirect("/Discussion/Show/" + reply.DiscussionId);
            }

          
        }

        //DELETE
        [Authorize(Roles = "User,Editor")]
        public IActionResult Delete(int id)
        {
            Comment comment = db.Comments.Find(id);

            if (User.IsInRole("Editor")) // mai trebuie daca ii apartine userului
            {
                db.Comments.Remove(comment);
                db.SaveChanges();
                TempData["message"] = "yup";
                return Redirect("/Discussion/Show/" + comment.DiscussionId);
            }
            else
            {
                TempData["message"] = "nop";
                return Redirect("/Discussion/Show/" + comment.DiscussionId);
            }
        }

		private void SetAccessRights()
		{
            ViewBag.esteUser = User.IsInRole("User");
			ViewBag.esteAdmin = User.IsInRole("Admin");
			ViewBag.esteModerator = User.IsInRole("Editor");
			ViewBag.currentUser = User.Identity.GetUserId();
		}
	}
}