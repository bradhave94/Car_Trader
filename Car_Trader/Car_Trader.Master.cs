/*
    Authors: Jacob Amaral & Bradley Haveman
    File Name: Car_Trader.Master
    File Description: All pagers using this master page inherit from this page
    Project name: Car Trader
    Project Description: Car Trader is an online car selling/buying site where you can find the perfect car for you, or sell your old car.
    Date Updated: July 24, 2015   
*/

using Car_Trader.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Script;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace Car_Trader
{
    public partial class Car_Trader : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //If user is logged in
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                //Show and hide menu items
                plhPostAd.Visible = true;
                plhPublic.Visible = false;
                plhPrivate.Visible = true;

                //Show the username
                lblUser.Text = HttpContext.Current.User.Identity.Name;

            }
            else
            {
                //Show and hide menu items
                plhPostAd.Visible = false;
                plhPublic.Visible = true;
                plhPrivate.Visible = false;
            }
        } 
    }
}