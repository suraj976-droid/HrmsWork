<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="HrmsWork.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HRMS Form</title>
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
       
            function toggleFields() {
                var selectedValue = $("#<%= DropDownList1.ClientID %>").val();
                if (selectedValue === "Fresher") {
                    $("#ctc, #ectc, #np").hide(); 
                } else {
                    $("#ctc, #ectc, #np, #attachment").show(); 
                }
            }

        
            function handleStreamSelection() {
                var streamValue = $("#<%= DropDownList2.ClientID %>").val();
                if (streamValue === "others") {
                    $("form").trigger("reset"); 
                    $("#eligibilityMessage").show(); 
                    $("#formFields").hide();
                } else {
                    $("#eligibilityMessage").hide();
                    $("#formFields").show(); 
                }
            }

           
            $("#<%= DropDownList1.ClientID %>").change(toggleFields);
            $("#<%= DropDownList2.ClientID %>").change(handleStreamSelection);

           
            toggleFields();
            handleStreamSelection();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div id="formFields">
            <label>Are You</label>
            <asp:DropDownList ID="DropDownList1" runat="server">
                <asp:ListItem>Fresher</asp:ListItem>
                <asp:ListItem>Experienced</asp:ListItem>
            </asp:DropDownList>
            <br /><br />

            <label>Stream</label>
            <asp:DropDownList ID="DropDownList2" runat="server">
                <asp:ListItem>BTech</asp:ListItem>
                <asp:ListItem>Mtech</asp:ListItem>
                <asp:ListItem>Bca</asp:ListItem>
                <asp:ListItem>Mca</asp:ListItem>
                <asp:ListItem>BscIt</asp:ListItem>
                <asp:ListItem>others</asp:ListItem>
            </asp:DropDownList>
            <br /><br />

            <label>Name</label>
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            <br /><br />

            <label>Email</label>
            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
            <br /><br />

            <label>Contact</label>
            <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
            <br /><br />

            <label>Dob</label>
            <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
            <br /><br />

            <div id="ctc">
                <label>CTC</label>
                <asp:TextBox ID="TextBox5" runat="server"></asp:TextBox>
                <br /><br />
            </div>

            <div id="ectc">
                <label>ECTC</label>
                <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                <br /><br />
            </div>

            <div id="np">
                <label>Notice Period</label>
                <asp:TextBox ID="TextBox7" runat="server"></asp:TextBox>
                <br /><br />
            </div>

            <div id="attachment">
                <label>Attachment</label>
                <asp:FileUpload ID="FileUpload1" runat="server" />
                <br /><br />
            </div>

            <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Submit" />
            <br />
        </div>

 
        <div id="eligibilityMessage" style="display:none; color:red; font-weight:bold;">
            You are not eligible
        </div>
    </form>
</body>
</html>
