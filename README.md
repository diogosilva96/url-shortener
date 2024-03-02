# url-shortener
Small url shortener application using Minimal APIs & following vertical slice architecture. 
The application makes use of the following packages:
- `Entity Framework Core` - for data access
- `MediatR` - for request handling & request middleware
- `FluentValidation` - for validation
- `XUnit` - for unit & integration tests
- `NSubstitute` - for mocking
- `AutoFixture` - for creating test data

The application uses exception middleware & RequestPreProcessors (from MediatR) in order to simplify & minimize code duplication in the control flow of the api responses. However, the result pattern could have also been used.

## TODO
Here is the list of things to do:
- Add more url management endpoints such as:
  - Get UrlMetadata (no redirect)
  - List UrlMetadata
  - Expire url?
- Add output caching for read urls?
- Add auth for UrlMetadata management endpoints (either via azure active directory or Entity framework + identity)
- Add open telemetry
- Add automatic expiration of urls (e.g., after 6 months)

## Adding migrations
To add a migration to the `Url.Shortener.Data.Migrator` project do the following:
1. Go to base path (`/`) of the project
2. Run command `dotnet ef migrations add <migrationName> --startup-project tools\Url.Shortener.Data.Migrator\` - make sure to replace `<migrationName>` with your actual migration name

## Running the application (locally)
1. Run `docker-compose up -d` in the base folder of the project
2. Make sure to run the `Url.Shortener.Data.Migrator` located in the `tools` folder pointing to the docker db (update appsettings for this or user secrets), so that the db gets initialized.
3. Open `localhost:30001/swagger` to view the swagger ui
4. Open `localhost:30004` to view the Seq ui

Note: docker-compose is meant to be used to only provide the required infrastructure locally.