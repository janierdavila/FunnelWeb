﻿using System.Web.Mvc;
using FunnelWeb.Extensions.SqlAuthentication.Model;
using FunnelWeb.Web.Application.Filters;

namespace FunnelWeb.Extensions.SqlAuthentication.Controllers
{
    [FunnelWebRequest]
    public class SqlAuthenticationController : Controller
    {
        public ActionResult Index()
        {
            return View(new IndexModel());
        }

        public ActionResult EnableSqlAuthentication()
        {
            return RedirectToAction("Index");
        }
    }
}