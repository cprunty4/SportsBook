# misson
Currently 6 states have legalized sports betting (New Jersey, Pennsylvania, Indiana etc)  I want to build an app that has the capability to enter the online sports gambiling space.   

My plan is release a NFL betting site before0 the start of the 2020 Season which starts September 13th.  My goal is start specific to NFL football but build a system that scalable to the other professional sports.


# background
I started this code in 2020 that runs on the Azure cloud <a href="https://cullens-sportsbook.azurewebsites.net/">click here</a>

This repo contains the front end code.  The back end api code is in a different github repo that I started in 2017.   The back end has a admin tool that connects to a Azure SQL database.

I'm using Visual Studio Code.  here's some commands I use:

# build command
dotnet build .\\SportsBook.sln /p:Configuration=Debug /p:Platform="Any CPU"

# run command
dotnet run .\\SportsBook.sln /p:Configuration=Debug /p:Platform="Any CPU"
