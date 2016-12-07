using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;

namespace WebApplication3
{
    public partial class Presenter : System.Web.UI.Page
    {
        // Backend has all main functionality
        Backend backend = new Backend();
        DataSet ds = new DataSet();
        MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.ConnectionString);

        bool liveImageBtnSelected;

        protected void Page_Load(object sender, EventArgs e)
        {
            // This happens when page loaded for the first time
            if (!IsPostBack)
            {
                // Make Live-mode selected on first page load
                liveImageBtnSelected = true;
                ViewState["liveImageBtnSelected"] = "true";
                liveImageBtn.BackColor = Color.Green;

                // Populate central image and datetime with latest image data
                Tick();

                // Populate gridview on the right with every picture
                populateGridview(0);
            }
            // Check the state of live-button and save to boolean
            if (ViewState["liveImageBtnSelected"] != null)
            {
                liveImageBtnSelected = bool.Parse(ViewState["liveImageBtnSelected"].ToString());
            }
            

        }



        // Delete selected image
        protected void delImageBtn_Click(object sender, EventArgs e)
        {
            if ((ViewState["selectedImageID"] != null) && ViewState["selectedImageImagepath"] != null)
            {
                int id = Int32.Parse(ViewState["selectedImageID"].ToString());
                string imagepath = ViewState["selectedImageImagepath"].ToString();
                backend.delImageFromDB(id, imagepath);
            }
            
        }



        // Get all pictures regardless of movement
        protected void allImageBtn_Click(object sender, EventArgs e)      
        {
            populateGridview(0);
        }



        // Get pictures where movement detected
        protected void movementImageBtn_Click(object sender, EventArgs e)
        {
            populateGridview(1);
        }



        // Get pictures where no movement detected
        protected void noMovementImageBtn_Click(object sender, EventArgs e)
        {
            populateGridview(2);
        }



        protected void gridView_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gridView.Rows)
            {
                if (row.RowIndex == gridView.SelectedIndex)
                {
                    row.BackColor = Color.Aquamarine;
                    row.ToolTip = string.Empty;

                    int ID = (int) gridView.DataKeys[row.RowIndex].Values["ID"];
                    string datetime = gridView.DataKeys[row.RowIndex].Values["datetime"].ToString();
                    string imagepath = gridView.DataKeys[row.RowIndex].Values["imagepath"].ToString();
                    Boolean movement = Convert.ToBoolean(gridView.DataKeys[row.RowIndex].Values["movement"]);
                    // Save selection data
                    setSelectedImage(ID, datetime, imagepath, movement);

                    mainImage.ImageUrl = imagepath;
                    TitleDatetime.Text = "<h2>" + datetime + "</h2>";
                } else
                {
                    row.BackColor = Color.White;
                    row.ToolTip = "Click to select this row";
                }
            }
        }

        
        
        // Save selected image data to ViewState
        protected void setSelectedImage(int id, string datetime, string imagepath, Boolean movement)
        {
            ViewState["selectedImageID"] = id;
            ViewState["selectedImageDatetime"] = datetime;
            ViewState["selectedImageImagepath"] = imagepath;
            ViewState["selectedImageMovement"] = movement;
        }
        


        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Attributes["onmouseover"] = "this.style.backgroundColor='aquamarine';";
                e.Row.Attributes["onmouseout"] = "this.style.backgroundColor='white';";
                e.Row.ToolTip = "Click to select this row";
                e.Row.Attributes["onclick"] = Page.ClientScript.GetPostBackClientHyperlink(gridView, "Select$" + e.Row.RowIndex);
            }
        }



        // Call Tick() from behind a timer
        protected void UpdateTimer_Tick(object sender, EventArgs e) {
            if (liveImageBtnSelected)
            {
                Tick();
            }
        }



        // If needed to call programmatically
        protected void Tick()
        {
            DataTable imagedata = backend.getImagepathFromDBLatest();
            mainImage.ImageUrl = imagedata.Rows[0]["imagepath"].ToString();
            TitleDatetime.Text = imagedata.Rows[0]["datetime"].ToString();
        }
        


        // liveImageBtn clicked, toggle live-state of latest picture
        protected void liveImageBtn_Click(object sender, EventArgs e)
        {
            if (!liveImageBtnSelected )
            {
                liveImageBtnSelected = true;
                ViewState["liveImageBtnSelected"] = "true";
                //System.Diagnostics.Debug.WriteLine(liveImageBtnSelected);
                liveImageBtn.BackColor = Color.Green;
                // Instantly update picture to latest
                Tick();
            } else if (liveImageBtnSelected)
            {
                liveImageBtnSelected = false;
                ViewState["liveImageBtnSelected"] = "false";
                liveImageBtn.BackColor = Color.Gray;
                //System.Diagnostics.Debug.WriteLine(liveImageBtnSelected);
            }
        }



        // Send selected image as email
        protected void sendEmailBtn_Click(object sender, EventArgs e)
        {
            if ((ViewState["selectedImageDatetime"] != null) 
                && (ViewState["selectedImageImagepath"] != null)
                && (ViewState["selectedImageMovement"] != null))
            {
                string datetime = ViewState["selectedImageDatetime"].ToString();
                string imagepath = ViewState["selectedImageImagepath"].ToString();
                Boolean movement = Boolean.Parse(ViewState["selectedImageMovement"].ToString());
                // Call backend -function
                backend.sendSelectedImageToEmail(datetime, imagepath, movement);
                
                
            }

        }



        // Function used to show data in gridview
        protected void populateGridview(int mode)
        {
            gridView.DataSource = backend.getDataTableFromDBImages(mode);
            gridView.DataBind();
        }



    }
}