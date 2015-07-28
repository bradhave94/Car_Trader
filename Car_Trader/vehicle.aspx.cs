/*
    Authors: Jacob Amaral & Bradley Haveman
    File Name: vehicle.aspx
    File Description: User can use this page to search for a car.
    Project name: Car Trader
    Project Description: Car Trader is an online car selling/buying site where you can find the perfect car for you, or sell your old car.
    Date Updated: July 27, 2015
 */

using Car_Trader.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Car_Trader
{
    public partial class vehicle : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Default sort is by the car make and ascending
                Session["SortColumn"] = "make";
                Session["SortDirection"] = "ASC";

                pnlCars.Visible = false;
                //Fill the make dropdown
                GetCarMakes();

                //Add please select a make to the dropdown
                ddlMakeSearch.Items.Insert(0, new ListItem("Please select a make", ""));

                //Add please select a model to the dropdown
                ddlModelSearch.Items.Insert(0, new ListItem("Please select a model", ""));

                //Disable the model dropdown
                ddlModelSearch.Enabled = false;
                ddlModelSearch.BackColor = System.Drawing.Color.LightGray;

                //Fill the fuel type dropdown
                GetFuelTypes();

                ddlFuelSearch.Items.Insert(0, new ListItem("Please select a fuel type", ""));
            } //End IF
        } //End Page_Load

        //Populate the dropdown for the car makes
        protected void GetCarMakes()
        {
            try
            {
                //Connect to the db
                using (COMP2007Entities db = new COMP2007Entities())
                {
                    //Get the car makes from the db
                    var makes = (from d in db.CarMakes
                                 select d);

                    //Bind them the grid
                    ddlMakeSearch.DataSource = makes.ToList();
                    ddlMakeSearch.DataBind();
                } //End using
            } //End TRY
            //Catch any error and redirect to the error page
            catch (SystemException ex)
            {
                Response.Redirect("/error.aspx");
            } //End CATCH
        } //End GetCarMakes

        //Populate the dropdown for fuel types
        protected void GetFuelTypes()
        {
            try
            {
                //Connect to the db
                using (COMP2007Entities db = new COMP2007Entities())
                {
                    //Get the fuel tyoes from the db
                    var fuels = (from d in db.CarEngines
                                 orderby d.fuelType
                                 select d).Distinct();

                    //try to bind them the the grid
                    try
                    {
                        ddlFuelSearch.DataSource = fuels.ToList().Take(3);
                        ddlFuelSearch.DataBind();
                    } //End TRY
                    catch (HttpException ex)
                    {
                        Console.WriteLine(ex.Message);
                    } //End CATCH
                } //end using
            } //End TRY
            //Catch any error and redirect to the error page
            catch (SystemException ex)
            {
                Response.Redirect("/error.aspx");
            } //End CATCH
        } //End GetFuelTypes

        //When the make dropdown index changes
        protected void ddlMakeSearch_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlCars.Visible = false;

            //If its anything but 0
            if (ddlMakeSearch.SelectedIndex != 0)
            {
                //Enable the model dropdwon
                ddlModelSearch.Enabled = true;
                ddlModelSearch.BackColor = System.Drawing.Color.White;

                DropDownList makeDropdown = (DropDownList)sender;
                //Populate car models based on selected car make
                Int32 makeID = Int32.Parse(makeDropdown.SelectedValue);

                try
                {
                    //Connect to the db
                    using (COMP2007Entities db = new COMP2007Entities())
                    {
                        //get the car models from the db
                        var models = (from d in db.CarModels
                                      where d.makeID == makeID
                                      select d);

                        //Bind them to the grid
                        ddlModelSearch.DataSource = models.ToList();
                        ddlModelSearch.DataBind();
                    } //End using
                } //End TRY
                //Catch any error and redirect to the error page
                catch (SystemException ex)
                {
                    Response.Redirect("/error.aspx");
                } //End CATCH
            } //End If

            //If no car make is selected, disable the car model dropdown
            else
            {
                ddlModelSearch.Items.Insert(0, new ListItem("Please select a model", ""));
                ddlModelSearch.Enabled = false;
                ddlModelSearch.BackColor = System.Drawing.Color.LightGray;
                ddlModelSearch.SelectedIndex = 0;
            } //End ELSE
        } //End ddlMakeSearch_SelectedIndexChanged

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
        } //End GetLocation

        //When the search button is clicked
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Which way are we sorting and by what column
            String SortString = Session["SortColumn"].ToString() + " " + Session["SortDirection"].ToString();
            //Show the cars grid
            pnlCars.Visible = true;
            try
            {
                //if the make is selected
                Int32 makeID = Int32.Parse(ddlMakeSearch.SelectedValue.ToString());
                using (COMP2007Entities db = new COMP2007Entities())
                {
                    try
                    {
                        //if a model is selected
                        Int32 modelID = Int32.Parse(ddlModelSearch.SelectedValue.ToString());
                        switch (ddlFuelSearch.SelectedIndex)
                        {
                            //if fuel is selected
                            case 1:
                            case 2:
                            case 3:
                                Int32 engineType = Int32.Parse(ddlFuelSearch.SelectedValue.ToString());
                                //Check if the user searched a location
                                if (txtLocation.Text != "")
                                {
                                    //Check if they put a something in min price
                                    if (txtMinPrice.Text != "")
                                    {
                                        Int32 minPrice = Int32.Parse(txtMinPrice.Text);
                                        //if they are searching just new cars
                                        if (chbNew.Checked && !chbUsed.Checked)
                                        {
                                            //A model was selected
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where make.makeID == makeID
                                                       && model.modelID == modelID
                                                       && a.cost >= minPrice
                                                       && a.@new == true
                                                       && a.location == txtLocation.Text
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });

                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        //Searching used cars
                                        else if (!chbNew.Checked && chbUsed.Checked)
                                        {
                                            //A model was selected
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where make.makeID == makeID
                                                       && model.modelID == modelID
                                                       && a.cost >= minPrice
                                                       && a.@new == false
                                                       && a.location == txtLocation.Text
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        //Searching both new and used
                                        else if (chbNew.Checked && chbUsed.Checked)
                                        {
                                            //A model was selected
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where make.makeID == makeID
                                                       && model.modelID == modelID
                                                       && a.cost >= minPrice
                                                       && a.location == txtLocation.Text
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        //Min and Max price are selected
                                        if (txtMaxPrice.Text != "")
                                        {
                                            Int32 maxPrice = Int32.Parse(txtMaxPrice.Text);
                                            //A model was selected
                                            if (chbNew.Checked && !chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.cost >= minPrice && a.cost <= maxPrice
                                                           && a.@new == true
                                                           && a.location == txtLocation.Text
                                                           && a.engineID == engineType
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (!chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.cost >= minPrice && a.cost <= maxPrice
                                                           && a.@new == false
                                                           && a.location == txtLocation.Text
                                                           && a.engineID == engineType
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.cost >= minPrice && a.cost <= maxPrice
                                                           && a.location == txtLocation.Text
                                                           && a.engineID == engineType
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (txtMaxPrice.Text != "")
                                        {
                                            Int32 maxPrice = Int32.Parse(txtMaxPrice.Text);
                                            //A model was selected
                                            if (chbNew.Checked && !chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.cost <= maxPrice
                                                           && a.@new == true
                                                           && a.location == txtLocation.Text
                                                           && a.engineID == engineType
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (!chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.cost <= maxPrice
                                                           && a.@new == false
                                                           && a.location == txtLocation.Text
                                                           && a.engineID == engineType
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && a.cost <= maxPrice
                                                           && model.modelID == modelID
                                                           && a.location == txtLocation.Text
                                                           && a.engineID == engineType
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                        }
                                        else
                                        {
                                            //A model was selected
                                            if (chbNew.Checked && !chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.@new == true
                                                           && a.location == txtLocation.Text
                                                           && a.engineID == engineType
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (!chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.@new == false
                                                           && a.location == txtLocation.Text
                                                           && a.engineID == engineType
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.location == txtLocation.Text
                                                           && a.engineID == engineType
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //Check if they put a something in min price
                                    if (txtMinPrice.Text != "")
                                    {
                                        Int32 minPrice = Int32.Parse(txtMinPrice.Text);
                                        if (chbNew.Checked && !chbUsed.Checked)
                                        {
                                            //A model was selected
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where make.makeID == makeID
                                                       && model.modelID == modelID
                                                       && a.cost >= minPrice
                                                       && a.@new == true
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (!chbNew.Checked && chbUsed.Checked)
                                        {
                                            //A model was selected
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where make.makeID == makeID
                                                       && model.modelID == modelID
                                                       && a.cost >= minPrice
                                                       && a.@new == false
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (chbNew.Checked && chbUsed.Checked)
                                        {
                                            //A model was selected
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where make.makeID == makeID
                                                       && model.modelID == modelID
                                                       && a.cost >= minPrice
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        //Min and Max price are selected
                                        if (txtMaxPrice.Text != "")
                                        {
                                            Int32 maxPrice = Int32.Parse(txtMaxPrice.Text);
                                            //A model was selected
                                            if (chbNew.Checked && !chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.cost >= minPrice && a.cost <= maxPrice
                                                           && a.@new == true
                                                           && a.engineID == engineType
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (!chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.cost >= minPrice && a.cost <= maxPrice
                                                           && a.@new == false
                                                           && a.engineID == engineType
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.cost >= minPrice && a.cost <= maxPrice
                                                           && a.engineID == engineType
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (txtMaxPrice.Text != "")
                                        {
                                            Int32 maxPrice = Int32.Parse(txtMaxPrice.Text);
                                            //A model was selected
                                            if (chbNew.Checked && !chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.cost <= maxPrice
                                                           && a.@new == true
                                                           && a.engineID == engineType
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (!chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.cost <= maxPrice
                                                           && a.@new == false
                                                           && a.engineID == engineType
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && a.cost <= maxPrice
                                                           && model.modelID == modelID
                                                           && a.engineID == engineType
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                        }
                                        else
                                        {
                                            //A model was selected
                                            if (chbNew.Checked && !chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.engineID == engineType
                                                           && a.@new == true
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (!chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.engineID == engineType
                                                           && a.@new == false
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.engineID == engineType
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                        }
                                    }
                                } break;
                            //No fuel type was selected, search all fuel types
                            default:
                                if (txtLocation.Text != "")
                                {
                                    //Check if they put a something in min price
                                    if (txtMinPrice.Text != "")
                                    {
                                        Int32 minPrice = Int32.Parse(txtMinPrice.Text);
                                        if (chbNew.Checked && !chbUsed.Checked)
                                        {
                                            //A model was selected
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where make.makeID == makeID
                                                       && model.modelID == modelID
                                                       && a.cost >= minPrice
                                                       && a.@new == true
                                                       && a.location == txtLocation.Text
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (!chbNew.Checked && chbUsed.Checked)
                                        {
                                            //A model was selected
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where make.makeID == makeID
                                                       && model.modelID == modelID
                                                       && a.cost >= minPrice
                                                       && a.@new == false
                                                       && a.location == txtLocation.Text
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (chbNew.Checked && chbUsed.Checked)
                                        {
                                            //A model was selected
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where make.makeID == makeID
                                                       && model.modelID == modelID
                                                       && a.cost >= minPrice
                                                       && a.location == txtLocation.Text
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        //Min and Max price are selected
                                        if (txtMaxPrice.Text != "")
                                        {
                                            Int32 maxPrice = Int32.Parse(txtMaxPrice.Text);
                                            //A model was selected
                                            if (chbNew.Checked && !chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.cost >= minPrice && a.cost <= maxPrice
                                                           && a.@new == true
                                                           && a.location == txtLocation.Text
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (!chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.cost >= minPrice && a.cost <= maxPrice
                                                           && a.@new == false
                                                           && a.location == txtLocation.Text
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.cost >= minPrice && a.cost <= maxPrice
                                                           && a.location == txtLocation.Text
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (txtMaxPrice.Text != "")
                                        {
                                            Int32 maxPrice = Int32.Parse(txtMaxPrice.Text);
                                            //A model was selected
                                            if (chbNew.Checked && !chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.cost <= maxPrice
                                                           && a.@new == true
                                                           && a.location == txtLocation.Text
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (!chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.cost <= maxPrice
                                                           && a.@new == false
                                                           && a.location == txtLocation.Text
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && a.cost <= maxPrice
                                                           && model.modelID == modelID
                                                           && a.location == txtLocation.Text
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                        }
                                        else
                                        {
                                            //A model was selected
                                            if (chbNew.Checked && !chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.@new == true
                                                           && a.location == txtLocation.Text
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (!chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.@new == false
                                                           && a.location == txtLocation.Text
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.location == txtLocation.Text
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    //Check if they put a something in min price
                                    if (txtMinPrice.Text != "")
                                    {
                                        Int32 minPrice = Int32.Parse(txtMinPrice.Text);
                                        if (chbNew.Checked && !chbUsed.Checked)
                                        {
                                            //A model was selected
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where make.makeID == makeID
                                                       && model.modelID == modelID
                                                       && a.cost >= minPrice
                                                       && a.@new == true
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (!chbNew.Checked && chbUsed.Checked)
                                        {
                                            //A model was selected
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where make.makeID == makeID
                                                       && model.modelID == modelID
                                                       && a.cost >= minPrice
                                                       && a.@new == false
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (chbNew.Checked && chbUsed.Checked)
                                        {
                                            //A model was selected
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where make.makeID == makeID
                                                       && model.modelID == modelID
                                                       && a.cost >= minPrice
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        //Min and Max price are selected
                                        if (txtMaxPrice.Text != "")
                                        {
                                            Int32 maxPrice = Int32.Parse(txtMaxPrice.Text);
                                            //A model was selected
                                            if (chbNew.Checked && !chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.cost >= minPrice && a.cost <= maxPrice
                                                           && a.@new == true
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (!chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.cost >= minPrice && a.cost <= maxPrice
                                                           && a.@new == false
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.cost >= minPrice && a.cost <= maxPrice
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (txtMaxPrice.Text != "")
                                        {
                                            Int32 maxPrice = Int32.Parse(txtMaxPrice.Text);
                                            //A model was selected
                                            if (chbNew.Checked && !chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.cost <= maxPrice
                                                           && a.@new == true
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (!chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.cost <= maxPrice
                                                           && a.@new == false
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && a.cost <= maxPrice
                                                           && model.modelID == modelID
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                        }
                                        else
                                        {
                                            //A model was selected
                                            if (chbNew.Checked && !chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.@new == true
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (!chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           && a.@new == false
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                            else if (chbNew.Checked && chbUsed.Checked)
                                            {
                                                //A model was selected
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where make.makeID == makeID
                                                           && model.modelID == modelID
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                        }
                                    }
                                } break;
                        }
                    }
                    //This catch runs if a model was not selected
                    catch (FormatException f)
                    {
                        switch (ddlFuelSearch.SelectedIndex)
                        {
                            case 1:
                            case 2:
                            case 3:
                                Int32 engineType = Int32.Parse(ddlFuelSearch.SelectedValue.ToString());
                                if (txtLocation.Text != "")
                                {
                                    //A make was selected
                                    if (chbNew.Checked && !chbUsed.Checked)
                                    {
                                        //A model was selected
                                        var car = (from a in db.Cars
                                                   join carclass in db.CarClasses on a.carID equals carclass.classID
                                                   join model in db.CarModels on carclass.modelID equals model.modelID
                                                   join make in db.CarMakes on model.makeID equals make.makeID
                                                   where make.makeID == makeID
                                                   && a.@new == true
                                                   && a.location == txtLocation.Text
                                                   && a.engineID == engineType
                                                   select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                        grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                        grdCars.DataBind();
                                    }
                                    else if (!chbNew.Checked && chbUsed.Checked)
                                    {
                                        //A model was selected
                                        var car = (from a in db.Cars
                                                   join carclass in db.CarClasses on a.carID equals carclass.classID
                                                   join model in db.CarModels on carclass.modelID equals model.modelID
                                                   join make in db.CarMakes on model.makeID equals make.makeID
                                                   where make.makeID == makeID
                                                   && a.@new == false
                                                   && a.location == txtLocation.Text
                                                   && a.engineID == engineType

                                                   select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                        grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                        grdCars.DataBind();
                                    }
                                    else if (chbNew.Checked && chbUsed.Checked)
                                    {
                                        //A make was selected
                                        var car = (from a in db.Cars
                                                   join carclass in db.CarClasses on a.carID equals carclass.classID
                                                   join model in db.CarModels on carclass.modelID equals model.modelID
                                                   join make in db.CarMakes on model.makeID equals make.makeID
                                                   where make.makeID == makeID
                                                   && a.location == txtLocation.Text
                                                   && a.engineID == engineType
                                                   select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                        grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                        grdCars.DataBind();
                                    }
                                }
                                else
                                {
                                    //A make was selected
                                    if (chbNew.Checked && !chbUsed.Checked)
                                    {
                                        //A model was selected
                                        var car = (from a in db.Cars
                                                   join carclass in db.CarClasses on a.carID equals carclass.classID
                                                   join model in db.CarModels on carclass.modelID equals model.modelID
                                                   join make in db.CarMakes on model.makeID equals make.makeID
                                                   where make.makeID == makeID
                                                   && a.@new == true
                                                   && a.engineID == engineType
                                                   select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                        grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                        grdCars.DataBind();
                                    }
                                    else if (!chbNew.Checked && chbUsed.Checked)
                                    {
                                        //A model was selected
                                        var car = (from a in db.Cars
                                                   join carclass in db.CarClasses on a.carID equals carclass.classID
                                                   join model in db.CarModels on carclass.modelID equals model.modelID
                                                   join make in db.CarMakes on model.makeID equals make.makeID
                                                   where make.makeID == makeID
                                                   && a.@new == false
                                                   && a.engineID == engineType
                                                   select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                        grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                        grdCars.DataBind();
                                    }
                                    else if (chbNew.Checked && chbUsed.Checked)
                                    {
                                        //A make was selected
                                        var car = (from a in db.Cars
                                                   join carclass in db.CarClasses on a.carID equals carclass.classID
                                                   join model in db.CarModels on carclass.modelID equals model.modelID
                                                   join make in db.CarMakes on model.makeID equals make.makeID
                                                   where make.makeID == makeID
                                                   && a.engineID == engineType
                                                   select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                        grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                        grdCars.DataBind();
                                    }
                                }

                                break;

                            default: if (txtLocation.Text != "")
                                {
                                    //A make was selected
                                    if (chbNew.Checked && !chbUsed.Checked)
                                    {
                                        //A model was selected
                                        var car = (from a in db.Cars
                                                   join carclass in db.CarClasses on a.carID equals carclass.classID
                                                   join model in db.CarModels on carclass.modelID equals model.modelID
                                                   join make in db.CarMakes on model.makeID equals make.makeID
                                                   where make.makeID == makeID
                                                   && a.@new == true
                                                   && a.location == txtLocation.Text
                                                   select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                        grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                        grdCars.DataBind();
                                    }
                                    else if (!chbNew.Checked && chbUsed.Checked)
                                    {
                                        //A model was selected
                                        var car = (from a in db.Cars
                                                   join carclass in db.CarClasses on a.carID equals carclass.classID
                                                   join model in db.CarModels on carclass.modelID equals model.modelID
                                                   join make in db.CarMakes on model.makeID equals make.makeID
                                                   where make.makeID == makeID
                                                   && a.@new == false
                                                   && a.location == txtLocation.Text
                                                   select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                        grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                        grdCars.DataBind();
                                    }
                                    else if (chbNew.Checked && chbUsed.Checked)
                                    {
                                        //A make was selected
                                        var car = (from a in db.Cars
                                                   join carclass in db.CarClasses on a.carID equals carclass.classID
                                                   join model in db.CarModels on carclass.modelID equals model.modelID
                                                   join make in db.CarMakes on model.makeID equals make.makeID
                                                   where make.makeID == makeID
                                                   && a.location == txtLocation.Text
                                                   select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                        grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                        grdCars.DataBind();
                                    }
                                }
                                else
                                {
                                    //A make was selected
                                    if (chbNew.Checked && !chbUsed.Checked)
                                    {
                                        //A model was selected
                                        var car = (from a in db.Cars
                                                   join carclass in db.CarClasses on a.carID equals carclass.classID
                                                   join model in db.CarModels on carclass.modelID equals model.modelID
                                                   join make in db.CarMakes on model.makeID equals make.makeID
                                                   where make.makeID == makeID
                                                   && a.@new == true
                                                   select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                        grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                        grdCars.DataBind();
                                    }
                                    else if (!chbNew.Checked && chbUsed.Checked)
                                    {
                                        //A model was selected
                                        var car = (from a in db.Cars
                                                   join carclass in db.CarClasses on a.carID equals carclass.classID
                                                   join model in db.CarModels on carclass.modelID equals model.modelID
                                                   join make in db.CarMakes on model.makeID equals make.makeID
                                                   where make.makeID == makeID
                                                   && a.@new == false
                                                   select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                        grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                        grdCars.DataBind();
                                    }
                                    else if (chbNew.Checked && chbUsed.Checked)
                                    {
                                        //A make was selected
                                        var car = (from a in db.Cars
                                                   join carclass in db.CarClasses on a.carID equals carclass.classID
                                                   join model in db.CarModels on carclass.modelID equals model.modelID
                                                   join make in db.CarMakes on model.makeID equals make.makeID
                                                   where make.makeID == makeID
                                                   select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                        grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                        grdCars.DataBind();
                                    }
                                }

                                break;
                        }
                    }
                }
            }
            //This catch runs if a make was not selected,search all makes
            catch (FormatException f)
            {
                using (COMP2007Entities db = new COMP2007Entities())
                {
                    switch (ddlFuelSearch.SelectedIndex)
                    {
                        case 1:
                        case 2:
                        case 3:
                            Int32 engineType = Int32.Parse(ddlFuelSearch.SelectedValue.ToString());
                            if (txtLocation.Text != "")
                            {
                                //A make was not selected, show all makes.
                                if (txtMinPrice.Text != "")
                                {
                                    Int32 minPrice = Int32.Parse(txtMinPrice.Text);

                                    //Min and Max price are selected
                                    if (txtMaxPrice.Text != "")
                                    {
                                        Int32 maxPrice = Int32.Parse(txtMaxPrice.Text);

                                        if (chbNew.Checked && !chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars

                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice && a.cost <= maxPrice
                                                       && a.@new == true
                                                       && a.location == txtLocation.Text
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });

                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (!chbNew.Checked && chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice && a.cost <= maxPrice
                                                       && a.@new == false
                                                       && a.location == txtLocation.Text
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });

                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice && a.cost <= maxPrice
                                                       && a.location == txtLocation.Text
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                    }
                                    //Only min price was selected
                                    else
                                    {
                                        if (chbNew.Checked && !chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice
                                                       && a.@new == true
                                                       && a.location == txtLocation.Text
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (!chbNew.Checked && chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice
                                                       && a.@new == false
                                                       && a.location == txtLocation.Text
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice
                                                       && a.location == txtLocation.Text
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                    }
                                }
                                //No price was selected
                                else
                                {
                                    if (txtMaxPrice.Text != "")
                                    {
                                        Int32 maxPrice = Int32.Parse(txtMaxPrice.Text);
                                        if (chbNew.Checked && !chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost <= maxPrice
                                                       && a.@new == true
                                                       && a.location == txtLocation.Text
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (!chbNew.Checked && chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost <= maxPrice
                                                       && a.@new == false
                                                       && a.location == txtLocation.Text
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else
                                        {
                                            if (chbNew.Checked && chbUsed.Checked)
                                            {
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where a.cost <= maxPrice
                                                           && a.location == txtLocation.Text
                                                           && a.engineID == engineType
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (chbNew.Checked && !chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.@new == true
                                                       && a.location == txtLocation.Text
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (!chbNew.Checked && chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.@new == false
                                                       && a.location == txtLocation.Text
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else
                                        {
                                            if (chbNew.Checked && chbUsed.Checked)
                                            {
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where a.location == txtLocation.Text
                                                           && a.engineID == engineType
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //A make was not selected, show all makes.
                                if (txtMinPrice.Text != "")
                                {
                                    Int32 minPrice = Int32.Parse(txtMinPrice.Text);
                                    //Min and Max price are selected
                                    if (txtMaxPrice.Text != "")
                                    {
                                        Int32 maxPrice = Int32.Parse(txtMaxPrice.Text);
                                        if (chbNew.Checked && !chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice && a.cost <= maxPrice
                                                       && a.@new == true
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (!chbNew.Checked && chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice && a.cost <= maxPrice
                                                       && a.@new == false
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice && a.cost <= maxPrice
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                    }
                                    //Only min price was selected
                                    else
                                    {
                                        if (chbNew.Checked && !chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice
                                                       && a.@new == true
                                                        && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (!chbNew.Checked && chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice
                                                       && a.@new == false
                                                        && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice
                                                        && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                    }
                                }
                                //No price was selected
                                else
                                {
                                    if (txtMaxPrice.Text != "")
                                    {
                                        Int32 maxPrice = Int32.Parse(txtMaxPrice.Text);
                                        if (chbNew.Checked && !chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost <= maxPrice
                                                       && a.@new == true
                                                        && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (!chbNew.Checked && chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost <= maxPrice
                                                       && a.@new == false
                                                        && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else
                                        {
                                            if (chbNew.Checked && chbUsed.Checked)
                                            {
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where a.cost <= maxPrice
                                                           && a.engineID == engineType
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (chbNew.Checked && !chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.@new == true
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (!chbNew.Checked && chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.@new == false
                                                       && a.engineID == engineType
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else
                                        {
                                            if (chbNew.Checked && chbUsed.Checked)
                                            {
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where a.engineID == engineType
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                        }
                                    }
                                }
                            }
                            break;

                        default:
                            if (txtLocation.Text != "")
                            {
                                //A make was not selected, show all makes.
                                if (txtMinPrice.Text != "")
                                {
                                    Int32 minPrice = Int32.Parse(txtMinPrice.Text);

                                    //Min and Max price are selected
                                    if (txtMaxPrice.Text != "")
                                    {
                                        Int32 maxPrice = Int32.Parse(txtMaxPrice.Text);

                                        if (chbNew.Checked && !chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars

                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice && a.cost <= maxPrice
                                                       && a.@new == true
                                                       && a.location == txtLocation.Text
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });

                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (!chbNew.Checked && chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice && a.cost <= maxPrice
                                                       && a.@new == false
                                                       && a.location == txtLocation.Text
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });

                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice && a.cost <= maxPrice
                                                       && a.location == txtLocation.Text
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                    }
                                    //Only min price was selected
                                    else
                                    {
                                        if (chbNew.Checked && !chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice
                                                       && a.@new == true
                                                       && a.location == txtLocation.Text
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (!chbNew.Checked && chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice
                                                       && a.@new == false
                                                       && a.location == txtLocation.Text
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice
                                                       && a.location == txtLocation.Text
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                    }
                                }
                                //No price was selected
                                else
                                {
                                    if (txtMaxPrice.Text != "")
                                    {
                                        Int32 maxPrice = Int32.Parse(txtMaxPrice.Text);
                                        if (chbNew.Checked && !chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost <= maxPrice
                                                       && a.@new == true
                                                       && a.location == txtLocation.Text
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (!chbNew.Checked && chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost <= maxPrice
                                                       && a.@new == false
                                                       && a.location == txtLocation.Text
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else
                                        {
                                            if (chbNew.Checked && chbUsed.Checked)
                                            {
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where a.cost <= maxPrice
                                                           && a.location == txtLocation.Text
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (chbNew.Checked && !chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.@new == true
                                                       && a.location == txtLocation.Text
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (!chbNew.Checked && chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.@new == false
                                                       && a.location == txtLocation.Text
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else
                                        {
                                            if (chbNew.Checked && chbUsed.Checked)
                                            {
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where a.location == txtLocation.Text
                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                //A make was not selected, show all makes.
                                if (txtMinPrice.Text != "")
                                {
                                    Int32 minPrice = Int32.Parse(txtMinPrice.Text);
                                    //Min and Max price are selected
                                    if (txtMaxPrice.Text != "")
                                    {
                                        Int32 maxPrice = Int32.Parse(txtMaxPrice.Text);
                                        if (chbNew.Checked && !chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice && a.cost <= maxPrice
                                                       && a.@new == true
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (!chbNew.Checked && chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice && a.cost <= maxPrice
                                                       && a.@new == false
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice && a.cost <= maxPrice

                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                    }
                                    //Only min price was selected
                                    else
                                    {
                                        if (chbNew.Checked && !chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice
                                                       && a.@new == true
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (!chbNew.Checked && chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice
                                                       && a.@new == false
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost >= minPrice

                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                    }
                                }
                                //No price was selected
                                else
                                {
                                    if (txtMaxPrice.Text != "")
                                    {
                                        Int32 maxPrice = Int32.Parse(txtMaxPrice.Text);
                                        if (chbNew.Checked && !chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost <= maxPrice
                                                       && a.@new == true
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (!chbNew.Checked && chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.cost <= maxPrice
                                                       && a.@new == false
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else
                                        {
                                            if (chbNew.Checked && chbUsed.Checked)
                                            {
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID
                                                           where a.cost <= maxPrice

                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (chbNew.Checked && !chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.@new == true
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else if (!chbNew.Checked && chbUsed.Checked)
                                        {
                                            var car = (from a in db.Cars
                                                       join carclass in db.CarClasses on a.carID equals carclass.classID
                                                       join model in db.CarModels on carclass.modelID equals model.modelID
                                                       join make in db.CarMakes on model.makeID equals make.makeID
                                                       where a.@new == false
                                                       select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                            grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                            grdCars.DataBind();
                                        }
                                        else
                                        {
                                            if (chbNew.Checked && chbUsed.Checked)
                                            {
                                                var car = (from a in db.Cars
                                                           join carclass in db.CarClasses on a.carID equals carclass.classID
                                                           join model in db.CarModels on carclass.modelID equals model.modelID
                                                           join make in db.CarMakes on model.makeID equals make.makeID

                                                           select new { make.make, model.model, carclass.@class, a.modelYear, a.transmission, a.@new, a.cost, a.location, a.kilometer });
                                                grdCars.DataSource = car.AsQueryable().OrderBy(SortString).ToList();
                                                grdCars.DataBind();
                                            }
                                        }
                                    }
                                }
                            } break;
                    }
                }
                //Scroll down to the grid after searching
                grdCars.Focus();
                ClientScript.RegisterStartupScript(this.GetType(), "hash", "location.hash = '#grid';", true);
            }
        }

        //When a user clicks a different page, show the rows in that page
        protected void grdCars_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCars.PageIndex = e.NewPageIndex;
            try
            {
                btnSearch_Click(sender, e);
            }
            catch (SystemException ex)
            {
                Response.Redirect("/error.aspx");
            }
        }

        //Switch the sorting for ascending to descending and vice versa on click
        protected void grdCars_Sorting(object sender, GridViewSortEventArgs e)
        {
            //get the column to sort by
            Session["SortColumn"] = e.SortExpression;

            //toggle the direction
            if (Session["SortDirection"].ToString() == "ASC")
                Session["SortDirection"] = "DESC";
            else
                Session["SortDirection"] = "ASC";

            //reload grid
            btnSearch_Click(sender, e);
        }

        //Show an arrow for the sorted column, up for ascending and down for descending
        protected void grdCars_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (IsPostBack)
            {
                if (e.Row.RowType == DataControlRowType.Header)
                {
                    System.Web.UI.WebControls.Image SortImage = new System.Web.UI.WebControls.Image();

                    for (int i = 0; i <= grdCars.Columns.Count - 1; i++)
                    {
                        if (grdCars.Columns[i].SortExpression == Session["SortColumn"].ToString())
                        {
                            if (Session["SortDirection"].ToString() == "DESC")
                            {
                                SortImage.ImageUrl = "images/desc.jpg";
                                SortImage.AlternateText = "Sort Descending";
                            }
                            else
                            {
                                SortImage.ImageUrl = "images/asc.jpg";
                                SortImage.AlternateText = "Sort Ascending";
                            }

                            e.Row.Cells[i].Controls.Add(SortImage);
                        }
                    }
                }
            }
        }
    } //End Class vehicles
} //End namespace