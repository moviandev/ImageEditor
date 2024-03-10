# Project ImageEditor APIs

This tool has been released to facilitate our user life where they can apply some simple effects on their pictures.

## Features
- Receive a image and the user select which effect they want to apply in it, them in milliseconds they have the result.

## Using the App
- First Signup for using the API you can make a request on POST /api/v1/SignUp
  - Swagger Doc
  <img src="./DocsImg/Screenshot 2024-03-10 at 4.52.50 PM.png" alt="Swagger Documentation For Signing up /api/v1/SignUp">
- Copy the Token which the Controller will provide you, click on the lock icon and write Bearer YOUR_TOKEN
  - Response Img:
  <img src="./DocsImg/Screenshot 2024-03-10 at 4.56.44 PM.png" alt="Controller response">
  - Field to write down the token:
  <img src="./DocsImg/Screenshot 2024-03-10 at 4.58.38 PM.png" alt="Window to authenticate">
- The POST /api/v1/ApplyEffect endpoint is where you can request to the app apply an effect in your image
  - Which are:
    - Blur - 1;
    - Gray - 2;
    - Sepia - 3;
    - Vignette - 4;
  - I've choose receiving form-data to facilitate usage in front-end so our front can use a simply input of the type file
  <img src="./DocsImg/Screenshot 2024-03-10 at 5.05.14 PM.png" alt="Controller sample to upload an image and choosing filter">
- After uploading the image and everything did go well the response od this controller will be your id in our Database
  <img src="./DocsImg/Screenshot 2024-03-10 at 5.08.43 PM.png" alt="Controller /api/v1/ApplyEffect response with success flag set to true and userId">
- To retrieve all the images you uploaded into the app you make a request on GET /api/v1/GetAllImages?userId={GUID}
  <img src="./DocsImg/Screenshot 2024-03-10 at 5.41.46 PM.png" alt="Endpoint request response sample">
- PS.: I've choose saving all the image original and edited in a S3 Bucket, I gave access to public so everyone can retrieve data from data bucket, also I save data in a Amazon RDS Postgres.
