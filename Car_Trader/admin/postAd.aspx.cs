/*
    Authors: Jacob Amaral & Bradley Haveman
    File Name: postAd.aspx
    File Description: This page is only accessable buy user who are logged in. They can use this page to post their car to the site
    Project name: Car Trader
    Project Description: Car Trader is an online car selling/buying site where you can find the perfect car for you, or sell your old car.
    Date Updated: July 27, 2015
 */

using Car_Trader.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Car_Trader.PostAd
{
    public partial class postAd : System.Web.UI.Page
    {
        //Min and max year for cars
        private const int MaxYear = 2017;

        private const int MinYear = 1900;
        private string carClass;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Populate that year drop down usin min and max year
                var fromRange = Enumerable.Range(MinYear, MaxYear - MinYear).Reverse();

                //Bing the data to the year dropdown
                ddlYear.DataSource = fromRange;
                ddlYear.DataBind();

                //Add please select a year to the dropdown
                ddlYear.Items.Insert(0, new ListItem("Please select a year", "0"));

                //Bind car makes to the make dropdown
                GetCarMakes();
                //Add please select a make to the dropdown
                ddlMake.Items.Insert(0, new ListItem("Please select a make", "0"));

                //Hide the make dropdown
                lblMake.Visible = false;
                ddlMake.Visible = false;

                //Hide the model dropdown
                lblModel.Visible = false;
                ddlModel.Visible = false;
            } //End IF

            //if save wasnt clicked and we have a student ID in thr url
            if ((!IsPostBack) && (Request.QueryString.Count > 0))
            {
                GetCar();
            }
        } //End Page_Load

        protected void GetCar()
        {
            //Show controls
            ddlMake.Visible = true;
            ddlModel.Visible = true;
            lblMake.Visible = true;
            lblModel.Visible = true;

            //populate the form with existing car records
            Int32 carID = Convert.ToInt32(Request.QueryString["carID"]);

            try
            {
                //connect to the db via EF
                using (COMP2007Entities db = new COMP2007Entities())
                {
                    //Populate the car instances
                    Car cars = (from objs in db.Cars where objs.carID == carID select objs).FirstOrDefault();
                    CarClass carClass = (from model in db.CarClasses where model.carID == carID select model).FirstOrDefault();

                    //Get the model ID
                    int modelID = carClass.modelID;

                    //Select the model
                    CarModel carModel = (from make in db.CarModels where make.modelID == modelID select make).FirstOrDefault();

                    int makeID = carModel.makeID;
                    //map the cars properties the form
                    if (cars != null)
                    {
                        ddlYear.SelectedValue = cars.modelYear.ToString();
                        ddlMake.SelectedValue = makeID.ToString();

                        Int32 makeid = Int32.Parse(ddlMake.SelectedValue);

                        try
                        {
                            //Connect to EF
                            using (COMP2007Entities ef = new COMP2007Entities())
                            {
                                //Select the models from the db
                                var models = (from d in ef.CarModels
                                              where d.makeID == makeID
                                              select d);

                                //Bind them to the grid
                                ddlModel.DataSource = models.ToList();
                                ddlModel.DataBind();

                                //Add please select a model to the dropdown
                                ddlModel.Items.Insert(0, new ListItem("Please select a model", "0"));
                            } //End using
                        } //End TRY
                        //Catch any error and redirect to the error page
                        catch (SystemException ex)
                        {
                            Response.Redirect("/error.aspx");
                        } //End CATCH

                        pnlSecondaryCarInput.Visible = true;

                        ddlModel.SelectedValue = modelID.ToString();
                        txtKM.Text = cars.kilometer.ToString();
                        txtCost.Text = cars.cost.ToString();
                        txtColour.Text = cars.color;

                        ddlEngine.SelectedIndex = cars.engineID;
                        ddlTransmission.SelectedValue = cars.transmission;

                        if (cars.transmission.ToString() == "Automatic")
                            ddlTransmission.SelectedIndex = 1;
                        if (cars.transmission.ToString() == "Manual")
                            ddlTransmission.SelectedIndex = 2;
                        else
                            ddlTransmission.SelectedIndex = 1;

                        if (cars.@new == true)
                            rblNewUsed.SelectedIndex = 0;
                        else
                            rblNewUsed.SelectedIndex = 1;

                        txtLocation.Text = cars.location;
                    }
                }
            } //End TRY
            //Catch any error and redirect to the error page
            catch (SystemException ex)
            {
                Response.Redirect("/error.aspx");
            } //End CATCH
        }

        //Populate the dropdown for the car makes
        protected void GetCarMakes()
        {
            try
            {
                //Connect to EF
                using (COMP2007Entities db = new COMP2007Entities())
                {
                    //Get the makes from the db
                    var makes = (from d in db.CarMakes
                                 orderby d.make
                                 select d);

                    //Bind them to the grid
                    ddlMake.DataSource = makes.ToList();
                    ddlMake.DataBind();
                } //End using
            } //End TRY
            //Catch any error and redirect to the error page
            catch (SystemException ex)
            {
                Response.Redirect("/error.aspx");
            } //End CATCH
        } //End GetCarmakes

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Remove this so they cant pick it
            ddlYear.Items.Remove(new ListItem("Please select a year", "0"));

            //Show the model dropdown
            lblMake.Visible = true;
            ddlMake.Visible = true;
        }

        protected void ddlMake_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the selected index is anything but 0
            if (ddlMake.SelectedItem != (new ListItem("Please select a make", "0")))
            {
                ddlMake.Items.Remove(new ListItem("Please select a make", "0"));
                //Show the model dropdown
                lblModel.Visible = true;
                ddlModel.Visible = true;

                pnlSecondaryCarInput.Visible = false;
                pnlButton.Visible = false;

                DropDownList makeDropdown = (DropDownList)sender;
                //Populate car models based on selected car make
                Int32 makeID = Int32.Parse(makeDropdown.SelectedValue);

                try
                {
                    //Connect to EF
                    using (COMP2007Entities db = new COMP2007Entities())
                    {
                        //Select the models from the db
                        var models = (from d in db.CarModels
                                      where d.makeID == makeID
                                      select d);

                        //Bind them to the grid
                        ddlModel.DataSource = models.ToList();
                        ddlModel.DataBind();

                        //Add please select a model to the dropdown
                        ddlModel.Items.Insert(0, new ListItem("Please select a model", "0"));
                    } //End using
                } //End TRY
                //Catch any error and redirect to the error page
                catch (SystemException ex)
                {
                    Response.Redirect("/error.aspx");
                } //End CATCH
            } //End IF

            //If no car make is selected, disable the car model dropdown
            else
            {
                ddlModel.Visible = false;
                lblModel.Visible = false;
                ddlModel.SelectedIndex = 0;
            } //End ELSE
        } //End ddlMake_SelectedIndexChanged

        protected void ddlModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            //If the selected index is anythin but 0
            if (ddlModel.SelectedItem != new ListItem("Please select a model", "0"))
            {
                ddlModel.Items.Remove(new ListItem("Please select a model", "0"));
                //Show the detials panle
                pnlSecondaryCarInput.Visible = true;
            } //End IF
        } //End ddlModel_SelectedIndexChanged

        protected void classRbt_CheckedChanged(object sender, EventArgs e)
        {
            //Get the car class that was picked
            carClass = ((RadioButton)sender).Text;
            //Show the post button
            pnlButton.Visible = true;
        }

        protected void ccontinueBtn_Click(object sender, EventArgs e)
        {
            //Show the next inputs
            classPnl.Visible = true;
            pnlButton.Visible = true;
        }

        //Get location script
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static List<string> GetLocation(string pre)
        {
            List<string> allLocations = new List<string>();
            try
            {
                using (COMP2007Entities dc = new COMP2007Entities())
                {
                    allLocations = (from a in dc.Cities
                                    where a.city1.StartsWith(pre)
                                    select a.city1).ToList();
                } //End using
            } //End TRY
            //Catch any error and redirect to the error page
            catch (SystemException ex)
            {
                HttpContext.Current.Response.Redirect("/error.aspx");
            } //End CATCH
            return allLocations;
        }

        protected bool checkClasses()
        {
            //Check if a class radio button was picked
            if (convertableRbt.Checked || coupeRbt.Checked || coupeRbt.Checked || hatchbackRbt.Checked || minivanRbt.Checked || sedanRbt.Checked || suvRbt.Checked || truckRbt.Checked || wagonRbt.Checked)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        protected void btnPost_Click(object sender, EventArgs e)
        {
            if (checkClasses())
            {
                string userName = User.Identity.Name;
                Int32 carID = 0;
                CarUser carUser = new CarUser();
                Car car = new Car();
                CarClass cClass = new CarClass();

                try
                {
                    //use EF to connect to SQL seer
                    using (COMP2007Entities db = new COMP2007Entities())
                    {
                        //check he query string for ID some we know add or edit
                        if (Request.QueryString["carID"] != null)
                        {
                            //Get the id from url
                            carID = Convert.ToInt32(Request.QueryString["carID"]);
                            car = (from objs in db.Cars where objs.carID == carID select objs).FirstOrDefault();
                            cClass = (from objs in db.CarClasses where objs.carID == carID select objs).FirstOrDefault();
                        }

                        //The URL is empty, get the userID of the logged in user.
                        else
                        {
                            carUser = (from objs in db.CarUsers where objs.userName == userName select objs).FirstOrDefault();
                            car.userID = carUser.userID;
                        }

                        car.engineID = Convert.ToInt32(ddlEngine.SelectedValue);
                        car.modelYear = Convert.ToInt32(ddlYear.SelectedValue);
                        car.transmission = ddlTransmission.SelectedItem.Text;

                        if (rblNewUsed.SelectedValue == "New")
                            car.@new = true;
                        else
                            car.@new = false;

                        car.color = txtColour.Text;
                        car.cost = Convert.ToDecimal(txtCost.Text);
                        car.location = txtLocation.Text;
                        car.kilometer = Convert.ToInt32(txtKM.Text);
                        car.listedDate = DateTime.Today;

                        if (carID == 0)
                        {
                            db.Cars.Add(car);
                        }

                        db.SaveChanges();

                        cClass.modelID = Convert.ToInt32(ddlModel.SelectedValue);

                        cClass.@class = carClass;

                        if (carID == 0)
                        {
                            cClass.carID = car.carID;
                            db.CarClasses.Add(cClass);
                        }

                        db.SaveChanges();
                    }
                } //End TRY
                catch (SystemException ex)
                {
                    Debug.WriteLine("ERROR MESSAGE: " + ex.Message);
                    Response.Redirect("/error.aspx");
                } //End CATCH
            }
            else
            {
                lblClass.Text = "Please pick a class";
            }
            Response.Redirect("/admin/account.aspx");
        }//End btnPost_Click
    } //End Class postAd
} //End namespace