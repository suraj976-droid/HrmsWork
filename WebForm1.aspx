<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="HrmsWork.WebForm1" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Slot Booking</title>
    <style>
        #message {
            color: red;
            font-weight: bold;
            display: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <!-- Select Day Dropdown -->
            <label>Select Day:</label>
            <asp:DropDownList ID="ddlDays" runat="server">
                <asp:ListItem Text="Monday" Value="Monday" />
                <asp:ListItem Text="Tuesday" Value="Tuesday" />
                <asp:ListItem Text="Wednesday" Value="Wednesday" />
                <asp:ListItem Text="Thursday" Value="Thursday" />
                <asp:ListItem Text="Friday" Value="Friday" />
            </asp:DropDownList>
            
            <br /><br />
            
            <!-- Select Time Slot Dropdown -->
            <label>Select Time Slot:</label>
            <asp:DropDownList ID="ddlTimeSlots" runat="server">
                <asp:ListItem Text="9 AM - 10 AM" Value="9-10" />
                <asp:ListItem Text="10 AM - 11 AM" Value="10-11" />
                <asp:ListItem Text="11 AM - 12 PM" Value="11-12" />
                <asp:ListItem Text="1 PM - 2 PM" Value="1-2" />
                <asp:ListItem Text="2 PM - 3 PM" Value="2-3" />
            </asp:DropDownList>

            <br /><br />

            <!-- Email Input -->
            <label for="txtEmail">Email:</label>
            <asp:TextBox ID="txtEmail" runat="server" placeholder="Enter your email" />
            
            <br /><br />
            
            <!-- Submit Button -->
            <asp:Button ID="btnBook" runat="server" Text="Book Slot" OnClick="btnBook_Click" />

            <br /><br />

            <!-- Message label to show success or error -->
            <asp:Label ID="message" runat="server" ForeColor="Red" Visible="false"></asp:Label>
        </div>
    </form>
</body>
</html>


