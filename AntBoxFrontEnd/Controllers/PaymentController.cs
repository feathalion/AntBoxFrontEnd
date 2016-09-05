﻿using AntBoxFrontEnd.Infrastructure;
using AntBoxFrontEnd.Models;
using AntBoxFrontEnd.Services.Customer;
using AntBoxFrontEnd.Services.Payments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace AntBoxFrontEnd.Controllers
{
    public class PaymentController : Controller
    {
        [HttpPost]
        public ActionResult GetCardsAjax()
        {
            var ps = new PaymentService(ServiceConfiguration.GetApiKey());

            List < CardObject > result = new List<CardObject>();
            try
            {
                result = ps.ListPaymetCards(((CustomerResponse)Session["customer"]).Id);

                return PartialView("_CustomerCards",result);
            } catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult RegisterCardAjax(string deviceId, string state, string token)
        {
            var ps = new PaymentService(ServiceConfiguration.GetApiKey());

            PaymentRequestOptions pro = new PaymentRequestOptions
            {
                Customer_id = ((CustomerResponse)Session["customer"]).Id,
                Device_id = deviceId,
                State = state,
                Token = token
            };
            bool result = ps.CreatePaymentCard(pro);
            if (result)
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult MakePaymentAjax(decimal amount)
        {
            var ps = new PaymentService(ServiceConfiguration.GetApiKey());
            Charge c = new Charge()
            {
                Amount = amount,
                Customer_id = ((CustomerResponse)Session["customer"]).Id
            };
            if (ps.DoCharge(c))
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }



    }
}