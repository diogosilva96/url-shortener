# url-shortener
Small url shortener application using Minimal APIs. It makes use of the following packages:
- Entity Framework Core
- MediatR
- FluentValidation
- Carter

## TODO
Here is the list of things to do:
- Add unit tests
- Add integration tests
- Add CI pipeline for the application
- Add dev container
- Improve the algorithm of the `UrlShortener`
- Use `Result<T1,T2...>` for the api responses (instead of exceptions) to reduce some of the magic?

## Adding migrations
To add a migration to the `Url.Shortener.Data.Migrator` project do the following:
1. Go to base path (`/`) of the project
2. Run command ` dotnet ef migrations add <migrationName> --startup-project tools\Url.Shortener.Data.Migrator\` - make sure to replace `<migrationName>` with your actual migration name