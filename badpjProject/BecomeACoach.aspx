<%@ Page Title="Become a Coach" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="BecomeACoach.aspx.cs" Inherits="badpjProject.BecomeACoach" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .input-validation-error {
            border: 1.5px solid red !important;
        }

        .form-control {
            border: 1px solid #ced4da;
        }
    </style>
    <div class="d-flex justify-content-center align-items-center" style="padding: 50px 0;">
        <!-- Panel wrapping the existing form without changing any existing IDs -->
        <asp:Panel ID="formDiv" runat="server">
            <div class="card shadow-lg p-4" style="max-width: 450px; width: 100%; border-radius: 10px;">
                <h2 class="text-center mb-4">Apply to Become a Coach</h2>

                <!-- Form Fields -->
                <div class="form-group mb-3">
                    <asp:TextBox ID="tb_Name" runat="server" CssClass="form-control" placeholder="Name" Style="width: 100%; max-width: 325px; margin-left: auto; margin-right: auto;" OnValidator="ApplyRedBorder" />
                    <asp:RequiredFieldValidator ID="rfv_Name" runat="server" ControlToValidate="tb_Name" CssClass="text-danger" ErrorMessage="Name is required." Display="Dynamic" Style="display: block; width: 100%; max-width: 325px; margin-left: auto; margin-right: auto;"></asp:RequiredFieldValidator>
                </div>

                <div class="form-group mb-3">
                    <asp:TextBox ID="tb_Email" runat="server" CssClass="form-control" TextMode="Email" placeholder="Email" Style="width: 100%; max-width: 325px; margin-left: auto; margin-right: auto;" OnValidator="ApplyRedBorder" />
                    <asp:RequiredFieldValidator ID="rfv_Email" runat="server" ControlToValidate="tb_Email" CssClass="text-danger" ErrorMessage="Email is required." Display="Dynamic" Style="display: block; width: 100%; max-width: 325px; margin-left: auto; margin-right: auto;"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="rev_Email" runat="server" ControlToValidate="tb_Email" CssClass="text-danger" ErrorMessage="Invalid email format." Display="Dynamic" ValidationExpression="^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" Style="display: block; width: 100%; max-width: 325px; margin-left: auto; margin-right: auto;"></asp:RegularExpressionValidator>
                </div>

                <div class="form-group mb-3">
                    <asp:TextBox ID="tb_Hp" runat="server" CssClass="form-control" placeholder="Phone Number" Style="width: 100%; max-width: 325px; margin-left: auto; margin-right: auto;" OnValidator="ApplyRedBorder" />
                    <asp:RequiredFieldValidator ID="rfv_Hp" runat="server" ControlToValidate="tb_Hp" CssClass="text-danger" ErrorMessage="Phone number is required." Display="Dynamic" Style="display: block; width: 100%; max-width: 325px; margin-left: auto; margin-right: auto;"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="rev_Hp" runat="server" ControlToValidate="tb_Hp" CssClass="text-danger" ErrorMessage="Invalid phone number format. Only digits are allowed." Display="Dynamic" ValidationExpression="^\d+$" Style="display: block; width: 100%; max-width: 325px; margin-left: auto; margin-right: auto;"></asp:RegularExpressionValidator>
                </div>

                <div class="form-group mb-3">
                    <asp:TextBox ID="tb_AboutYou" runat="server" CssClass="form-control" TextMode="MultiLine" placeholder="About You" Style="width: 100%; max-width: 325px; margin-left: auto; margin-right: auto;" OnValidator="ApplyRedBorder" />
                    <asp:RequiredFieldValidator ID="rfv_AboutYou" runat="server" ControlToValidate="tb_AboutYou" CssClass="text-danger" ErrorMessage="This field is required." Display="Dynamic" Style="display: block; width: 100%; max-width: 325px; margin-left: auto; margin-right: auto;"></asp:RequiredFieldValidator>
                </div>

                <div class="form-group mb-3">
                    <asp:DropDownList ID="ddl_Qualification" runat="server" onchange="toggleCertUpload();" CssClass="form-control" Style="width: 100%; max-width: 325px; margin-left: auto; margin-right: auto;">
                        <asp:ListItem Value="" Text="--Select Qualification--" />
                        <asp:ListItem Value="Certified-PT" Text="Certified Personal Trainer" />
                        <asp:ListItem Value="Rank" Text="Rank" />
                    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="rfv_Qualification" runat="server" ControlToValidate="ddl_Qualification" CssClass="text-danger" InitialValue="" ErrorMessage="Please select a qualification." Display="Dynamic" Style="display: block; width: 100%; max-width: 325px; margin: auto;"></asp:RequiredFieldValidator>

                    <asp:CustomValidator ID="cvRank" runat="server" ControlToValidate="ddl_Qualification" ErrorMessage="You must be Emerald Exerciser or higher (≥ 3000 points) to select this rank." OnServerValidate="cvRank_ServerValidate" Display="Dynamic" CssClass="text-danger" Style="display: block; width: 100%; max-width: 325px; margin: auto;">
                    </asp:CustomValidator>
                </div>

                <!-- Certification Document File Upload (hidden by default) -->
                <div class="form-group mb-3" id="certDocSection" style="display: none;">
                    <h5 class="text-center">Upload Your Certification Document</h5>
                    <asp:FileUpload ID="fu_CertDoc" runat="server" CssClass="form-control"
                        Style="width: 100%; max-width: 325px; margin: auto;" />
                    <asp:CustomValidator ID="cvCertDoc" runat="server"
                        ControlToValidate="ddl_Qualification"
                        OnServerValidate="cvCertDoc_ServerValidate"
                        Display="Dynamic" CssClass="text-danger"
                        ErrorMessage="Please upload a valid certification document for Certified PT."
                        Style="display: block; width: 100%; max-width: 325px; margin: auto;" >
                    </asp:CustomValidator>

                </div>

                <div class="form-group mb-3">
                    <asp:DropDownList ID="ddl_AreaOfExpertise" runat="server" CssClass="form-control" Style="width: 100%; max-width: 325px; margin-left: auto; margin-right: auto;">
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
                    <asp:RequiredFieldValidator ID="rfv_AreaOfExpertise" runat="server" ControlToValidate="ddl_AreaOfExpertise" CssClass="text-danger" InitialValue="" ErrorMessage="Please select an area of expertise." Display="Dynamic" Style="display: block; width: 100%; max-width: 325px; margin-left: auto; margin-right: auto;"></asp:RequiredFieldValidator>
                </div>

                <div class="form-group mb-3">
                    <h5 class="text-center">Video showcasing your expertise</h5>
                    <asp:FileUpload ID="fu_Coach" runat="server" CssClass="form-control" accept="video/*" Style="width: 100%; max-width: 325px; margin-left: auto; margin-right: auto;" />
                    <asp:RequiredFieldValidator ID="rfv_Coach" runat="server" ControlToValidate="fu_Coach" CssClass="text-danger" ErrorMessage="Please upload a video file." Display="Dynamic" Style="display: block; width: 100%; max-width: 325px; margin-left: auto; margin-right: auto;"></asp:RequiredFieldValidator>
                </div>

                <div class="text-center">
                    <asp:Button ID="btn_Submit" runat="server" Text="Submit" CssClass="btn btn-primary w-" OnClick="btn_Submit_Click" />
                </div>
            </div>
        </asp:Panel>

        <!-- Modern Pending Status Alert Panel -->
        <asp:Panel ID="divPendingStatus" runat="server" CssClass="alert alert-warning text-center" Style="display: none;">
            <asp:Literal ID="litPendingStatus" runat="server" />
        </asp:Panel>
    </div>

    <script>
        function toggleCertUpload() {
            var ddlQualification = document.getElementById("<%= ddl_Qualification.ClientID %>");
            var certDocSection = document.getElementById("certDocSection");

            if (ddlQualification.value === "Certified-PT") {
                certDocSection.style.display = "block";
            } else {
                certDocSection.style.display = "none";
            }
        }

        // This runs on page load to ensure the correct state after postback
        window.onload = function () {
            toggleCertUpload();
        };
    </script>
</asp:Content>
