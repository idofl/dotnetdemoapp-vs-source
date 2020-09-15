using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Principal;

namespace dotnetdemoappMVC.Controllers
{
    public class SecureController : Controller
    {
        // GET: Secure
        [Authorize]
        public ActionResult Authenticate()
        {
            ViewBag.Message = "Test Windows Authentication";
            var groupNames = new List<string>();
            var wi = (WindowsIdentity)User.Identity;
            foreach (var group in wi.Groups)
            {
                groupNames.Add(group.Translate(typeof(NTAccount)).Value);
            }

            return View(groupNames);
        }
    }
}