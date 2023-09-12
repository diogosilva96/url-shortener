# url-shortener

## Adding migrations
To add a migration to the `Url.Shortener.Data.Migrator` project do the following:
1. Go to base path (`/`) of the project
2. Run command ` dotnet ef migrations add <migrationName> --startup-project tools\Url.Shortener.Data.Migrator\` - make sure to replace `<migrationName>` with your actual migration name