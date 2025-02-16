using System;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;

namespace badpjProject
{
    public partial class TestPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public static string TestMethod()
        {
            // No session check, no DB, just return a string
            return "Test success!";
        }
    }
}
