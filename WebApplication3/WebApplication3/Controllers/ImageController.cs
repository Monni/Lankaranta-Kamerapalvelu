using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    [HandleError]
    public class ImageController : Controller
    {
        ImageModel db;

        public ImageController()
        {
            db = new ImageModel();
        }
      
        // GET: Image
        public ActionResult Index()
        {
            ViewData.Model = db.imagepath.ToList();
            return View();
        }
    }
}