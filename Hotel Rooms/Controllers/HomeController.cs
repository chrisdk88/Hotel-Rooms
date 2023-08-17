using Hotel_Rooms.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;

namespace HotelH2.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public bool bookingUpdate(Rooms bookingUpdate)
        {
            string query =  "UPDATE [HotelRooms].[dbo].[Rooms] " +
                            "SET type = @Type, price = @price, available = 1" +
                            "WHERE roomNumber = @roomNumber";

            using (SqlConnection connection = new SqlConnection("Data Source=PCVDATALAP117\\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@roomNumber", bookingUpdate.roomNumber);
                        cmd.Parameters.AddWithValue("@Type", bookingUpdate.type);
                        cmd.Parameters.AddWithValue("@price", bookingUpdate.price);
                        //cmd.Parameters.AddWithValue("@StartDate", bookingUpdate.dateStart);
                        //cmd.Parameters.AddWithValue("@EndDate", bookingUpdate.dateEnd);

                        int rowsAffected = cmd.ExecuteNonQuery();


                        return rowsAffected > 0;
                    }
                }
                catch (SqlException ex)
                {
                    // Handle exceptions here
                    return false;
                }
            }
        }
        public IActionResult cancel()
        {
            return View();
        }

        public bool bookingCancel(Rooms bookingCancel)
        {
            string query = "UPDATE [HotelRooms].[dbo].[Rooms]" +
                            " SET available = 0" +
                            " WHERE roomNumber = @roomNumber"; //bookingCancel.roomNumber;

            using (SqlConnection connection = new SqlConnection("Data Source=PCVDATALAP117\\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                try
                {
                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        string setPrice = "0";
                        if (bookingCancel.type == "standard")
                        {
                            setPrice = "600";
                        }
                        else if (bookingCancel.type == "Premium")
                        {
                            setPrice = "1200";
                        }
                        else if (bookingCancel.type == "Deluxe")
                        {
                            setPrice = "2400";
                        }
                        cmd.Parameters.AddWithValue("@roomNumber", bookingCancel.roomNumber);
                        cmd.Parameters.AddWithValue("@Type", bookingCancel.type);
                        cmd.Parameters.AddWithValue(setPrice, bookingCancel.price);
                        //cmd.Parameters.AddWithValue("@StartDate", bookingUpdate.dateStart);
                        //cmd.Parameters.AddWithValue("@EndDate", bookingUpdate.dateEnd);

                        int rowsAffected = cmd.ExecuteNonQuery();


                        return rowsAffected > 0;
                    }
                }
                catch (SqlException ex)
                {
                    // Handle exceptions here
                    return false;
                }
            }
        }

        public IActionResult Booking(int id)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=PCVDATALAP117\\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM [HotelRooms].[dbo].[Rooms] WHERE id = @ID";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ID", id);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Rooms roomToUpdate = new Rooms();
                                roomToUpdate.roomNumber = (int)reader[7];
                                roomToUpdate.type = (string)reader[1];
                                roomToUpdate.price = (int)reader[3];

                                return View(roomToUpdate);
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                    }
                }
                catch (SqlException ex)
                {
                    return View("Error");
                }
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Status()
        {
            RoomsDB r = new RoomsDB();
            List<Rooms> RoomsList = r.ReadRoom();
            for (int i = 0; i < RoomsList.Count; i++)
            {
                if (RoomsList[i].available == "False")
                {
                    RoomsList[i].available = "Available";
                }
                else
                {
                    RoomsList[i].available = "Occupied";
                }
            }

            return View(RoomsList);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        
    }
}