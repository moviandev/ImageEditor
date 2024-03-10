# Project ImageEditor APIs

This tool has been released to facilitate our user life where they can apply some simple effects on their pictures.

## Features
- Receive a image and the user select which effect they want to apply in it, them in milliseconds they have the result.

## Using the App
- First Signup for using the API you can make a request on POST /api/v1/SignUp
  - Swagger Doc
  ![Swagger Documentation For Signing up /api/v1/SignUp](./DocsImg/Screenshot 2024-03-10 at 4.52.50 PM.png) 
- Copy the Token which the Controller will provide you, click on the lock icon and write Bearer YOUR_TOKEN
  - Response Img:
  ![Controller response](./DocsImg/Screenshot 2024-03-10 at 4.56.44 PM.png)
  - Field to write down the token:
  ![Window to authenticate](./DocsImg/Screenshot 2024-03-10 at 4.58.38 PM.png)
- The POST /api/v1/ApplyEffect endpoint is where you can request to the app apply an effect in your image
  - Which are:
    - Blur - 1;
    - Gray - 2;
    - Sepia - 3;
    - Vignette - 4;
  - I've choose receiving form-data to facilitate usage in front-end so our front can use a simply input of the type file
  ![Controller sample to upload an image and choosing filter](./DocsImg/Screenshot 2024-03-10 at 5.05.14 PM.png)
- After uploading the image and everything did go well the response od this controller will be your id in our Database
  ![Controller /api/v1/ApplyEffect response with success flag set to true and userId](./DocsImg/Screenshot 2024-03-10 at 5.08.43 PM.png)
- To retrieve all the images you uploaded into the app you make a request on GET /api/v1/GetAllImages?userId={GUID}
  ![Endpoint request response sample](./DocsImg/Screenshot 2024-03-10 at 5.25.32 PM.png)
- PS.: I've choose saving all the image original and edited in a S3 Bucket, I gave access to public so everyone can retrieve data from data bucket, also I save data in a Amazon RDS Postgres.
