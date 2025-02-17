using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace badpjProject
{
    public partial class Chatbot : System.Web.UI.Page
    {
        //training data
        private static Dictionary<string, string> trainingData = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"hello", "Hi there! How can I assist you today?"},
            {"how do I reset my password?", "To reset your password, go to the login page and click 'Forgot Password'."},
            {"what is machine learning?", "Machine learning is a subset of AI that enables computers to learn from data."},
            {"how can I fix a C# null reference exception?", "A null reference exception occurs when an object is not initialized. Check if it's null before using it."},
            {"tell me a joke", "Why don’t programmers like nature? Because it has too many bugs!"},
            {"what is the capital of france?", "The capital of France is Paris."},
            {"who created c#?", "C# was developed by Microsoft and led by Anders Hejlsberg."}
        };

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected async void btnChatbotSend_Click(object sender, EventArgs e)
        {
            string userMessage = txtChatbotInput.Text.Trim();
            if (string.IsNullOrWhiteSpace(userMessage))
                return;

            string botResponse;

            if (trainingData.ContainsKey(userMessage))
            {
                botResponse = trainingData[userMessage];
            }
            else
            {
                botResponse = await GetGeminiResponse(userMessage);
            }

            litChatbotResponse.Text += $"<p><strong>You:</strong> {userMessage}</p>";
            litChatbotResponse.Text += $"<p><strong>AI:</strong> {botResponse}</p>";

            txtChatbotInput.Text = ""; 
        }

        private async Task<string> GetGeminiResponse(string prompt)
        {
            string apiKey = "AIzaSyB_L06Lzlh_TQ7Of_JFd4_eyasMJgc1deA";
            string apiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={apiKey}";

            using (HttpClient client = new HttpClient())
            {
                var requestBody = new
                {
                    contents = new[]
                    {
                        new
                        {
                            parts = new[]
                            {
                                new { text = prompt }
                            }
                        }
                    }
                };

                string jsonBody = JsonConvert.SerializeObject(requestBody);
                HttpContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                string responseString = await response.Content.ReadAsStringAsync();

                dynamic jsonResponse = JsonConvert.DeserializeObject(responseString);
                return jsonResponse?.candidates?[0]?.content?.parts?[0]?.text ?? "Sorry, I couldn't understand that.";
            }
        }
    }
}