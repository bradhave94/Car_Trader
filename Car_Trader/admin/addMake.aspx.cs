using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Car_Trader.Models;

/*
        Authors: Jacob Amaral & Bradley Haveman
        File Name: addMake.aspx
        File Description: User who are logged in can add a make to the database
        Project name: Car Trader
        Project Description: Car Trader is an online car selling/buying site where you can find the perfect car for you, or sell your old car.
        Date Updated: July 27, 2015   
 */

namespace Car_Trader.admin
{
    public partial class addMake : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAddMake_Click(object sender, EventArgs e)
        {
            try
            {
                //Connect to EF
                using (COMP2007Entities db = new COMP2007Entities())
                {

                    //Check if the make exists
                    int count = (from m in db.CarMakes
                                     where m.make == txtMake.Text
                                     select m).Count();

                    if(count != 0)
                    {
                        //Reset the fields
                        lblStatus.Visible = true;
                        txtMake.Text = "";

                        //Show message
                        lblStatus.Text = "Make already exists";
                        
                    }
                    else
                    {
                        CarMake make = new CarMake();

                        //The the user input
                        make.make = txtMake.Text;

                        //Add to the database
                        db.CarMakes.Add(make);

                        //Save the database
                        db.SaveChanges();

                        //Reset the fields
                        lblStatus.Visible = true;
                        txtMake.Text = "";

                        //Show message
                        lblStatus.Text = "Make successfully added";
                    }
                    
                }

                
            } //End TRY
            //Catch any error and redirect to the error page
            catch (SystemException ex)
            {
                Response.Redirect("/error.aspx");
            } //End CATCH
        }
    }
}