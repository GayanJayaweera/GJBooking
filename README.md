

 The below sql table should be created before running the application

CREATE TABLE [dbo].[GJApptTest](
      [ApptID] [int] IDENTITY(1,1) NOT NULL,
      [ApptDate] [date] NULL,
      [ApptStartTime] [time](7) NULL
) ON [PRIMARY]
GO


        static string connectionString = "Server=localDB; Database=TALTEST;Integrated Security = True;";
Please update connection string as well.  I was on the laption and didnt have time to install sql express so I used a network sql server

Year 2024 is appended as default   -  Wider Date validation
Cleanup formating
Case Insensitive
Show detailed help with possible option when no or invalid  parameters are entered in the command line 
in ADD check the 30 minute overlap before adding an appointment
FIND Finds the first appointment avaiable 
KEEP  only looks at Future Dates From Today and Automatically adds an appointment when a date is found . 

option to view all appointments by day  if required.   (currently have to use SSMS)

