using System;
using System.Collections.Generic;
using System.Text;
using HotelDBConsole21.Interfaces;
using HotelDBConsole21.Models;
using Microsoft.Data.SqlClient;

namespace HotelDBConsole21.Services
{
    public class RoomService : Connection, IRoomService //Arve fra vores connection-klasse
    {
        // lad klassen arve fra interfacet IRoomService og arve fra Connection klassen
        // indsæt sql strings

        private string getAllSql = "select * from po22_Room";
        private string getAllHotelRooms = "select * from po22_Room where Hotel_No = @ID";
        private string insertRoom = "insert into po22_Room Values(@IDRoom, @IDHotel, @Type, @Pris)";
        private string deleteRoom = "delete from po22_Room where Room_No = @ID AND Hotel_No = @HotelID";
        private string searchRoom = "select * from po22_Room where Room_No = @ID AND Hotel_No = @HotelID";
        private string updateRoom = "update po22_Room set Room_No = @NewID, Hotel_No = @NewHotelID, " +
            "Types = @NewTypeID, Price = @NewPriceID where Room_No = @OldID AND Hotel_No = @Hotel_No";

        //Implementer metoderne som der skal ud fra interfacet

        /// <summary>
        /// Denne metode laver et værelse.
        /// </summary>
        /// <param name="hotelNr"></param>
        /// <param name="room"></param>
        /// <returns>Denne metode returnerer antallet af rækker eller et false.</returns>
        public bool CreateRoom(int hotelNr, Room room)
        {
            using( SqlConnection connection = new SqlConnection(connectionString) ) {
                try {
                    SqlCommand command = new SqlCommand(insertRoom,connection);
                    command.Parameters.AddWithValue("@IDRoom",room.RoomNr);
                    command.Parameters.AddWithValue("@IDHotel",room.HotelNr);
                    command.Parameters.AddWithValue("@Type",room.Types);
                    command.Parameters.AddWithValue("@Pris",room.Pris);
                    command.Connection.Open();
                    int noOfRows = command.ExecuteNonQuery();
                    return noOfRows == 1;
                }
                catch( SqlException sqlEx ) {
                    Console.WriteLine("Database error: " + sqlEx.Message);
                }
                catch( Exception ex ) {
                    Console.WriteLine("Generel fejl: " + ex.Message);
                }
            }
            return false;
        }

        /// <summary>
        /// Denne metode fjerner et værelse.
        /// </summary>
        /// <param name="roomNr"></param>
        /// <param name="hotelNr"></param>
        /// <returns>Denne metode returnerer et værelse.</returns>
        public Room DeleteRoom(int roomNr, int hotelNr)
        {
            Room room = GetRoomFromId(roomNr,hotelNr);
            using( SqlConnection connection = new SqlConnection(connectionString) ) {
                try {
                    SqlCommand command = new SqlCommand(deleteRoom,connection);
                    command.Parameters.AddWithValue("@ID",roomNr);
                    command.Parameters.AddWithValue("@HotelID",hotelNr);
                    command.Connection.Open();
                    command.ExecuteNonQuery();
                }
                catch( SqlException sqlEx ) {
                    Console.WriteLine("Database error" + sqlEx.Message);
                }
                catch( Exception ex ) {
                    Console.WriteLine("Generel fejl" + ex.Message);
                }
                return room;
            }
        }

        /// <summary>
        /// Denne metode henter alle værelser.
        /// </summary>
        /// <returns>Denne metode returnerer værelser.</returns>
        public List<Room> GetAllRooms() {
            List<Room> værelser = new List<Room>();
            try {
                using( SqlConnection connection = new SqlConnection(connectionString) ) {
                    SqlCommand command = new SqlCommand(getAllSql,connection);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while( reader.Read() ) {
                        int RoomNr = reader.GetInt32(0);
                        int HotelNr = reader.GetInt32(1);
                        char Types = reader.GetString(2)[0];
                        double Pris = reader.GetDouble(3);
                        Room room = new Room(RoomNr,Types,Pris,HotelNr);
                        værelser.Add(room);
                    }
                }
            }
            catch(SqlException sqlEx) {
                Console.WriteLine("Database error" + sqlEx.Message);
            }
            catch(Exception ex) {
                Console.WriteLine("Generel fejl" + ex.Message);
            }
            finally {
            }
            return værelser;
        }

