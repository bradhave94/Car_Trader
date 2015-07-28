/*
    Authors: Jacob Amaral & Bradley Haveman
    File Name: account.aspx
    File Description: User can view their account details, delete or edit the user detial. And see their posted cars
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
using Car_Trader.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace Car_Trader.admin
{
    public partial class account : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Show the users detials
            getPosts();
            getAccount();

        } //end Page_Load


        protected void grdUsers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Store the selected row
            Int32 selectedRow = e.RowIndex;

            //Get the data key, in this case the UserID
            Int32 userID = Convert.ToInt32(grdUsers.DataKeys[selectedRow].Values["UserID"]);



            //Connect To Entity Framework
            using (COMP2007Entities db = new COMP2007Entities())
            {
                //Count the number of postings
                int count = (from cars in db.Cars
                             where cars.userID == userID
                             select cars).Count();

                if (count != 0)
                {
                    Response.Write(@"<script language='javascript'>alert('Please delete your postings first!');</script>");
                }
                else
                {
                    //Select the user by using the UserID
                    CarUser u = (from objs in db.CarUsers where objs.userID == userID select objs).FirstOrDefault();


                    //Delete it from our CarUsers table
                    db.CarUsers.Remove(u);
                    db.SaveChanges();

                    //Find our user in the ASP.Net Idenity user table
                    var userStore = new UserStore<IdentityUser>();
                    var userManager = new UserManager<IdentityUser>(userStore);
                    var user = userManager.Find(u.userName, u.userPassword);

                    //Delete that user from Idenity user table
                    userManager.Delete(user);


                    //Sign the deleted user out
                    var authenticationManager = HttpContext.Current.GetOwinContext().Authentication;
                    authenticationManager.SignOut();

                    //Redirect
                    Response.Redirect("/default.aspx");
                }
            } //End using


        } //End grdUsers_RowDeleting

        protected void getAccount()
        {
            try
            {
                //Connect to Entity Framework
                using (COMP2007Entities db = new COMP2007Entities())
                {
                    //get the current user name
                    string name = HttpContext.Current.User.Identity.Name;

                    //Find the user with that user name
                    var User = from u in db.CarUsers
                               where u.userName == name
                               select u;

                    //Show the resualts in the grid
                    grdUsers.DataSource = User.ToList();
                    grdUsers.DataBind();
                } //End using
            } //End TRY
            //Catch any error and redirect to the error page
            catch (SystemException ex)
            {
                Response.Redirect("/error.aspx");
            } //End CATCH
        } //End getAccount


        protected void getPosts()
        {
            //Get the user name of the logged in user
            string userName = User.Identity.Name;
            //Create some objects
            Int32 userID = 0;
            CarUser carUser = new CarUser();
            Car car = new Car();
            CarClass cClass = new CarClass();

            try
            {
                //Connect to EF
                using (COMP2007Entities db = new COMP2007Entities())
                {

                    //Cherck the URL for ID
                    if (Request.QueryString["UserID"] != null)
                    {
                        //Get the id from url
                        userID = Convert.ToInt32(Request.QueryString["userID"]);

                        //Get that records from the database
                        carUser = (from objs in db.CarUsers where objs.userID == userID select objs).FirstOrDefault();
                    }

                    //The URL is empty, get the userID of the logged in user.
                    else
                    {
                        carUser = (from objs in db.CarUsers where objs.userName == userName select objs).FirstOrDefault();
                    }

                    userID = carUser.userID;

                    //Count the number of postings
                    int count = (from cars in db.Cars
                                 where cars.userID == userID
                                 select cars).Count();

                    //If its greater that 0 them them
                    if (count > 0)
                    {
                        var objE = (from cars in db.Cars
                                    join cu in db.CarUsers on cars.userID equals cu.userID
                                    join carE in db.CarEngines on cars.engineID equals carE.engineID
                                    join carclass in db.CarClasses on cars.carID equals carclass.carID
                                    join model in db.CarModels on carclass.modelID equals model.modelID
                                    join make in db.CarMakes on model.makeID equals make.makeID
                                    where cars.userID == userID
                                    select new { make.make, model.model, carclass.@class, cars.modelYear, cars.transmission, carE.fuelType, cars.@new, cars.cost, cars.kilometer, cars.carID });

                        grdPosts.DataSource = objE.ToList();
                        grdPosts.DataBind();
                    }

                    //If there is none ask if they want to make one
                    else
                    {
                        grdPosts.Visible = false;
                        pnlAdPost.Visible = true;
                    }
                }
            } //End TRY
            //Catch any error and redirect to the error page
            catch (SystemException ex)
            {
                Response.Redirect("/error.aspx");
            } //End CATCH
        }

        protected void grdPosts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //store Theme row that was click
            Int32 selectedRow = e.RowIndex;

            //get the selected StudetnID using the grid's data key collection
            Int32 carID = Convert.ToInt32(grdPosts.DataKeys[selectedRow].Values["carID"]);

            //use EF to remove the seleted student
            using (COMP2007Entities db = new COMP2007Entities())
            {
                Car cars = (from objs in db.Cars where objs.carID == carID select objs).FirstOrDefault();
                CarClass carClass = (from objs in db.CarClasses where objs.carID == carID select objs).FirstOrDefault();

                db.Cars.Remove(cars);
                db.CarClasses.Remove(carClass);
                db.SaveChanges();
            }

            //refresh the grid
            getAccount();
            getPosts();
        }
    } //End class account
} //End namespace