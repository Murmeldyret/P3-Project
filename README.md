# P3-Project

## Requirements

Dotnet ef needs to be installed as a global or local tool in order to create migrations and other stuff.
```
dotnet tool install --global dotnet-ef
```

## Database

Delete the database
```
dotnet ef database drop
```

Create an initial database
```
dotnet ef migrations add InitialCreate
```

Apply an update to the database
```
dotnet ef database update
```

Add new column to the database
```
dotnet ef migrations add AddBlogCreatedTimestamp
```

In the command above, "Blog" is the model, "CreatedTimestamp" is the newly added property. 

### More info
https://learn.microsoft.com/da-dk/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli <br>
https://learn.microsoft.com/en-gb/aspnet/core/data/ef-mvc/migrations?view=aspnetcore-2.0#introduction-to-migrations