        /// <summary>
        /// Denne metode henter alle værelser fra et hotel.
        /// </summary>
        /// <param name="hotelNr"></param>
        /// <returns>Denne metode returnerer værelser.</returns>
        public List<Room> GetAllRoomsFromHotel(int hotelNr) {
            List<Room> værelser = new List<Room>();
            try {
                using( SqlConnection connection = new SqlConnection(connectionString) ) {
                    SqlCommand command = new SqlCommand(getAllHotelRooms,connection);
                    command.Parameters.AddWithValue("@ID",hotelNr);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while( reader.Read() ) {
                        int RoomNr = reader.GetInt32(0);
                        int HotelNr = reader.GetInt32(1);
                        char Types = reader.GetString(2)[0];
                        double Pris = reader.GetDouble(3);
                        Room room = new Room(RoomNr,Types,Pris,HotelNr);
                        værelser.Add(room);
                    }
                }
            }
            catch( SqlException sqlEx ) {
                Console.WriteLine("Database error" + sqlEx.Message);
            }
            catch( Exception ex ) {
                Console.WriteLine("Generel fejl" + ex.Message);
            }
            finally {
            }
            return værelser;
        }

        /// <summary>
        /// Denne metode henter værelse ud fra ID.
        /// </summary>
        /// <param name="roomNr"></param>
        /// <param name="hotelNr"></param>
        /// <returns>Denne metode returnerer et værelse eller et null.</returns>
        public Room GetRoomFromId(int roomNr, int hotelNr)
        {
            using( SqlConnection connection = new SqlConnection(connectionString) ) {
                try {
                    SqlCommand command = new SqlCommand(searchRoom,connection);
                    command.Parameters.AddWithValue("@ID",roomNr);
                    command.Parameters.AddWithValue("@HotelID",hotelNr);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if( !reader.Read() )
                        return null;
                    int roomNum = reader.GetInt32(0);
                    int hotelNum = reader.GetInt32(1);
                    char type = reader.GetString(2)[0];
                    double pris = reader.GetDouble(3);
                    Room room = new Room(roomNum,type,pris,hotelNum);
                    return room;
                }
                catch( SqlException sqlEx ) {
                    Console.WriteLine("Database error" + sqlEx.Message);
                }
                catch( Exception ex ) {
                    Console.WriteLine("Generel fejl" + ex.Message);
                }
                return null;
            }
        }
        /// <summary>
        /// Denne metode opdaterer et værelse.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="roomNr"></param>
        /// <param name="hotelNr"></param>
        /// <returns>Denne metode returnerer antallet af rækker eller et false.</returns>
        public bool UpdateRoom(Room room, int roomNr, int hotelNr)
        {
            using( SqlConnection connection = new SqlConnection(connectionString) ) {
                try {
                    SqlCommand command = new SqlCommand(updateRoom,connection);
                    command.Parameters.AddWithValue("@NewID",room.RoomNr);
                    command.Parameters.AddWithValue("@NewHotelID",room.HotelNr);
                    command.Parameters.AddWithValue("@NewTypeID",room.Types);
                    command.Parameters.AddWithValue("@NewPriceID",room.Pris);
                    command.Parameters.AddWithValue("@OldID",roomNr);
                    command.Parameters.AddWithValue("@Hotel_No",hotelNr);
                    command.Connection.Open();
                    int noOfRows = command.ExecuteNonQuery();
                    return noOfRows == 1;
                }
                catch( SqlException sqlEx ) {
                    Console.WriteLine("Database error" + sqlEx.Message);
                }
                catch( Exception ex ) {
                    Console.WriteLine("Generel fejl" + ex.Message);
                }
            }
            return false;
        }
    }
}
