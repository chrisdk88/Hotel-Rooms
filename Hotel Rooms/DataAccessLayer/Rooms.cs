using Hotel_Rooms.Models;
using System.Data.SqlClient;

namespace HotelH2.Controllers
{
    public class RoomsDB
    {
        string connectionString = "Data Source=PCVDATALAP117\\SQLEXPRESS;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        SqlConnection DBConnect;

        public RoomsDB()
        {
            DBConnect = new SqlConnection(connectionString);//when dbConnect is called by the Open() and Close() command, the connection to the databse opens and closes
        }

        public List<Rooms> ReadRoom()
        {
            List<Rooms> RoomsList = new List<Rooms>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                try
                {
                    string query = "SELECT * FROM [HotelRooms].[dbo].[Rooms]";

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
                                tempList.available = reader[4].ToString();
                                if (!reader.IsDBNull(5))
                                {
                                    DateTime startDateTime = reader.GetDateTime(5);
                                    tempList.dateStart = new System.DateOnly(startDateTime.Year, startDateTime.Month, startDateTime.Day);
                                }

                                if (!reader.IsDBNull(6))
                                {
                                    DateTime slutDateTime = reader.GetDateTime(6);
                                    tempList.dateEnd = new System.DateOnly(slutDateTime.Year, slutDateTime.Month, slutDateTime.Day);
                                }
                                tempList.roomNumber = (int)reader[7];

                                RoomsList.Add(tempList);
                            }
                        }
                    }
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
