using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HotelDBConsole21.Interfaces;
using HotelDBConsole21.Models;
using Microsoft.Data.SqlClient;

namespace HotelDBConsole21.Services
{
    class HotelServiceAsync : Connection, IHotelServiceAsync
    {
        private string queryString = "select * from po22_Hotel";
        private String queryStringFromID = "select * from po22_Hotel where Hotel_No = @ID";
        private string insertSql = "insert into po22_Hotel Values(@ID, @Navn, @Adresse)";
        private string deleteSql = "delete from po22_Hotel where Hotel_No = @ID";
        private string updateSql = "update po22_Hotel set Hotel_No = @NewID, Name = @NewName, Address = @NewAddress where Hotel_No = @ID";
        private string queryNameString = "select * from po22_Hotel where  Name like @Navn";

        public async Task<List<Hotel>> GetAllHotelAsync() //Navnekonvention - pakket ind i en Task
        {
            List<Hotel> hoteller = new List<Hotel>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(queryString, connection);
                    await command.Connection.OpenAsync();
                    Thread.Sleep(1000);
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    Thread.Sleep(1000);
                    while (await reader.ReadAsync())
                    {
                        int hotelNr = reader.GetInt32(0);
                        //int hotel HotelNr = reader.GetInt32("Hotel No"); // virkelig godt husket - det er et problem, hvis man binder sig fast på rækkefølgen.
                        string hotelNavn = reader.GetString(1);
                        string hotelAddresse = reader.GetString(2);
                        Hotel hotel = new Hotel(hotelNr, hotelNavn, hotelAddresse); //lave et nyt Hotel-objekt
                        hoteller.Add(hotel);
                    }
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Database error" + sqlEx.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl" + ex.Message);
                }
                finally
                {
                    //her kommer man altid

                }

            }
            return hoteller;
        }

        public async Task<Hotel> GetHotelFromIdAsync(int hotelNr) {
            using( SqlConnection connection = new SqlConnection(connectionString) ) {
                try {
                    SqlCommand command = new SqlCommand(queryStringFromID,connection);
                    command.Parameters.AddWithValue("@ID",hotelNr);
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    if( ! await reader.ReadAsync() )
                        return null;
                    int hotelNum = reader.GetInt32(0);
                    string hotelNavn = reader.GetString(1);
                    string hotelAddresse = reader.GetString(2);
                    Hotel hotel = new Hotel(hotelNum,hotelNavn,hotelAddresse);
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

        public async Task<bool> CreateHotelAsync(Hotel hotel)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand(insertSql, connection);
                    command.Parameters.AddWithValue("@ID", hotel.HotelNr); //Vi har lavet en placeholder - hotelNr tager vi ud af vores parameter
                    command.Parameters.AddWithValue("@Navn", hotel.Navn);
                    command.Parameters.AddWithValue("@Adresse", hotel.Adresse);
                    await command.Connection.OpenAsync();

                    int noOfRows = await command.ExecuteNonQueryAsync();
                    return noOfRows == 1;
                }
                catch (SqlException sqlEx)
                {
                    Console.WriteLine("Database error" + sqlEx.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Generel fejl" + ex.Message);
                }
            }
            return false;
        }

        public async Task<bool> UpdateHotelAsync(Hotel hotel, int hotelNr)
        {
            using( SqlConnection connection = new SqlConnection(connectionString) ) {
                try {
                    SqlCommand command = new SqlCommand(updateSql,connection);
                    command.Parameters.AddWithValue("@NewID",hotel.HotelNr); //Vi har lavet en placeholder - hotelNr tager vi ud af vores parameter
                    command.Parameters.AddWithValue("@NewName",hotel.Navn);
                    command.Parameters.AddWithValue("@NewAddress",hotel.Adresse);
                    command.Parameters.AddWithValue("@ID",hotelNr);
                    await command.Connection.OpenAsync();

                    int noOfRows = await command.ExecuteNonQueryAsync();
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

        public async Task<Hotel> DeleteHotelAsync(int hotelNr) {
            using( SqlConnection connection = new SqlConnection(connectionString) ) {
                try {
                    //Hotel hotelVar = await GetHotelFromIdAsync(hotelNr);
                    Hotel hotelVar = GetHotelFromIdAsync(hotelNr).Result;
                    SqlCommand command = new SqlCommand(deleteSql,connection);
                    command.Parameters.AddWithValue("@ID",hotelNr);
                    await command.Connection.OpenAsync();
                    int reader = await command.ExecuteNonQueryAsync();
                    return hotelVar;
                }
                catch {

                }
            }
            return null;
        }
        
        public async Task<List<Hotel>> GetHotelsByNameAsync(string name)
        {
            List<Hotel> hoteller = new List<Hotel>();
            using( SqlConnection connection = new SqlConnection(connectionString) ) {
                try {
                    SqlCommand command = new SqlCommand(queryNameString,connection);
                    command.Parameters.AddWithValue("@Navn",name);
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = command.ExecuteReader();
                    while( await reader.ReadAsync() ) {
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
