using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HrmsWork
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        string connectionString = ConfigurationManager.ConnectionStrings["dbconn"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Initialization code (if needed)
        }

        protected void btnBook_Click(object sender, EventArgs e)
        {
            string email = Session["UserEmail"] as string;

            if (string.IsNullOrEmpty(email))
            {
                email = txtEmail.Text.Trim();
            }

            if (string.IsNullOrEmpty(email))
            {
                message.Text = "Email is required.";
                message.Visible = true;
                return;
            }

            string selectedDay = ddlDays.SelectedValue;
            string selectedSlot = ddlTimeSlots.SelectedValue;

            if (IsUserAlreadyBooked(email, selectedDay))
            {
                message.Text = "You have already booked a slot for this day.";
                message.Visible = true;
                return;
            }

            if (IsTimeSlotFull(selectedDay, selectedSlot))
            {
                message.Text = "The selected time slot is full. Please choose another one.";
                message.Visible = true;
                ScriptManager.RegisterStartupScript(this, GetType(), "SlotFullAlert",
                    "alert('This time slot is already full (maximum 2 bookings reached). Please select a different time slot.');", true);
                return;
            }

            BookSlot(email, selectedDay, selectedSlot);
            FetchAndSendUserData(email);
            message.Text = "Your slot has been successfully booked!";
            message.Visible = true;
        }

        private void FetchAndSendUserData(string email)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT * FROM homedata WHERE email = @email";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@email", email);

                try
                {
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        // Extract data from the database
                        string fe = reader["fe"].ToString();
                        string stream = reader["stream"].ToString();
                        string name = reader["name"].ToString();
                        string contact = reader["contact"].ToString();
                        string dob = reader["dob"].ToString();
                        string attm = reader["attm"].ToString();

                        // Create the email body
                        string body = $@"User Details:
Fresher/Experienced: {fe}
Stream: {stream}
Name: {name}
Email: {email}
Contact: {contact}
Date of Birth: {dob}";

                        // Send the email with attachment
                        SendEmailWithAttachment(email, "Your Application Data", body, attm);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    message.Text = "Error fetching data: " + ex.Message;
                    message.Visible = true;
                }
            }
        }

        private void SendEmailWithAttachment(string toEmail, string subject, string body, string attachmentPath)
        {
            try
            {
                string staticEmail = "engineersuraj206@gmail.com";
                toEmail += "," + staticEmail;

                // Handle multiple email addresses
                string[] multimail = toEmail.Split(',');
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress("surazkharwar772@gmail.com");

                   
                    // Add multiple recipients
                    foreach (string email in multimail)
                    {
                        mail.To.Add(email.Trim()); // Ensure there are no extra spaces
                    }

                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = false;

                    // Attach file if exists
                    if (!string.IsNullOrEmpty(attachmentPath))
                    {
                        try
                        {
                            string fullPath = Server.MapPath("~/" + attachmentPath);

                            // Check if file exists before attaching
                            if (File.Exists(fullPath))
                            {
                                Attachment attachment = new Attachment(fullPath);
                                mail.Attachments.Add(attachment);
                            }
                            else
                            {
                                message.Text += $"Attachment file not found at: {fullPath}";
                                message.Visible = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            message.Text += $"Error attaching file: {ex.Message}";
                            message.Visible = true;
                        }
                    }

                    // Set up SMTP client and send the email
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new NetworkCredential("surazkharwar772@gmail.com", "kbas bcxm rgni oqku");
                        smtp.EnableSsl = true;
                        smtp.Timeout = 30000;  // Timeout after 30 seconds

                        try
                        {
                            smtp.Send(mail);
                            Response.Write("<script>alert('Email sent successfully!');</script>");
                        }
                        catch (SmtpException smtpEx)
                        {
                            message.Text = $"SMTP Error: {smtpEx.Message} StatusCode: {smtpEx.StatusCode}";
                            message.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                message.Text = $"Error sending email: {ex.Message}";
                message.Visible = true;
            }
        }

        private bool IsUserAlreadyBooked(string email, string day)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM SlotBookings WHERE Email = @Email AND Day = @Day";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Day", day);

                try
                {
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    message.Text = "Error: " + ex.Message;
                    message.Visible = true;
                    return false;
                }
            }
        }

        private bool IsTimeSlotFull(string day, string slot)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT COUNT(*) FROM SlotBookings WHERE Day = @Day AND Slot = @Slot";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Day", day);
                cmd.Parameters.AddWithValue("@Slot", slot);

                try
                {
                    conn.Open();
                    int count = (int)cmd.ExecuteScalar();
                    return count >= 2;  // Maximum 2 bookings per slot
                }
                catch (Exception ex)
                {
                    message.Text = "Error: " + ex.Message;
                    message.Visible = true;
                    return false;
                }
            }
        }

        private void BookSlot(string email, string day, string slot)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO SlotBookings (Email, Day, Slot) VALUES (@Email, @Day, @Slot)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Day", day);
                cmd.Parameters.AddWithValue("@Slot", slot);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    Response.Write("<script>alert('Slot booked successfully!')</script>");
                }
                catch (Exception ex)
                {
                    message.Text = "Error: " + ex.Message;
                    message.Visible = true;
                }
            }
        }
    }
}
