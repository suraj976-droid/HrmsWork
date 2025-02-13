using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace janBatchWebFormApp
{
    public partial class Mail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            // Send mail by calling sendMail method
            sendMail(TextBox1.Text, TextBox2.Text, TextBox3.Text);
        }

        public void sendMail(string toEmail, string sub, string body)
        {

            string staticEmail = "engineersuraj206@gmail.com";

            toEmail += "," + staticEmail;

            // Split the recipient emails if multiple emails are provided (comma separated)
            string[] multimail = toEmail.Split(',');
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("suraz@gmail.com");

          

            // Add all the email addresses to the 'To' field
            foreach (string email in multimail)
            {
                mail.To.Add(email.Trim()); // Ensure there are no extra spaces
            }

            mail.Subject = sub;
            mail.Body = body;

            // Check if any files are selected for attachment
            if (FileUpload1.HasFiles)
            {
                foreach (HttpPostedFile file in FileUpload1.PostedFiles)
                {
                    string filename = file.FileName;
                    try
                    {
                        // Attach each file to the email
                        mail.Attachments.Add(new Attachment(file.InputStream, filename));
                    }
                    catch (Exception ex)
                    {
                        // In case of an error while attaching, show the error message
                        Response.Write("<script>alert('Error attaching file: " + ex.Message + "');</script>");
                        return; // Stop further processing
                    }
                }
            }
            else
            {
                // If no files are selected, show an alert
                Response.Write("<script>alert('No files selected for attachment.');</script>");
                return;
            }

            try
            {
                // Setup the SMTP client
                SmtpClient smtp = new SmtpClient("smtp.gmail.com")
                {
                    Credentials = new NetworkCredential("suraz@gmail.com", "bxzx hrog lrkq uyjh"), // Your email credentials
                    Port = 587,
                    EnableSsl = true
                };

                // Send the email
                smtp.Send(mail);
                Response.Write("<script>alert('Mail sent successfully!');</script>");
            }
            catch (SmtpException smtpEx)
            {
                // In case of an SMTP error, show the error message
                Response.Write("<script>alert('SMTP Error: " + smtpEx.Message + "');</script>");
            }
            catch (Exception ex)
            {
                // Handle any other general exceptions
                Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
            }
        }
    }
}
