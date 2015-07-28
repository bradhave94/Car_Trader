/*
    Authors: Jacob Amaral & Bradley Haveman
    File Name: logout.aspx
    File Description: use to logout user, this page is never seen.
    Project name: Car Trader
    Project Description: Car Trader is an online car selling/buying site where you can find the perfect car for you, or sell your old car.
    Date Updated: July 24, 2015 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace Car_Trader
{
    public partial class logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Sign the logged in user out
            var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
            authenticationManager.SignOut();

            //Redirect
            Response.Redirect("default.aspx");
        } //End Page_Load
    } //End class logout
} //End namespace