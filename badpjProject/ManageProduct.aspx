<%@ Page Title="Manage Products" Language="C#" MasterPageFile="~/Site.Master"
    AutoEventWireup="true" CodeBehind="ManageProduct.aspx.cs"
    Inherits="badpjProject.ManageProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style type="text/css">
        body {
            background-color: #fff;
            color: #000;
        }
        .manage-products-container {
            margin-top: 20px;
            margin-bottom: 40px;
        }
        .manage-products-container h2 {
            margin-bottom: 25px;
        }
        .table th {
            background-color: #f8f9fa;
        }
        .table-hover tbody tr:hover {
            background-color: #f2f2f2;
        }

        /* Black button style (like Wishlist) */
        .btn-black {
            background-color: #000;
            border: 1px solid #000;
            color: #fff;
        }
        .btn-black:hover {
            background-color: #333;
            border-color: #333;
        }

        /* Optional narrower discount field */
        .discount-field {
            width: 70px;
            display: inline-block;
            margin-right: 8px;
        }

        /* Image style for the products in the table */
        .manage-product-image {
            width: 80px;     /* or any preferred size */
            height: 80px;    
            object-fit: cover;
            border: 1px solid #ccc;
            border-radius: 5px;
        }
    </style>

    <script type="text/javascript">
        function confirmDelete(uniqueId) {
            event.preventDefault();
            Swal.fire({
                title: "Are you sure?",
                text: "You won't be able to revert this!",
                icon: "warning",
                showCancelButton: true,
                confirmButtonColor: "#3085d6",
                cancelButtonColor: "#d33",
                confirmButtonText: "Yes, delete it!"
            }).then((result) => {
                if (result.isConfirmed) {
                    __doPostBack(uniqueId, '');
                }
            });
            return false;
        }
    </script>

    <div class="container manage-products-container">
        <h2>Manage Products</h2>

        <!-- If you want this button also black -->
        <asp:HyperLink 
            runat="server"
            NavigateUrl="CreateProduct.aspx"
            Text="Create New Product"
            CssClass="btn btn-black mb-3" />

        <asp:GridView
            ID="gvProducts"
            runat="server"
            AutoGenerateColumns="False"
            DataKeyNames="ProductID"
            CssClass="table table-bordered table-hover"
            OnRowEditing="gvProducts_RowEditing"
            OnRowDeleting="gvProducts_RowDeleting"
            OnRowCommand="gvProducts_RowCommand">

            <Columns>
                <asp:TemplateField HeaderText="Image">
                    <ItemTemplate>
                        <img src='<%# ResolveUrl(Eval("ImageUrl").ToString()) %>'
                             alt="Product Image"
                             class="manage-product-image" />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:BoundField DataField="ProductID" HeaderText="ID" ReadOnly="true" />
                <asp:BoundField DataField="ProductName" HeaderText="Name" />
                <asp:BoundField DataField="Price" HeaderText="Price" DataFormatString="{0:C}" />

                <asp:TemplateField HeaderText="Discount (%)">
                    <ItemTemplate>
                        <!-- Wrap the TextBox and Button in a Flex container -->
                        <div class="d-flex align-items-baseline">
                            <asp:TextBox 
                                ID="txtDiscount" 
                                runat="server" 
                                Text='<%# Eval("DiscountPercent") %>' 
                                CssClass="form-control discount-field me-2" 
                                />
                            <asp:LinkButton 
                                ID="btnUpdateDiscount" 
                                runat="server"
                                CommandName="UpdateDiscount" 
                                CommandArgument='<%# Eval("ProductID") %>'
                                Text="Apply"
                                CssClass="btn btn-black btn-sm" />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Actions">
                    <ItemTemplate>
                        <asp:LinkButton
                            ID="btnEdit"
                            runat="server"
                            CommandName="Edit"
                            Text="Edit"
                            CssClass="btn btn-black btn-sm" />
                        &nbsp;
                        <asp:LinkButton
                            ID="btnDelete"
                            runat="server"
                            CommandName="Delete"
                            Text="Delete"
                            CssClass="btn btn-black btn-sm" 
                            OnClientClick='<%# "return confirmDelete(\"" + ((GridViewRow)Container).FindControl("btnDelete").UniqueID + "\");" %>'>
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
