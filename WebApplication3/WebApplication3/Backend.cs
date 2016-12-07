using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;

namespace WebApplication3
{
    public class Backend
    {



        public DataTable getDataTableFromDBImages(int movement)
        {
            MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.ConnectionString);
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
            }
            else if (movement == 1)
            {
                command.CommandText = sqlcommandMovement;
            }
            else
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
                    throw new Exception("MySQL error!", ex);
                }
                finally
                {
                    connection.Close();
                }
               
            }
            return dataset.Tables[0];
        }



        public DataTable getImagepathFromDBLatest()
        {
            MySqlConnection conn = new MySqlConnection(Properties.Settings.Default.ConnectionString);
            DataTable datatable;
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
                    throw new Exception("MySQL error!", ex);
                }
                finally
                {
                    connection.Close();
                    datatable = dataset.Tables[0];
                }
            }
            return datatable;
        }



        // Delete selected image from database
        public void delImageFromDB(int id, string imagepath)
        {
            string sqlcommand = "DELETE FROM `lankaranta`.`images` WHERE id=?id;";

            using (MySqlConnection connection = new MySqlConnection(Properties.Settings.Default.ConnectionString))
            {
                try
                {
                    MySqlCommand command = connection.CreateCommand();
                    // Open connection to DB
                    connection.Open();
                    System.Diagnostics.Debug.WriteLine("Connection to DB opened");
                    // Add command parameters
                    command.CommandText = sqlcommand;
                    System.Diagnostics.Debug.WriteLine("ID to delete: " + id);
                    command.Parameters.Add("?id", MySqlDbType.UInt32).Value = id;
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Error message here
                    throw new Exception("MySQL error!", ex);
                }
                finally
                {
                    // Close the connection to DB
                    connection.Close();
                    // Delete image from local folder
                    System.Diagnostics.Debug.WriteLine("ImagePath to delete: " + imagepath);
                    if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(imagepath)))
                    {
                        System.IO.File.Delete(System.Web.HttpContext.Current.Server.MapPath(imagepath));
                        System.Diagnostics.Debug.WriteLine("File deleted: " + imagepath);
                    }
                }
            }

        }



        // Send selected image to email
        public void sendSelectedImageToEmail(string datetime, string imagepath, Boolean movement)
        {
            System.Diagnostics.Debug.WriteLine("Sending email..");
            string movementText = "";
            if (movement)
            {
                movementText = "liikettä havaittu";
            }

            try
            {
                MailMessage mailMessage = new MailMessage();
                // From what account is the email from?
                mailMessage.From = new MailAddress("miika_avela@hotmail.com");
                // To whom is the mail to be sent?
                mailMessage.To.Add("miika_avela@hotmail.com");
                // Email Subject
                mailMessage.Subject = "Lankaranta " + datetime +" "+ movementText;
                // Email body
                mailMessage.Body = "Lankakameran kuva " + datetime;
                // Attach image to email
                mailMessage.Attachments.Add(new Attachment(System.Web.HttpContext.Current.Server.MapPath(imagepath)));
                // Smtp settings
                System.Diagnostics.Debug.WriteLine("Creating SmtpClient..");
                SmtpClient smtpClient = new SmtpClient
                {
                    Host = "smtp.live.com",
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("miika_avela@hotmail.com", "PASSU")

                };
                System.Diagnostics.Debug.WriteLine("SmtpClient ready!");

                // Send the email via SmtpClient
                smtpClient.Send(mailMessage);
                System.Diagnostics.Debug.WriteLine("Email sent!");
            } catch (Exception ex)
            {
                throw new Exception("Email error:", ex);
            }
        }






    }
}