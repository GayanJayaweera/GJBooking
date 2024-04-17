// See https://aka.ms/new-console-template for more information
//Technical Test Calendar Booking - C#
//Instructions
//Using the latest .NET release create a simple console command line Application that
//demonstrates knowledge of:
//• Database Development
//• C# Programming
//• Dependency Injection and/or other suitable design patterns
//• Unit Testing
//• Documentation
//Requirements
//• The application must accept the following commands from the command line:
//o ADD DD/MM hh:mm to add an appointment.
//o DELETE DD/MM hh:mm to remove an appointment.
//o FIND DD/MM to find a free timeslot for the day.
//o KEEP hh:mm keep a timeslot for any day.
//• The time slot will be always equal to 30 minutes.
//• The application can assign any slot on any day, with the following constraints:
//o The acceptable time is between 9AM and 5PM
//o Except from 4 PM to 5 PM on each second day of the third week of any month -
//this must be reserved and unavailable

//• Use SQL Server Express LocalDB for the state storage.
//• You are NOT required to build any GUI.
//What we are looking for:
//1.The solution needs to be fully functioning as per the requirements above.
//2. Share the details of your repository for access by the TAL members you will interview
//with
//3. You do not need to spend more than 3 hours on this task - instead...
//4. In the project READ.ME, list out the areas of improvement and refinement if you had a
//full 2 days to build this application.



using GJBooking;
using Microsoft.VisualBasic;

Console.WriteLine("Booking System  Coded by Gayan Jayaweera based on TAL test  (c) 16-Apr-2024");

string retvalue = "";
string result = "";
int retval = 0;

if (args.Length == 0)
{
    retvalue += "No parameters found";
    retval = -1;
}

else

{
    switch (args[0])
    {
        case "ADD"://o ADD DD/MM hh:mm to add an appointment.
            retvalue += "Add ";
            if (args.Length == 3)
            {
                string dateIn = args[1] + "/2024 " + args[2];
                var getDate = ValiDate.Vdatetime(dateIn);
                if (getDate.HasValue)
                {
                    DateTime dateOut = getDate.Value;
                    retvalue += "  Adding Datevalue " + dateIn + " " + dateOut.ToString();
                    SQLDB.AddAppointment(dateOut);
                }

                else
                {
                    retvalue += " Invalid Date Time Parameter. " + dateIn;
                }
            }
            else

            {
                retvalue += " Date Time Parameter not Found. ";
            }

            break;

        case "DELETE":
            retvalue += "Delete ";
            if (args.Length == 3)
            {
                string dateIn = args[1] + "/2024 " + args[2];
                var getDate = ValiDate.Vdatetime(dateIn);
                if (getDate.HasValue)
                {
                    DateTime dateOut = getDate.Value;
                    retvalue += "  Deleteing  Datevalue " + dateIn + " " + dateOut.ToString();
                    SQLDB.DeleteAppointment(dateOut);
                }

                else
                {
                    retvalue += " Invalid Date Time Parameter. " + dateIn;
                }
            }
            else
            {
                retvalue += " Date Time Parameter not Found. ";
            }

            break;

        case "FIND": //o FIND DD/MM to find a free timeslot for the day.
            retvalue += "Find ";
            if (args.Length == 2)
            {
                string dateIn = args[1] + "/2024 ";
                var getDate = ValiDate.Vdatetime(dateIn);
                if (getDate.HasValue)
                {
                    DateTime dateOut = getDate.Value;
                    retvalue += " Finding  Datevalue " + dateIn + " " + dateOut.ToString();
                    DateTime apptOut = SQLDB.FindAppointment(dateOut);
//                    Console.WriteLine(" Earliest Appointment found at " + apptOut);

                    // Check Evening Condition 

                    DateTime endAppt = apptOut.AddMinutes(29);
//                    Console.WriteLine(" Appointment end at  " + endAppt);

                    if (endAppt.Hour >= 17)
                    {
                        retvalue += " No Appointments Avaiable.";

                    }
                    else
                    // 3rd tuesday of the month. 15 and 23

                    if (endAppt.Hour >= 16 && dateOut.Day > 15 && dateOut.Day < 23 && dateOut.DayOfWeek == DayOfWeek.Tuesday)
                    {
                        retvalue += " No appoitments before 4 avaiable.";
                    }
                    else
                    {
                        retvalue += " Appointment found on " + dateOut.ToString("dd/MM") + " at " + apptOut.ToString("HH:mm");
                        result = " Appointment found on " + dateOut.ToString("dd/MM") + " at " + apptOut.ToString("HH:mm");
                    }
                }
                else
                {
                    retvalue += " Invalid Date Time Parameter. " + dateIn;
                }
            }
            else
            {
                retvalue += " Date Time Parameter not Found. ";
            }
            break;


        case "KEEP":  ////o KEEP hh:mm keep a timeslot for any day.
            retvalue += "Keep ";
            if (args.Length == 2)
            {
                string chkDate = DateTime.Today.ToString().Substring(0, 10) + " " + args[1];
                 var getDate = ValiDate.Vdatetime(chkDate);
                if (getDate.HasValue)
                {
 //                   Console.WriteLine(" KEEP3 :" + getDate.ToString());

                    DateTime dateOut = getDate.Value;
                    retvalue += "  Checking  Datevalue  " + dateOut.ToString();
                    bool found = false;

                    while (!found)
                    {
                        found = SQLDB.CheckAppt(dateOut);
                        if (found)
                        {
                            retvalue += " Appointment Added to " + dateOut.ToString();
                            result = " Appointment Added to " + dateOut.ToString();
                        }
                        else
                        {
                            dateOut = dateOut.AddDays(1);
                        }
                    }
                }
                else
                {
                    retvalue += " Invalid Date Time Parameter. " + chkDate;
                }
            }
            else

            {
                retvalue += " Date Time Parameter not Found. ";
            }


            break;


        default:
            retvalue += "Invalid Command ";
            break;
    }
}


retvalue += args[0] + " Done";
if (result != "")
{
    Console.WriteLine("Results :"+result);
}

else
{
    Console.WriteLine("Completed with Log " + retvalue);
    //    Console.WriteLine(result);
}
return retval;

