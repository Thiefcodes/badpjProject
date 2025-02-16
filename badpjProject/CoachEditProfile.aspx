<%@ Page Title="Coach Edit Profile" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="CoachEditProfile.aspx.cs" Inherits="badpjProject.CoachEditProfile" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css">
    <style>
        .form-group label {
            font-weight: bold;
        }

        .input-validation-error {
            border: 1.5px solid red !important;
        }

        .form-control {
            border: 1px solid #ced4da;
        }

        .d-none {
            display: none;
        }

        .image-wrapper {
            position: relative;
            display: inline-block;
            width: 200px;
            height: 200px;
        }

            .image-wrapper .overlay {
                position: absolute;
                top: 0;
                left: 0;
                width: 100%;
                height: 100%;
                border-radius: 50%;
                background-color: rgba(0,0,0,0.5);
                opacity: 0;
                transition: opacity 0.3s ease;
                display: flex;
                justify-content: center;
                align-items: center;
                cursor: pointer;
            }

            .image-wrapper:hover .overlay {
                opacity: 1;
            }

        .overlay-text {
            color: #fff;
            font-size: 20px;
            font-weight: 500;
        }
    </style>
    
    <div class="container my-5">
        <h2 class="text-center mb-4">Edit Coach Profile</h2>
        <asp:Panel ID="pnlEditProfile" runat="server">
            <div class="card shadow-lg p-4" style="max-width: 450px; width: 100%; border-radius: 10px; margin: 0 auto;">
                <!-- Profile Picture -->
                <div class="form-group mb-3 text-center">
                    <div class="image-wrapper" onclick="document.getElementById('fu_ProfilePic').click(); return false;" style="position: relative; display: inline-block; width: 200px; height: 200px;">
                        <asp:Image ID="imgProfile" runat="server" ClientIDMode="Static" CssClass="img-fluid rounded-circle profile-click"
                            Style="display: block; width: 200px; height: 200px; object-fit: cover;" />
                        <div class="overlay">
                            <span class="overlay-text">Edit</span>
                        </div>
                    </div>
                    <asp:FileUpload ID="fu_ProfilePic" runat="server" ClientIDMode="Static" CssClass="d-none" accept="image/*" onchange="readURL(this);" />
                </div>
                <!-- Name -->
                <div class="form-group mb-3">
                    <asp:TextBox ID="tb_Name" runat="server" CssClass="form-control" placeholder="Name" Style="width: 100%; max-width: 325px; margin: 0 auto;" />
                    <asp:RequiredFieldValidator ID="rfv_Name" runat="server" ControlToValidate="tb_Name" CssClass="text-danger" ErrorMessage="Name is required." Display="Dynamic" Style="display: block; width: 100%; max-width: 325px; margin: 0 auto;" />
                </div>
                <!-- Email -->
                <div class="form-group mb-3">
                    <asp:TextBox ID="tb_Email" runat="server" CssClass="form-control" TextMode="Email" placeholder="Email" Style="width: 100%; max-width: 325px; margin: 0 auto;" />
                    <asp:RequiredFieldValidator ID="rfv_Email" runat="server" ControlToValidate="tb_Email" CssClass="text-danger" ErrorMessage="Email is required." Display="Dynamic" Style="display: block; width: 100%; max-width: 325px; margin: 0 auto;" />
                    <asp:RegularExpressionValidator ID="rev_Email" runat="server" ControlToValidate="tb_Email" CssClass="text-danger" ErrorMessage="Invalid email format." Display="Dynamic" ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" Style="display: block; width: 100%; max-width: 325px; margin: 0 auto;" />
                </div>
                <!-- Phone -->
                <div class="form-group mb-3">
                    <asp:TextBox ID="tb_Hp" runat="server" CssClass="form-control" placeholder="Phone Number" Style="width: 100%; max-width: 325px; margin: 0 auto;" />
                    <asp:RequiredFieldValidator ID="rfv_Hp" runat="server" ControlToValidate="tb_Hp" CssClass="text-danger" ErrorMessage="Phone number is required." Display="Dynamic" Style="display: block; width: 100%; max-width: 325px; margin: 0 auto;" />
                    <asp:RegularExpressionValidator ID="rev_Hp" runat="server" ControlToValidate="tb_Hp" CssClass="text-danger" ErrorMessage="Invalid phone number format. Only digits are allowed." Display="Dynamic" ValidationExpression="^\d+$" Style="display: block; width: 100%; max-width: 325px; margin: 0 auto;" />
                </div>
                <!-- About You / Description -->
                <div class="form-group mb-3">
                    <asp:TextBox ID="tb_AboutYou" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="About You" Style="width: 100%; max-width: 325px; margin: 0 auto;" />
                    <asp:RequiredFieldValidator ID="rfv_AboutYou" runat="server" ControlToValidate="tb_AboutYou" CssClass="text-danger" ErrorMessage="This field is required." Display="Dynamic" Style="display: block; width: 100%; max-width: 325px; margin: 0 auto;" />
                </div>
                <!-- Qualification (Non-editable) -->
                <div class="form-group mb-3">
                    <asp:DropDownList ID="ddl_Qualification" runat="server" CssClass="form-control" Style="width: 100%; max-width: 325px; margin: 0 auto;" Enabled="false">
                        <asp:ListItem Value="" Text="--Select Qualification--" />
                        <asp:ListItem Value="Certified-PT" Text="Certified Personal Trainer" />
                        <asp:ListItem Value="Placeholder Rank" Text="Placeholder Rank" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfv_Qualification" runat="server" ControlToValidate="ddl_Qualification" CssClass="text-danger" InitialValue="" ErrorMessage="Please select a qualification." Display="Dynamic" Style="display: block; width: 100%; max-width: 325px; margin: 0 auto;" />
                </div>
                <!-- Area of Expertise -->
                <div class="form-group mb-3">
                    <asp:DropDownList ID="ddl_AreaOfExpertise" runat="server" CssClass="form-control" Style="width: 100%; max-width: 325px; margin: 0 auto;">
                        <asp:ListItem Value="" Text="--Select Expertise--" />
                        <asp:ListItem Value="Strength Training" Text="Strength Training" />
                        <asp:ListItem Value="Cardiovascular Training" Text="Cardiovascular Training" />
                        <asp:ListItem Value="HIIT" Text="High-Intensity Interval Training (HIIT)" />
                        <asp:ListItem Value="Bodybuilding" Text="Bodybuilding" />
                        <asp:ListItem Value="Powerlifting" Text="Powerlifting" />
                        <asp:ListItem Value="CrossFit" Text="CrossFit" />
                        <asp:ListItem Value="Functional Training" Text="Functional Training" />
                        <asp:ListItem Value="Endurance Training" Text="Endurance Training" />
                        <asp:ListItem Value="Flexibility & Mobility Training" Text="Flexibility & Mobility Training" />
                        <asp:ListItem Value="Sports Performance Training" Text="Sports Performance Training" />
                        <asp:ListItem Value="Weight Loss & Fat Reduction" Text="Weight Loss & Fat Reduction" />
                        <asp:ListItem Value="Injury Prevention & Rehabilitation" Text="Injury Prevention & Rehabilitation" />
                        <asp:ListItem Value="Core Training" Text="Core Training" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfv_AreaOfExpertise" runat="server" ControlToValidate="ddl_AreaOfExpertise" CssClass="text-danger" InitiMessage="Please select an area of expertise." Display="Dynamic" Style="display: block; width: 100%; max-width: 325px; margin: 0 auto;" />
                </div>
                <!-- Submit and Back Button -->
                <div class="text-center">
                    <asp:Button ID="btnBack" runat="server" Text="Back" CssClass="btn btn-secondary" OnClick="btnBack_Click" />
                    <asp:Button ID="btn_Submit" runat="server" Text="Update Profile" CssClass="btn btn-primary" OnClick="btn_Submit_Click" />
                </div>
            </div>
        </asp:Panel>
        <asp:Label ID="lblStatus" runat="server" CssClass="text-center" ForeColor="red" Visible="false"></asp:Label>
    </div>
    <script type="text/javascript">
        function readURL(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    document.getElementById('<%= imgProfile.ClientID %>').src = e.target.result;
                }
                reader.readAsDataURL(input.files[0]);
            }
        }
    </script>
</asp:Content>