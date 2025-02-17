<%@ Page Title="Wishlist" Language="C#" MasterPageFile="~/Site.Master" 
    AutoEventWireup="true" CodeBehind="Wishlist.aspx.cs" Inherits="badpjProject.Wishlist" %>


<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<style type="text/css">
  body { background-color: #fff; color: #000; }
  .wishlist-item {
    padding: 15px;
    border: 1px solid #ccc;
    border-radius: 5px;
    margin-bottom: 15px;
  }
  .wishlist-image { width: 150px; height: 150px; object-fit: cover; }
  .product-details { padding-left: 15px; }
  .discount-indicator {
    font-weight: bold;
    color: red;
    background: transparent;
    padding: 1px 4px;
    border-radius: 3px;
    font-size: 0.7rem;
  }
  .btn-black {
    background-color: #000;
    border: 1px solid #000;
    color: #fff;
  }
  .btn-black:hover {
    background-color: #333;
    border-color: #333;
  }
  .wishlist-note { font-style: italic; color: #555; }
  .star-rating { font-size: 0.9rem; color: #ff9900; }
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


<div class="container">
  <h2 class="my-4">Your Wishlist</h2>
    
  <!-- Repeater with OnItemCommand & OnItemDataBound -->
  <asp:Repeater 
    ID="rptWishlist" 
    runat="server" 
    OnItemCommand="rptWishlist_ItemCommand"
    OnItemDataBound="rptWishlist_ItemDataBound">
    <ItemTemplate>
      <div class="wishlist-item row align-items-center">
        <!-- Product Image -->
        <div class="col-md-2">
          <a href='<%# "ProductDetails.aspx?productID=" + Eval("ProductID") %>'>
            <img src='<%# ResolveUrl(Eval("ImageUrl").ToString()) %>' alt='<%# Eval("ProductName") %>' class="wishlist-image img-fluid" />
          </a>
        </div>
        <!-- Product Details -->
        <div class="col-md-5 product-details">
          <h4>
            <a href='<%# "ProductDetails.aspx?productID=" + Eval("ProductID") %>' class="text-dark">
              <%# Eval("ProductName")%>
            </a>
          </h4>
          <div class="star-rating mb-1">
            <%# Eval("AverageRating") != DBNull.Value 
                  ? String.Format("{0:N1} / 5", Eval("AverageRating")) 
                  : "No ratings" %>
          </div>
          <p>
            <%# Convert.ToInt32(Eval("DiscountPercent")) > 0 
                  ? String.Format("<small><del>{0:C}</del></small> {1:C} <span class='discount-indicator'>{2}% OFF</span>", 
                                  Eval("Price"), 
                                  Convert.ToDecimal(Eval("Price")) * (1 - Convert.ToInt32(Eval("DiscountPercent")) / 100m), 
                                  Eval("DiscountPercent"))
                  : String.Format("{0:C}", Eval("Price")) %>
          </p>
          <p><%# Eval("Description") %></p>
        </div>

        <!-- Note Section -->
        <div class="col-md-3">
          <h5>Note:</h5>
          <!-- View mode placeholder -->
          <asp:PlaceHolder ID="phView" runat="server">
            <p class="wishlist-note"><%# Eval("Notes") %></p>
            <!-- Single Edit Note button -->
            <asp:LinkButton 
              ID="LinkButton1" 
              runat="server" 
              CommandName="Edit" 
              CommandArgument='<%# Eval("ProductID") %>' 
              CssClass="btn btn-sm btn-black">
              Edit Note
            </asp:LinkButton>
          </asp:PlaceHolder>

          <!-- Edit mode placeholder -->
          <asp:PlaceHolder ID="phEdit" runat="server" Visible="false">
            <asp:TextBox 
              ID="txtNotes" 
              runat="server" 
              Text='<%# Eval("Notes") %>' 
              CssClass="form-control" 
              TextMode="MultiLine" 
              Rows="3">
            </asp:TextBox>
            <asp:LinkButton 
              ID="btnUpdate" 
              runat="server" 
              CommandName="Update" 
              CommandArgument='<%# Eval("ProductID") %>' 
              CssClass="btn btn-sm btn-black mt-2">
              Update
            </asp:LinkButton>
            <asp:LinkButton 
              ID="btnCancel" 
              runat="server" 
              CommandName="Cancel" 
              CommandArgument='<%# Eval("ProductID") %>' 
              CssClass="btn btn-sm btn-black mt-2">
              Cancel
            </asp:LinkButton>
          </asp:PlaceHolder>
        </div>

        <div class="col-md-2 text-end">
            <asp:LinkButton 
                ID="btnRemove" 
                runat="server" 
                CommandName="Remove" 
                CommandArgument='<%# Eval("ProductID") %>' 
                CssClass="btn btn-sm btn-black mb-2"
                OnClientClick='<%# "return confirmDelete(\"" + ((LinkButton)Container.FindControl("btnRemove")).UniqueID + "\");" %>'>
                Remove
            </asp:LinkButton>
                
            <asp:LinkButton 
                ID="btnAddToCart" 
                runat="server" 
                CommandName="AddToCart" 
                CommandArgument='<%# Eval("ProductID") %>' 
                CssClass="btn btn-sm btn-black mb-2">
                Add to Cart
            </asp:LinkButton>
        </div>
      </div>
    </ItemTemplate>
  </asp:Repeater>
</div>
</asp:Content>
