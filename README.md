# Api boilerplate

This will serve as a general base for creating a backend project in .NET

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

You need Visual Studio 2017 to compile this. ```NuGet``` will take care of the rest.

### Installing

Pull the repo, open it in Visual Studio and enable NuGet package restore. I assume you gave your database the name ```GenericDb```. If this is not the case, please change it accordingly in web.config and also in the ```ContextFactory``` class.

Generate your sql migration script like so:

```
Update-Database -ConfigurationTypeName ContextConfiguration -Script -Verbose -ConnectionString "Data Source=.;Initial Catalog=GenericDb;Integrated Security=True;" -ConnectionProviderName "System.Data.SqlClient" -StartupProjectName App.Api -ProjectName App.Database
```
This will generate a SQL script that you can then run against your database. 
In case you modify the database entities you can always add a migration like so: 

```
Add-Migration -Verbose -ConfigurationTypeName ContextConfiguration -ConnectionString "Data Source=.;Initial Catalog=GenericDb;Integrated Security=True;" -ConnectionProviderName "System.Data.SqlClient" MigrationNameHere -StartupProjectName App.Api -ProjectName App.Database
```

End with an example of getting some data out of the system or using it for a little demo

## Running the tests

Comming up.

## Deployment

More to come. Will contain a build script capable of deploy using [Cake](http://cakebuild.net/).

## Contributing

Please read [CONTRIBUTING.md](https://github.com/CiBuildOrg/WebApi-Boilerplate/blob/master/CONTRIBUTING.md) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/CiBuildOrg/WebApi-Boilerplate/tags). 


## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
