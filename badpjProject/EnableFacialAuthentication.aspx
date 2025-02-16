<%@ Page Title="Enable Facial Authentication" Language="C#" MasterPageFile="~/Site1loggedin.Master" AutoEventWireup="true" CodeBehind="EnableFacialAuthentication.aspx.cs" Inherits="badpjProject.EnableFacialAuthentication" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-5">
        <div class="d-flex justify-content-center align-items-center flex-column">
            <h2>Enable Facial Authentication</h2>
            <!-- Display current username -->
            <asp:Label ID="UserLabel" runat="server" CssClass="h4 mb-3"></asp:Label>
            <!-- Button to start the camera -->
            <input type="button" value="Access Camera" onclick="startCamera();" class="btn btn-primary mb-3" />
            <!-- Video element to display live camera feed -->
            <video id="videoElement" width="640" height="480" autoplay style="border: 1px solid #ccc;"></video>
            <br /><br />
            <!-- Hidden canvas for capturing image frame -->
            <canvas id="canvas" width="640" height="480" style="display: none;"></canvas>
            <!-- Button to capture image and enroll facial authentication -->
            <input type="button" value="Capture & Enroll Facial Data" onclick="captureAndEnroll();" class="btn btn-warning" />
        </div>
    </div>

    <!-- Include jQuery -->
    <script src="https://code.jquery.com/jquery-3.7.1.min.js"></script>


    <!-- Include face-api.js (browser build) -->
    <script src="https://cdn.jsdelivr.net/npm/face-api.js@0.22.2/dist/face-api.min.js"></script>
    <script type="text/javascript">
        $.ajaxSetup({
            xhrFields: {
                withCredentials: true
            }
        });

        // Load face-api.js models from the /Facialmodels folder
        async function loadModels() {
            const modelUrl = '/Facialmodels';
            await faceapi.nets.ssdMobilenetv1.loadFromUri(modelUrl);
            await faceapi.nets.faceLandmark68Net.loadFromUri(modelUrl);
            await faceapi.nets.faceRecognitionNet.loadFromUri(modelUrl);
            console.log("Models loaded");
        }

        // On page load, load the models and set the displayed username from session
        window.addEventListener('load', async () => {
            await loadModels();
            document.getElementById('<%=UserLabel.ClientID%>').innerText = '<%=Session["Username"] ?? "" %>';
});

        // Start camera streaming into the video element
        function startCamera() {
            if (navigator.mediaDevices && navigator.mediaDevices.getUserMedia) {
                navigator.mediaDevices.getUserMedia({ video: true })
                    .then(function (stream) {
                        var video = document.getElementById('videoElement');
                        video.srcObject = stream;
                        video.play();
                    })
                    .catch(function (error) {
                        console.error("Error accessing camera:", error);
                        alert("Unable to access the camera. Please check your device permissions.");
                    });
            } else {
                alert("Your browser does not support camera access.");
            }
        }

        // Helper: Convert DataURL to Blob
        function dataURItoBlob(dataURI) {
            var byteString;
            if (dataURI.split(',')[0].indexOf('base64') >= 0)
                byteString = atob(dataURI.split(',')[1]);
            else
                byteString = decodeURI(dataURI.split(',')[1]);
            var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];
            var ab = new ArrayBuffer(byteString.length);
            var ia = new Uint8Array(ab);
            for (var i = 0; i < byteString.length; i++) {
                ia[i] = byteString.charCodeAt(i);
            }
            return new Blob([ia], { type: mimeString });
        }

        // Capture the image, compute the face descriptor, and enroll in facial authentication
        async function captureAndEnroll() {
            var canvas = document.getElementById('canvas');
            var video = document.getElementById('videoElement');
            var context = canvas.getContext('2d');

            // Capture the current frame onto the canvas
            context.drawImage(video, 0, 0, canvas.width, canvas.height);
            var imageDataUrl = canvas.toDataURL("image/png");

            // Convert the data URL to a Blob and then to an Image for processing
            const blob = dataURItoBlob(imageDataUrl);
            const img = await faceapi.bufferToImage(blob);

            // Detect a face and compute its descriptor
            const detection = await faceapi.detectSingleFace(img).withFaceLandmarks().withFaceDescriptor();
            if (!detection) {
                alert("No face detected. Please try again.");
                return;
            }
            console.log("Face descriptor:", detection.descriptor);
            const descriptorArray = Array.from(detection.descriptor);

            // Send the descriptor to the server to enroll the facial authentication data
            $.ajax({
                type: "POST",
                url: "EnableFacialAuthentication.aspx/EnrollFacialAuth",
                data: JSON.stringify({ descriptor: descriptorArray }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                xhrFields: { withCredentials: true },
                crossDomain: false,  // same domain
                success: function (response) {
                    alert(response.d);
                },
                error: function (err) {
                    console.error(err);
                    alert("Error enrolling facial authentication.");
                }
            });
        }

    </script>
</asp:Content>
