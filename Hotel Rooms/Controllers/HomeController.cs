using Hotel_Rooms.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;

namespace HotelH2.Controllers
{
    public class HomeController : Controller
    {
        public List<Rooms> RoomsList = new List<Rooms>();


        public IActionResult Index()
        {
            ReadRoom(RoomsList);

            return View(RoomsList);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public List<Rooms> ReadRoom(List<Rooms> RoomsList)
        {
            string connectionString = "Data Source=PCVDATALAP117\\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string query = "SELECT * FROM [HotelRooms].[dbo].[Rooms]";

                    // my method
                    void Print(string query)
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            using (SqlDataReader reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    Rooms tempList = new Rooms();

                                    tempList.id = (int)reader[0];
                                    tempList.type = (string)reader[1];
                                    tempList.roomTemp = (int)reader[2];
                                    tempList.price = (int)reader[3];
                                    tempList.available = (bool)reader[4];
                                    if (!reader.IsDBNull(5))
                                    {
                                        DateTime startDateTime = reader.GetDateTime(5);
                                        tempList.dateStart = new System.DateOnly(startDateTime.Year, startDateTime.Month, startDateTime.Day);
                                    }
                                    else
                                    {

                                    }
                                    if (!reader.IsDBNull(6))
                                    {
                                        DateTime slutDateTime = reader.GetDateTime(6);
                                        tempList.dateEnd = new System.DateOnly(slutDateTime.Year, slutDateTime.Month, slutDateTime.Day);
                                    }

                                    RoomsList.Add(tempList);
                                }

                            }
                        }
                    }

                    Print(query);

                    connection.Close();
                }
                catch (SqlException e)
                {

                    throw e;
                }
            }
            return RoomsList;
        }
    }
}