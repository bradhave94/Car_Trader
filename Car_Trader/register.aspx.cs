using Car_Trader.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

/*
    Authors: Jacob Amaral & Bradley Haveman
    File Name: register.aspx
    File Description: Used to register an account within our site
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
    public partial class register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //If the save button wasn't click & we have an ID in the URL
            if (!IsPostBack && (Request.QueryString.Count > 0))
            {
                //Change the H1
                lblRegister.Text = "Change Info";
                //Show the old password field
                pnlOldPassword.Visible = true;
                //Populate the form with the user info
                GetUser();
            } //End IF
        } //End Page_Load

        protected void GetUser()
        {
            //Populate the form with the user info
            Int32 UserID = Convert.ToInt32(Request.QueryString["UserID"]);

            try
            {
                //Connect to db
                using (COMP2007Entities db = new COMP2007Entities())
                {
                    //Populate the user from the database
                    CarUser u = (from objs in db.CarUsers where objs.userID == UserID select objs).FirstOrDefault();

                    //Map the user info into the form
                    if (u != null)
                    {
                        txtFirstName.Text = u.firstName;
                        txtLastName.Text = u.lastName;
                        txtPhone.Text = u.phoneNum;
                        txtEmail.Text = u.email;
                        txtUsername.Text = u.userName;
                        txtOldPassHidden.Text = u.userPassword;
                    } //End IF
                } //End using
            } //End TRY
            //Catch any error and redirect to the error page
            catch (SystemException ex)
            {
                Response.Redirect("/error.aspx");
            } //End CATCH
        } //End GetUser

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Default UserStore constructor uses the default connection string named
            var userStore = new UserStore<IdentityUser>();
            var userManager = new UserManager<IdentityUser>(userStore);

            //If there is an ID in the URL - We are editing
            if ((Request.QueryString.Count > 0))
            {
                string userID = "";
                if (Request.QueryString["UserID"] != null)
                {
                    //Get the ID from url
                    userID = (Request.QueryString["UserID"]);
                } //End IF

                //Get the user name of the logged in user
                string userName = HttpContext.Current.User.Identity.Name;

                //Find the user with that user
                var u = userManager.FindByName(userName);

                //Set the user name
                u.UserName = txtUsername.Text;

                //CHANGE THE PASSWORD
                //What I tried to get to work, but wouldn't work properly
                //**userManager.ChangePassword(u.Id.ToString(), txtOldPassword.Text, txtPassword.Text);**//

                //This worked, but doesn't seem very secure. What if the AddPassword fails? then that user has no password.
                //But it work for the purpose of this application
                userManager.RemovePassword(u.Id.ToString());
                userManager.AddPassword(u.Id.ToString(), txtPassword.Text);

                //update the user in the Asp.Net identity table
                userManager.Update(u);

                //Save the user in our CarUsers table
                saveEditUser();

                //Sign them out and make them login again with thier new info
                var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                authenticationManager.SignOut();
                Response.Redirect("login.aspx");
            } //End IF

            //If we are creating a new user
            else
            {
                //try and create the user.
                var user = new IdentityUser() { UserName = txtUsername.Text };
                IdentityResult result = userManager.Create(user, txtPassword.Text);

                //If creating the user succeeded
                if (result.Succeeded)
                {
                    //Create the user in the Asp.Net identity table
                    var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                    var userIdentity = userManager.CreateIdentity(user, DefaultAuthenticationTypes.ApplicationCookie);

                    //Sign them is
                    authenticationManager.SignIn(new AuthenticationProperties() { }, userIdentity);

                    //Create the user in our CarUsers tabel
                    saveEditUser();

                    //Redirect
                    Response.Redirect("default.aspx");
                } //End IF

                //If creating the user failed - it already exsists
                else
                {
                    //Display an error message
                    lblStatus.Text = result.Errors.FirstOrDefault();
                    lblStatus.CssClass = "label label-danger";
                } //End ELSE
            } //End ELSE
        } //End btnSave_Click

        protected void saveEditUser()
        {
            try
            {
                //Connect to EF
                using (COMP2007Entities db = new COMP2007Entities())
                {
                    //Initialized a new CarUser
                    CarUser u = new CarUser();
                    Int32 UserID = 0;

                    //Check he query string for an ID so we know to add or edit
                    if (Request.QueryString["UserID"] != null)
                    {
                        //Get the ID from url
                        UserID = Convert.ToInt32(Request.QueryString["UserID"]);

                        //Get the current user from EF
                        u = (from objs in db.CarUsers where objs.userID == UserID select objs).FirstOrDefault();
                    } //End IF

                    //Set the CarUser field to the form feild

                    u.firstName = txtFirstName.Text;
                    u.lastName = txtLastName.Text;
                    u.email = txtEmail.Text;
                    u.phoneNum = txtPhone.Text;
                    u.userName = txtUsername.Text;
                    u.userPassword = txtPassword.Text;

                    //If the user ID is 0 - We are adding
                    if (UserID == 0)
                    {
                        db.CarUsers.Add(u);
                    } //End IF

                    //Save the changes done to the database
                    db.SaveChanges();
                } //End using
            } //End TRY
            //Catch any error and redirect to the error page
            catch (SystemException ex)
            {
                Response.Redirect("/error.aspx");
            } //End CATCH
        } //End saveEditUser
    } //End Class register
} //End namespace