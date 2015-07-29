using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Car_Trader.Models;

/*
        Authors: Jacob Amaral & Bradley Haveman
        File Name: addModel.aspx
        File Description: User who are logged in can add a model to the database
        Project name: Car Trader
        Project Description: Car Trader is an online car selling/buying site where you can find the perfect car for you, or sell your old car.
        Date Updated: July 27, 2015   
 */

namespace Car_Trader.admin
{
    public partial class addModel : System.Web.UI.Page
    {
        private int makeID;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                getMakes();
        }

        protected void ddlMake_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void getMakes()
        {
            try
            {
                //Connect to EF
                using (COMP2007Entities db = new COMP2007Entities())
                {

                    //Get the makes from the db
                    var makes = (from d in db.CarMakes
                                 orderby d.makeID
                                 select d);


                    //Bind them to the grid
                    ddlMake.DataSource = makes.ToList();
                    ddlMake.DataBind();
                    ddlMake.Items.Insert(0, new ListItem("Please select a make", "0"));


                } //End using
            } //End TRY
            //Catch any error and redirect to the error page
            catch (SystemException ex)
            {

            } //End CATCH
        }

        protected void btnAddMake_Click(object sender, EventArgs e)
        {
            try
            {
                //Connect to EF
                using (COMP2007Entities db = new COMP2007Entities())
                {

                    //Check if the make exists
                    int count = (from m in db.CarModels
                                 where m.model == txtModel.Text
                                 select m).Count();

                    if (count != 0)
                    {
                        //Reset the fields
                        lblStatus.Visible = true;
                        txtModel.Text = "";

                        //Show message
                        lblStatus.Text = "Model already exists";

                    }

                    else
                    {

                        CarModel model = new CarModel();

                        //Get the user input
                        model.model = txtModel.Text;
                        model.makeID = Convert.ToInt32(ddlMake.SelectedValue);

                        //Add to th DB
                        db.CarModels.Add(model);

                        //Save the DB
                        db.SaveChanges();

                        //Reset the fields
                        lblStatus.Visible = true;
                        ddlMake.SelectedIndex = 0;
                        txtModel.Text = "";

                        //Show message
                        lblStatus.Text = "Model successfully added";
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