using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using HotelDBConsole21.Interfaces;
using HotelDBConsole21.Models;
using Microsoft.Data.SqlClient;

namespace HotelDBConsole21.Services
{
    public class HotelService : Connection, IHotelService
    {
        private string queryString = "select * from po22_Hotel";
        private String queryStringFromID = "select * from po22_Hotel where Hotel_No = @ID";
        private string insertSql = "insert into po22_Hotel Values(@ID, @Navn, @Adresse)";
        private string deleteSql = "delete from po22_Hotel where Hotel_No = @ID";
        private string updateSql = "update po22_Hotel set Hotel_No = @NewID, Name = @NewName, " +
            "Address = @NewAddress where Hotel_No = @ID";
        private string queryNameString = "select * from po22_Hotel where  Name like @Navn";

        // lav selv sql strengene færdige og lav gerne yderligere sqlstrings

        /// <summary>
        /// Denne metode henter alle hoteller.
        /// </summary>
        /// <returns>Denne metode returnerer hoteller.</returns>
        public List<Hotel> GetAllHotel() {
            List<Hotel> hoteller = new List<Hotel>();
            using( SqlConnection connection = new SqlConnection(connectionString) ) {
                try {
                    SqlCommand command = new SqlCommand(queryString,connection);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader(); //som peger på alle vores data
                    while( reader.Read() ) {
                        int hotelNr = reader.GetInt32(0);
                        //int hotel HotelNr = reader.GetInt32("Hotel No"); // virkelig godt husket - det er et problem, hvis man binder sig fast på rækkefølgen.
                        string hotelNavn = reader.GetString(1);
                        string hotelAddresse = reader.GetString(2);
                        Hotel hotel = new Hotel(hotelNr,hotelNavn,hotelAddresse); //lave et nyt Hotel-objekt
                        hoteller.Add(hotel);
                    }
                }
                catch( SqlException sqlEx ) {
                    Console.WriteLine("Database error" + sqlEx.Message);
                }
                catch( Exception ex ) {
                    Console.WriteLine("Generel fejl" + ex.Message);
                }
                finally {
                    //her kommer man altid

                }

            }
            return hoteller;
        }
        /// <summary>
        /// Denne metode henter hoteller ud fra ID.
        /// </summary>
        /// <param name="hotelNr"></param>
        /// <returns>Denne metode returnerer et hotel eller et null.</returns>
        public Hotel GetHotelFromId(int hotelNr) {
            using( SqlConnection connection = new SqlConnection(connectionString) ) {
                try {
                    SqlCommand command = new SqlCommand(queryStringFromID,connection);
                    command.Parameters.AddWithValue("@ID",hotelNr);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if( !reader.Read() )
                        return null;
                    int hotelNum = reader.GetInt32(0);
                    //int hotel HotelNr = reader.GetInt32("Hotel No"); // virkelig godt husket - det er et problem, hvis man binder sig fast på rækkefølgen.
                    string hotelNavn = reader.GetString(1);
                    string hotelAddresse = reader.GetString(2);
                    Hotel hotel = new Hotel(hotelNum,hotelNavn,hotelAddresse); //lave et nyt Hotel-objekt
                    return hotel;
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
        /// Denne metode laver et hotel.
        /// </summary>
        /// <param name="hotel"></param>
        /// <returns>Denne metode returnerer antallet af rækker eller et false.</returns>
        public bool CreateHotel(Hotel hotel) {
            using( SqlConnection connection = new SqlConnection(connectionString) ) {
                try {
                    SqlCommand command = new SqlCommand(insertSql,connection);
                    command.Parameters.AddWithValue("@ID",hotel.HotelNr); //Vi har lavet en placeholder - hotelNr tager vi ud af vores parameter
                    command.Parameters.AddWithValue("@Navn",hotel.Navn);
                    command.Parameters.AddWithValue("@Adresse",hotel.Adresse);
                    command.Connection.Open();

                    int noOfRows = command.ExecuteNonQuery();
                    return noOfRows == 1;
                    //if (noOfRows == 1)
                    //{
                    //    return true;
                    //}
                    //else
                    //{
                    //    return false;
                    //}
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
        /// <summary>
        /// Denne metode opdaterer hoteller.
        /// </summary>
        /// <param name="hotel"></param>
        /// <param name="hotelNr"></param>
        /// <returns>Denne metode returnerer antallet af rækker eller false.</returns>
        public bool UpdateHotel(Hotel hotel,int hotelNr) {
            using( SqlConnection connection = new SqlConnection(connectionString) ) {
                try {
                    SqlCommand command = new SqlCommand(updateSql,connection);
                    command.Parameters.AddWithValue("@NewID",hotel.HotelNr); //Vi har lavet en placeholder - hotelNr tager vi ud af vores parameter
                    command.Parameters.AddWithValue("@NewName",hotel.Navn);
                    command.Parameters.AddWithValue("@NewAddress",hotel.Adresse);
                    command.Parameters.AddWithValue("@ID",hotelNr);
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
        /// <summary>
        /// Denne metode sletter et hotel.
        /// </summary>
        /// <param name="hotelNr"></param>
        /// <returns>Denne metode returner et hotel-variabel eller et null.</returns>
        public Hotel DeleteHotel(int hotelNr) {
            using( SqlConnection connection = new SqlConnection(connectionString) ) {
                try {
                    Hotel hotelVar = GetHotelFromId(hotelNr);
                    SqlCommand command = new SqlCommand(deleteSql,connection);
                    command.Parameters.AddWithValue("@ID",hotelNr);
                    command.Connection.Open();
                    int reader = command.ExecuteNonQuery();
                    //if (!reader.Read())
                    //    return null;
                    //int hotelNum = reader.GetInt32(0);
                    //string hotelNavn = reader.GetString(1);
                    //string hotelAddresse = reader.GetString(2);
                    //Hotel hotel = new Hotel(hotelNum, hotelNavn, hotelAddresse); //lave et nyt Hotel-objekt
                    return hotelVar;
                }
                catch {

                }
            }
            return null;
        }
        /// <summary>
        /// Denne metode henter et hotel ud fra navn.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public List<Hotel> GetHotelsByName(string name) {
            List<Hotel> hoteller = new List<Hotel>();
            using( SqlConnection connection = new SqlConnection(connectionString) ) {
                try {
                    SqlCommand command = new SqlCommand(queryNameString,connection);
                    command.Parameters.AddWithValue("@Navn",name);
                    command.Connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    while( reader.Read() ) {
                        int hotelNr = reader.GetInt32(0);
                        //int hotel HotelNr = reader.GetInt32("Hotel No"); // virkelig godt husket - det er et problem, hvis man binder sig fast på rækkefølgen.
                        string hotelNavn = reader.GetString(1);
                        string hotelAddresse = reader.GetString(2);
                        Hotel hotel = new Hotel(hotelNr,hotelNavn,hotelAddresse);
                        hoteller.Add(hotel);
                    }
                }
                catch( SqlException sqlEx ) {
                    Console.WriteLine("Database error" + sqlEx.Message);
                }
                catch( Exception ex ) {
                    Console.WriteLine("Generel fejl" + ex.Message);
                }
                finally {
                    //her kommer man altid
                }
                return hoteller;
            }
        }
    }
}
