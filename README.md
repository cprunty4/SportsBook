# SportsBook
A new SportsBook app started in 2020 that runs on the Azure cloud <a href="https://cullens-sportsbook.azurewebsites.net/">click here</a>

This repo contains the front end code.  The back end api code is in a different github repo that I started in 2017.   The back end has a admin tool that connects to a Azure SQL database.

I'm using Visual Studio Code.  here's some commands I use:

# build command
dotnet build .\\SportsBook.sln /p:Configuration=Debug /p:Platform="Any CPU"

# run command
dotnet run .\\SportsBook.sln /p:Configuration=Debug /p:Platform="Any CPU"
