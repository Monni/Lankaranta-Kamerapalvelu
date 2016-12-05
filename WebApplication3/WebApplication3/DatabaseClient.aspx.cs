using System;
using System.IO;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Drawing;

namespace WebApplication3
{
    public partial class DatabaseClient : System.Web.UI.Page
    {
        MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.ConnectionString);

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void UploadButton_Click(object sender, EventArgs e)
        {
            if (FileUploadControl.HasFile)
            {
                try
                {
                    if (FileUploadControl.PostedFile.ContentType == "image/jpeg")
                    {
                        if (FileUploadControl.PostedFile.ContentLength < 1024000)
                        {
                            string filename = Path.GetFileName(FileUploadControl.FileName);
                            FileUploadControl.SaveAs(Server.MapPath("~/Uploads/") + filename);
                            StatusLabel.Text = "Upload status: File uploaded!";
                            // Open DataBase connection and send to DB
                            sendToDB("~/Uploads/" + filename);
                        }
                        else
                            StatusLabel.Text = "Upload status: The file has to be less than 1000 kb!";
                    }
                    else
                        StatusLabel.Text = "Upload status: Only JPEG files are accepted!";
                }
                catch (Exception ex)
                {
                    StatusLabel.Text = "Upload status: The file could not be uploaded. The following error occured: " + ex.Message;
                }
            }
        }


        protected void sendToDB(string fpath)
        {
            String sqlImagesInsert = "INSERT INTO images (datetime, imagepath) VALUES (now(), ?filepath);";
            String sqlLatestInsert = "UPDATE latest SET datetime=now(), imagepath=?filepath WHERE ID=1;";

            using (MySqlConnection connection = new MySqlConnection(Properties.Settings.Default.ConnectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command1 = new MySqlCommand(sqlImagesInsert, connection))
                    {
                        command1.Parameters.Add("?filepath", MySqlDbType.VarChar).Value = fpath;
                        command1.ExecuteNonQuery();
                    }
                    using (MySqlCommand command2 = new MySqlCommand(sqlLatestInsert, connection))
                    {
                        command2.Parameters.Add("?filepath", MySqlDbType.VarChar).Value = fpath;
                        command2.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Error message here
                }
                finally
                {
                    connection.Close();
                }
            }




        }
    }
}