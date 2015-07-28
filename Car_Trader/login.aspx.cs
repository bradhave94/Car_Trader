using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

/*
    Authors: Jacob Amaral & Bradley Haveman
    File Name: login.aspx
    File Description: User can use trhis page to log into thier account
    Project name: Car Trader
    Project Description: Car Trader is an online car selling/buying site where you can find the perfect car for you, or sell your old car.
    Date Updated: July 27, 2015
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Car_Trader
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        } //End Page_Load

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //Find the user with the given login info
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);
            var user = userManager.Find(txtUsername.Text, txtPassword.Text);

            //If user is found
            if (user != null)
            {
                //log them in
                var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, userIdentity);
                if (User.Identity.IsAuthenticated)
                    Response.Redirect("default.aspx");
            }//End IF

            //If user doesn't exsist
            else
            {
                lblStatus.Text = "Invalid username or password.";
            } //End ELSE
        } //End btnLogin_Click
    } //End Class login
} //End namespace