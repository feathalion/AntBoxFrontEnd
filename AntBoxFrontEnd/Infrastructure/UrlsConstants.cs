﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AntBoxFrontEnd.Infrastructure
{
    public static class UrlsConstants
    {
        // public static String BaseUrl { get { return "http://173.203.159.102:8081/v1"; } }

        public static String Host { get { return "http://198.19.143.41:8082"; } }
        //public static String Host { get { return "http://64.28.103.85:8082"; } }
        public static String HostPublic { get { return "http://64.28.103.85:8082"; } }

        public static String BaseUrl { get { return Host + "/v1"; } }

        public static string AuthUrl { get { return Host + "/auth"; } }


        public static String CustomerAddress { get { return BaseUrl + "/customer-address"; } }

        public static String CustomerAddressSearch { get { return BaseUrl + "/customer-address/search"; } }

        public static String Customer { get { return BaseUrl + "/customer"; } }
        public static String CustomerTask { get { return BaseUrl + "/customer-tasks"; } }

        public static String Task { get { return BaseUrl + "/tasks"; } }

        public static String Schedules { get { return BaseUrl + "/schedules"; } }

        public static String Login { get { return BaseUrl + "/login"; } }

        public static String ZipCode { get { return BaseUrl + "/zipcode"; } }

        public static String PaymentCard { get { return BaseUrl + "/payment-card"; } }

        public static String PaymentDetail { get { return BaseUrl + "/payment-detail"; } }

        public static String Payment { get { return BaseUrl + "/payment"; } }

        public static String Box { get { return BaseUrl + "/box"; } }

        public static String BoxesSearch { get { return BaseUrl + "/box/search"; } }

        public static String Worker { get { return BaseUrl + "/worker"; } }

        public static String AntBoxOut { get { return BaseUrl + "/antboxs/out"; } }

        public static String AntBox { get { return BaseUrl + "/antboxs"; } }

        public static String AntBoxList { get { return BaseUrl + "/antboxs/search"; } }

        public static String UserList { get { return BaseUrl + "/user/search"; } }

        public static String User { get { return BaseUrl + "/user"; } }

        public static String Coupon { get { return BaseUrl + "/coupon"; } }

        public static String CouponList { get { return BaseUrl + "/coupon/search"; } }

        public static String Code { get { return BaseUrl + "/code"; } }

        public static String CodeList { get { return BaseUrl + "/code/search"; } }

        public static String CustomerList { get { return BaseUrl + "/cs/customer"; } }

        public static String AntboxsListCS { get { return BaseUrl + "/cs/customer"; } }

        public static String DeliveryListCS { get { return BaseUrl + "/cs/delivery"; } }

        public static String PickupListCS { get { return BaseUrl + "/cs/pickup"; } }

        public static String OrderListCS { get { return BaseUrl + "/cs/order"; } }

        public static String ListingCustomer { get { return BaseUrl + "/listing/customer"; } }

        public static String ListingClient { get { return BaseUrl + "/listing/client"; } }

        public static String ListingPayments { get { return BaseUrl + "/listing/payment"; } }

        public static String ListingPickups { get { return BaseUrl + "/listing/pickup"; } }

        public static String ListingDeliveries { get { return BaseUrl + "/listing/delivery"; } }

        public static String Contact { get { return BaseUrl + "/contact"; } }

        public static String Zipcode { get { return BaseUrl + "/zipcode"; } }

        public static String BillingAddress { get { return BaseUrl + "/address-billing"; } }

        public static String Billing { get { return BaseUrl + "/billing"; } }

        public static String Rules { get { return BaseUrl + "/rules"; } }

        public static String Client { get { return BaseUrl + "/client"; } }

        public static String CustomerBillingAddressSearch { get { return BaseUrl + "/address-billing/search"; } } 

        public static String ValidateAddress { get { return BaseUrl + "/customer-address/check"; } }

        public static String ResetPassword { get { return BaseUrl + "/reset"; } }

        public static String RestorePassword { get { return BaseUrl + "/restore"; } }

        public static String Referer { get { return BaseUrl + "/refered"; } }

    }
}