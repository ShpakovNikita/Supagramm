# Supagramm. #
##### Instagram-like photo hosting resource #####
This is very easy to use and little photo hosting resource, where you may share your photos with the friends! You may post, like and comment photos, search other users and find new friends. Also, for moderation, here we have a special admin panel, that may be accessed only if you have a proper status. 
## Architecture ##
This application was devided by two levels - App managers and MVC application.
### App managers ###
According to onion architecture principles, we may add additional storage providers layer, but for this application it will be not necessarily. Using managers, we are getting our data through defined method directly from database (sqlite database that was created by the code first principle, using Entity Framework Core, passing database context to each manager through dependency injection). Also here we have some logical firtering and convertations from the database models to main EntityCore models.
### MVC Application ###
This is the main MVC application, that have controllers, models (View Models, because we have our ones in EntityCore module) and view. Also here we have some front end logic and other components, like TagHelpers, SignalR hubs and view components. 

## How to run? ##

### [Make sure you have installed the latest .NET Core 2.1 SDK](https://dotnet.microsoft.com/download/dotnet-core/2.1) ###
And now you have to build solution throught Visual studio.

## Admin panel ##
To become an admin and gain access to the admin panel, you need to be approved by other administration team members. But for the case, when we don't have any admins at all, you need to change EntityCore.Constants.Roles.DEFAULT_ROLE getter to Admin role and create a new user. It is better to have a separate command line tools application for creating admins in this cases, but we don't have it yet.
