using Hotel_Rooms.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace HotelH2.Controllers
{
    public class RoomsController : Controller
    {
        public IActionResult Index()
        {
            List<Rooms> Rooms = new List<Rooms>();
            string connectionString = "Data Source=PCVDATALAP117\\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT id, type, roomTemp, price, available, dateStart, dateEnd FROM [HotelRooms].[dbo].[Rooms]";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Rooms.Add(new Rooms
                            {
                                id = (int)reader["id"],
                                type = reader["type"].ToString(),
                                roomTemp = (int)reader["roomTemp"],
                                price = (int)reader["price"],
                                available = (bool)reader["available"],
                                dateStart = (DateOnly)reader["dateStart"],
                                dateEnd = (DateOnly)reader["dateEnd"],
                            });
                        }
                    }
                }
            }
            return View();
        }
    }
}
