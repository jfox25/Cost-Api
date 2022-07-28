<h1 align="center" style="color:#d2042d">Cost Api</h1>

<p align="center">
    <img style="width: 400px" src="./CostLogo.png">
</p>
<p align="center">By <a href="https://github.com/jfox25">James Fox</a></p>

## About
Cost Api is a .Net 5 Web Api. It provides all necessary data to the <a href="https://github.com/jfox25/Cost-Client">Cost Client</a>. The Api was built using a mySql database, but switched to a postgresql database for production. Cost Api provides authentication functionality to the client using JWT tokens. Background services are implemented to manage users and their expenses. For the best experience, use in conjunction with the Cost Client.


## Technologies Used

- .Net 5 Web Api
- Microsoft EntityFramework Core
- Microsoft Identity
- AutoMapper
- Postgressql

## Project Requirements

1. .Net 5 SDK

## Project Setup

1. Clone this repository to your desktop using git clone

```
git clone https://github.com/jfox25/Cost-Api
```
2. Cd into the Api folder of the project
```
cd api
```

3. Install all necessary dependencies 
```
dotnet install
```
4. Create an appsettings.json file in the root of the api folder and add the following code, replace with your Postgres database information. 
```
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost; Port={PORTFORSERVER}; User Id={USERIDFORSERVER}; Password={YOURSERVERPASSWORD}; Database={DATBASENAME};sslmode=Prefer;Trust Server Certificate=true"
  }
```

5. Run Migrations to update your database
```
dotnet ef database update
```

6. Your app is now ready for use! Use with the [Cost-Client](https://github.com/jfox25/Cost-Client) for best the experience . 

## GitHub Link

[Link to Code on GitHub](https://github.com/jfox25/Cost-Api)

## Creator's Linkedin 

[Linkedin](https://www.linkedin.com/in/jamesconnorfox/)

## Bugs

- No know bugs at this time. 

## Future Improvements

- Internal changes to clean up the api and get better performance. 

## License

MIT
Copyright (c) 2022 James Fox
