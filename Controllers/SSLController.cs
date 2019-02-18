using SAILI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SAILI.Controllers
{
    public class SSLController : Controller
    {
        // GET: SSL
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ConvertHTTPSLogin()
        {
            SecuredSocketLayer SSL = new SecuredSocketLayer();
            string returnUrl = null;

            returnUrl = SSL.SecuredLayerLogin();
            Response.Redirect(returnUrl);

            return View();
        }

        public ActionResult ConvertHTTPSRegister()
        {
            SecuredSocketLayer SSL = new SecuredSocketLayer();
            string returnUrl = null;

            returnUrl = SSL.SecuredLayerRegister();

            Response.Redirect(returnUrl);

            return View();
        }
    }
}