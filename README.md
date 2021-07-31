# Running PropertyDemo

## Docker

PropertyDemo has been configured to support running in Docker

1. Build the image via `docker build -t property-demo-app:latest .`
2. Execute the image via `docker-compose up -d`. It will spin up Microsoft SQL Server (Developer edition) before running PropertyDemo
3. Optional - if you have Microsoft SQL Server installed locally then comment out the `ms-sql` service in the docker-compose.yml file

## Local

Make sure MS SQL Server is install. Then open the project with visual studio and run it.

There is no need fro manual database/tables creation in prior to running the app.

The project has been configured to use code-first approach. Read the **Database** section below for more details.

## Database

The project has been configure to use code-first approach for database management.

All changes are reflected on the database through migrations.

A new migration must be created when:

- Database design is changed (relationships, tables etc)

### Prerequisite

Make sure `dotnet tool` is installed. If not use this command in command prompt to install `dotnet tool install --global dotnet-ef`

### Creating new migration

The best pratice is to group all related database changes within one
migration then name your migration accordingly to reflect the changes.

1. Navigate to the same folder as the solution e.g. `C:\git\PropertyDemo`
2. Invoke `dotnet ef migrations add <migrationName> -s PropertyDemo -p PropertyDemo.Migrations`
1. The generated migration code will appear in
`PropertyDemo.Migrations\Migrations\`
4. Optionally invoke `dotnet ef database update -s PropertyDemo -p PropertyDemo.Migrations` to test the created migration.