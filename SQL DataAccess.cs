
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;

namespace GJBooking
{
    internal class SQLDB
    {
        static string connectionString = "Server=localDB; Database=TALTEST;Integrated Security = True;";
        public static bool AddAppointment(DateTime addDate)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "insert into GJApptTest(ApptDate, ApptStartTime) values(@aDate, @aTime)";

                    command.Parameters.AddWithValue("@aDate", addDate);
                    command.Parameters.AddWithValue("@aTime", addDate);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        public static bool DeleteAppointment(DateTime addDate)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM GJApptTest Where ApptDate=CONVERT(VARCHAR(10),@aDate,23) and ApptStartTime=  CONVERT(VARCHAR(5),@aTime,108)";
                    command.Parameters.AddWithValue("@aDate", addDate);
                    command.Parameters.AddWithValue("@aTime", addDate);
                    connection.Open();
                    command.ExecuteNonQuery();
                    connection.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }

        }

        public static DateTime FindAppointment(DateTime findDate)
        {
            //   try
            {

                //    DateTime freeAppt = findDate.AddHours(9);
                DateTime freeAppt = DateTime.Today.AddHours(9);

                using (var connection = new SqlConnection(connectionString))
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT ApptStartTime,DATEADD(MI,30,APPTSTARTTIME) AS ApptEndTime FROM GJApptTest  WHERE ApptDate=CONVERT(VARCHAR(10),@findDate,23) ORDER  BY APPTSTARTTIME";
                    command.Parameters.AddWithValue("@findDate", findDate);
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            DateTime apptStartTime;
                            DateTime.TryParse(reader[0].ToString(), out apptStartTime);
                            DateTime apptEndTime;
                            DateTime.TryParse(reader[1].ToString(), out apptEndTime);
                            TimeSpan start = apptStartTime.Subtract(freeAppt);
//                            Console.WriteLine("timegap is :: " + start.TotalMinutes.ToString());

                            if (start.TotalMinutes < 30)
                            {
//                                Console.WriteLine(" Cant Do it it is too close ");
                                freeAppt = apptEndTime;
                            }
                            else
                            {
                                TimeSpan ts = apptEndTime.Subtract(freeAppt);
//                                Console.WriteLine("timegap2 is :: " + ts.TotalMinutes.ToString());

  //                              Console.WriteLine("No. of Minutes (Difference) = {0}", ts.TotalMinutes);
                                if (ts.TotalMinutes >= 30)
                                {
//                                    Console.WriteLine(" found a slot ");
                                    return freeAppt;
                                }
                                freeAppt = apptEndTime;
                            }
                        }
//                        Console.WriteLine(" End Reading ");
                    }
                }
                return freeAppt;
            }
        }
        public static bool CheckAppt(DateTime getDate)
        {
//            Console.WriteLine("Checking Appointment :" + getDate.ToString());
            using (var connection = new SqlConnection(connectionString))
            using (var command = connection.CreateCommand())
            {
                command.CommandText = "SELECT apptid,apptdate, ApptStartTime,DATEADD(MI,30,APPTSTARTTIME) AS ApptEndTime FROM GJApptTest " +
                    "where  apptdate =CONVERT(VARCHAR(10),@findDate,23) AND  APPTSTARTTIME <CONVERT(VARCHAR(5),@endtime,108) AND DATEADD(MI,30,APPTSTARTTIME) >CONVERT(VARCHAR(5),@starttime,108)";
                command.Parameters.AddWithValue("@finddate", getDate);
                command.Parameters.AddWithValue("@starttime", getDate);
               
                DateTime enddate = getDate.AddMinutes(30);
                command.Parameters.AddWithValue("@endtime", enddate);

                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    return false;
                }
                else
                {
                    AddAppointment(getDate);
                    return true;
                }
            }
        }
    }
}
