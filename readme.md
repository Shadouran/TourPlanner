#Architecture

The app uses a layer based client-server architecture.
Projects realize each of the different layers:
*Client contains the MVVM based GUI (view layer)
*Client.BL contains the report generation as well as methods regarding tour and log management (business layer, logic)
*Client.DAL defines the interfaces used for data access (data access layer interface)
*Client.DAL.Endpoint is one possible implementation of Client.DAL, defines the server API endpoint access and takes care of the client-side file management (data access layer implementation)
*Server contains the endpoints as well as various logic (server business layer)
*Server.DAL has the database access, server-side filesystem management (server data access layer)
*Shared contains classes used on both sides, like file management and logging
*Shared contains the logging interface and manager as well as the filesystem definition and implementation used in the DALs
*Shared.Log4Net is a logging implentation
*Shared.Models defines models used throughout the program
*Client.Test and Server.Test contain the tests for the respective projects

#UX

Users use client app to access the service, which allows CRUD of tours, as well as logs associated with each tour,
report generation of single tours containing all information as well as a list of all logs
or statistical summary of all tours that have some selected information.
The user can create a tour specifying a name, distance, start and end point, transport type as well as route information and description.
If a viable start and end point have been declared a picture of a map showing the route is added.
Logs are associated to a single tour and can be created and edited giving a start time, total time spent, rating and difficulty.
Popularity and child-friendliness of tours is automatically calculated based on the distance, amount of logs, and their average rating and time spent.

#Library decisions

I used Newtonsoft.Json for json file handling between client and server since I had used it before and the simplicity makes it easily readable.
The client side image handling is done via System.Drawing.Common which is an offical Microsoft library, but it only works on windows so this has to be refactored
should other client implementation be done.
Logging is done via Log4Net since this is the library we used in class and implemented.
For report generation I used QuestPDF. It is a rather small library and has extremely readable document generation code.
It also has a preview done via an extern executable as well as a VSCode extension which allows for fast document designing.
CvsHelper, chosen for its ease of use, is used in the Filesystem class for importing/exporting of tours in CVS format.

#Design patterns

I used the Builder design on the server side in MapQuestUriBuilder:
The constructor takes the MapQuest API key and each method then adds specific parameters needed for the API call.
This allows for easy and readable URI generation.

On the client side I implemented a factory for the report generation.
Here only a factory for QuestPdfGenerator using QuestPDF is implemented but it can easily be extended with other libraries.
The IReportGeneratorFactory creates a preconfigured instance of a class implementing IReportGenerator. QuestPDF does not need anything configurated so this was simply done for future-proofing.
