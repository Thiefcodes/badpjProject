<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CameraAccess.aspx.cs" Inherits="YourNamespace.CameraAccess" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title>Register Account with Facial Data</title>
  <!-- Load jQuery -->
  <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
  <!-- Load face-api.js (browser build) -->
  <script src="https://cdn.jsdelivr.net/npm/face-api.js@0.22.2/dist/face-api.min.js"></script>
  <script type="text/javascript">
      // Load face-api.js models from the /models folder
      async function loadModels() {
          const modelUrl = '/Facialmodels';
          await faceapi.nets.ssdMobilenetv1.loadFromUri(modelUrl);
          await faceapi.nets.faceLandmark68Net.loadFromUri(modelUrl);
          await faceapi.nets.faceRecognitionNet.loadFromUri(modelUrl);
          console.log("Models loaded");
      }

      // On page load, load the models
      window.addEventListener('load', async () => {
          await loadModels();
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

      // Capture image, compute face descriptor, and enroll (store) the facial data for the account
      async function captureAndEnroll() {
          var canvas = document.getElementById('canvas');
          var video = document.getElementById('videoElement');
          var context = canvas.getContext('2d');

          // Capture the current frame onto the canvas
          context.drawImage(video, 0, 0, canvas.width, canvas.height);
          var imageDataUrl = canvas.toDataURL("image/png");

          // Convert to Blob and then an Image for processing by face-api.js
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

          var username = $("#username").val();
          if (!username) {
              alert("Please enter a username.");
              return;
          }

          // Send the username and descriptor to the server to enroll the face data
          $.ajax({
              type: "POST",
              url: "CameraAccess.aspx/EnrollFaceDescriptor",
              data: JSON.stringify({ username: username, descriptor: descriptorArray }),
              contentType: "application/json; charset=utf-8",
              dataType: "json",
              success: function (response) {
                  alert(response.d);
              },
              error: function (err) {
                  console.error(err);
                  alert("Error enrolling face data.");
              }
          });
      }
  </script>
  <style>
      /* Hide the canvas (used only for capturing image) */
      #canvas { display: none; }
  </style>
</head>
<body>
  <form id="form1" runat="server">
    <div style="text-align: center;">
      <h2>Register Account with Facial Data</h2>
      <!-- Input field for username -->
      <input type="text" id="username" placeholder="Enter Username" style="padding:8px; font-size:16px;" />
      <br /><br />
      <!-- Button to start camera -->
      <input type="button" value="Access Camera" onclick="startCamera();" style="padding:10px 20px; font-size:16px;" />
      <br /><br />
      <!-- Video element for live camera feed -->
      <video id="videoElement" width="640" height="480" autoplay style="border: 1px solid #ccc;"></video>
      <br /><br />
      <!-- Hidden canvas for capturing a frame -->
      <canvas id="canvas" width="640" height="480"></canvas>
      <br />
      <!-- Button to capture and enroll facial data -->
      <input type="button" value="Capture & Enroll Face" onclick="captureAndEnroll();" style="padding:10px 20px; font-size:16px;" />
      <br /><br />
      <!-- Link to the authentication page (to be created separately) -->
      <a href="Authentication.aspx" style="font-size:16px;">Go to Authentication Page</a>
    </div>
  </form>
</body>
</html>
