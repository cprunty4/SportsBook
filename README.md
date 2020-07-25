# mission
Currently 6 states have legalized sports betting (New Jersey, Pennsylvania, Indiana etc)  I want to build an app that has the capability to enter the online sports gambling space.   

My plan is release a NFL betting site before the start of the 2020 Season on September 13th.  My goal is start specific to NFL football but build a system that scalable to the other professional sports.


# background
This repo contains the front end code.  I started this code in 2020. It runs on the Azure cloud <a href="https://cullens-sportsbook.azurewebsites.net/">cullens-sportsbook.azurewebsites.net</a>

The back end api code is in a different github repo that I started in 2017. The code has not been made public yet.  The tool connects to a Azure SQL database.  <a href="https://entity-types-backend-admin.azurewebsites.net/"> entity-types-backend-admin.azurewebsites.net</a>

I'm using Visual Studio Code. 

Here's some commands I use for debugging:

# build command
dotnet build .\\SportsBook.sln /p:Configuration=Debug /p:Platform="Any CPU"

# run command
dotnet run .\\SportsBook.sln /p:Configuration=Debug /p:Platform="Any CPU"
