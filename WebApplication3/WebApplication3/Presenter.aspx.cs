using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

namespace WebApplication3
{
    public partial class Presenter : System.Web.UI.Page
    {
        DataSet ds = new DataSet();
        MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.ConnectionString);

        bool liveImageBtnSelected;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Populate central image with newest picture
                string imagepath = getImagepathFromDBLatest();
                mainImage.ImageUrl = imagepath;

                // Populate gridview on the right with every picture
                getGridviewFromDBImages(0);
            }
            // Check the state of live-button and save to boolean
            if (ViewState["liveImageBtnSelected"] != null)
            {
                liveImageBtnSelected = bool.Parse(ViewState["liveImageBtnSelected"].ToString());
            }
            

        }



        protected void delImageBtn_Click(object sender, EventArgs e)
        {
         //   MySqlCommand command = conn.CreateCommand();
        //    command.CommandText = "DELETE FROM `lankaranta`.`images` WHERE ID=?image";
            //    command.Parameters.Add("?image", MySqlDbType.Int32).Value = VALITTU IMAGEID;
     //       conn.Open();
            // tässä jtn
        //    conn.Close();
        }



        protected void allImageBtn_Click(object sender, EventArgs e)
        // Get all pictures
        {
            getGridviewFromDBImages(0);
        }



        protected void movementImageBtn_Click(object sender, EventArgs e)
            // If movement detected in picture (movement boolean)
        {
            getGridviewFromDBImages(1);
        }



        protected void noMovementImageBtn_Click(object sender, EventArgs e)
            // Get all pictures where no movement detected
        {
            getGridviewFromDBImages(2);
        }



        protected void gridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gridView.Rows)
            {
                if (((CheckBox)row.FindControl("gridViewCheckBox")).Checked)
                {
                    
                    string datetime = gridView.DataKeys[row.RowIndex].Values["datetime"].ToString();
                    string imagepath = gridView.DataKeys[row.RowIndex].Values["imagepath"].ToString();
                    mainImage.ImageUrl = imagepath;
                    TitleDatetime.Text = datetime;
                    System.Diagnostics.Debug.WriteLine("paskaa");


                    ///
                    //
                    //
                    // TODO
                    // DataKeys tarvitaan myös datetime! ja sitte TitleDatetime -labeliin
                    //
                    //
                    ///



                }
            }
        }



        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='aquamarine';";
                e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
                e.Row.ToolTip = "Click last column for selecting this row.";
            }
        }



        protected void UpdateTimer_Tick(object sender, EventArgs e) {
            if (liveImageBtnSelected)
            {
                Tick();
            }
        }

        // If needed to call programmatically
        protected void Tick()
        {
            string imagepath = getImagepathFromDBLatest();
            mainImage.ImageUrl = imagepath;
        }



        protected string getImagepathFromDBLatest()
        {
            // This returns imagepath to latest image
            string imagepath = "";
            string sqlcommand = "SELECT * FROM `lankaranta`.`latest`;";
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = conn.CreateCommand();
            DataSet dataset = new DataSet();

            using (MySqlConnection connection = new MySqlConnection(Properties.Settings.Default.ConnectionString))
            {
                try
                {
                    connection.Open();
                    command.CommandText = sqlcommand;
                    adapter.SelectCommand = command;
                    adapter.Fill(dataset);
                    adapter.Dispose();
                }
                catch (Exception ex)
                {
                    // Error message here
                }
                finally
                {
                    connection.Close();
                    imagepath = dataset.Tables[0].Rows[0]["imagepath"].ToString();
                }
            }
            return imagepath;
        }



        protected void getGridviewFromDBImages(int movement)
        {
            // This function gets data from lankaranta.images and sets the data as datasource for gridview
            string sqlcommandAll = "SELECT * FROM `lankaranta`.`images`;"; // Movement = 0
            string sqlcommandMovement = "SELECT * FROM `lankaranta`.`images` WHERE movement=true;"; // Movement = 1
            string sqlcommandNoMovement = "SELECT * FROM `lankaranta`.`images` WHERE movement=false;"; // Movement = 2
   
            MySqlDataAdapter adapter = new MySqlDataAdapter();
            MySqlCommand command = conn.CreateCommand();
            DataSet dataset = new DataSet();

            if (movement == 0)
            {
                command.CommandText = sqlcommandAll;
            } else if (movement == 1)
            {
                command.CommandText = sqlcommandMovement;
            } else
            {
                command.CommandText = sqlcommandNoMovement;
            }

            using (MySqlConnection connection = new MySqlConnection(Properties.Settings.Default.ConnectionString))
            {
                try
                {
                    connection.Open();
                    adapter.SelectCommand = command;
                    adapter.Fill(dataset);
                    adapter.Dispose();
                }
                catch (Exception ex)
                {
                    // Error message here
                }
                finally
                {
                    connection.Close();
                    gridView.DataSource = dataset.Tables[0];
                    gridView.DataBind();
                }
            }
        }
        protected void liveImageBtn_Click(object sender, EventArgs e)
        {
            if (!liveImageBtnSelected || liveImageBtnSelected == null)
            {
                liveImageBtnSelected = true;
                ViewState["liveImageBtnSelected"] = "true";
                System.Diagnostics.Debug.WriteLine(liveImageBtnSelected);
                liveImageBtn.BackColor = Color.Green;
                // Instantly update picture to latest
                Tick();
            } else if (liveImageBtnSelected)
            {
                liveImageBtnSelected = false;
                ViewState["liveImageBtnSelected"] = "false";
                liveImageBtn.BackColor = Color.Gray;
                System.Diagnostics.Debug.WriteLine(liveImageBtnSelected);
            }
        }

        protected void sendEmailBtn_Click(object sender, EventArgs e)
        {

        }
    }
}