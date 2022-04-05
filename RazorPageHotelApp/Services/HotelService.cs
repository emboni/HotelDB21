using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RazorPageHotelApp.Exceptions;
using RazorPageHotelApp.Interfaces;
using RazorPageHotelApp.Models;

namespace RazorPageHotelApp.Services
{
    public class HotelService : Connection, IHotelService
    {

        private String queryString = "select * from po22_Hotel";
        private String queryNameString = "select * from po22_Hotel where  lower(Name) like lower(@Navn) " +
                                         "or lower(Address) like lower(@Navn)";
        private String queryStringFromID = "select * from po22_Hotel where Hotel_No = @ID";
        private String insertSql = "insert into po22_Hotel Values (@ID, @Navn, @Adresse)";
        private String deleteSql = "delete from po22_Hotel where Hotel_No = @ID";
        private String updateSql = "update po22_Hotel " +
                                   "set Hotel_No= @HotelID, Name=@Navn, Address=@Adresse " +
                                   "where Hotel_No = @ID";


        public HotelService(IConfiguration configuration) : base(configuration) {

        }
        public HotelService(string costring) : base(costring) { 
            
        }
        public async Task<List<Hotel>> GetAllHotelAsync() {
            List<Hotel> hoteller = new List<Hotel>();
            using( SqlConnection connection = new SqlConnection(connectionString) ) {
                using( SqlCommand command = new SqlCommand(queryString,connection) ) {
                    await command.Connection.OpenAsync();
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while( await reader.ReadAsync() ) {
                        int hotelNr = reader.GetInt32(0);
                        String hotelNavn = reader.GetString(1);
                        String hotelAdr = reader.GetString(2);
                        Hotel hotel = new Hotel(hotelNr,hotelNavn,hotelAdr);
                        hoteller.Add(hotel);
                    }
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
                    if( !await reader.ReadAsync() )
                        return null;
                    int hotelNum = reader.GetInt32(0);
                    string hotelNavn = reader.GetString(1);
                    string hotelAddresse = reader.GetString(2);
                    Hotel hotel = new Hotel(hotelNum,hotelNavn,hotelAddresse);
                    return hotel;
                }
                catch( SqlException sqlEx ) {
                    throw new DatabaseException(sqlEx.Message);
                }
            }
        }

        public async Task<bool> CreateHotelAsync(Hotel hotel) {
            using( SqlConnection connection = new SqlConnection(connectionString) ) {
                try {
                    SqlCommand command = new SqlCommand(insertSql,connection);
                    command.Parameters.AddWithValue("@ID",hotel.HotelNr);
                    command.Parameters.AddWithValue("@Navn",hotel.Navn);
                    command.Parameters.AddWithValue("@Adresse",hotel.Adresse);
                    await command.Connection.OpenAsync();

                    int noOfRows = await command.ExecuteNonQueryAsync();
                    return noOfRows == 1;
                }
                catch( SqlException sqlEx ) {
                    throw new DatabaseException(sqlEx.Message);
                }
            }
            return false;
        }

        public async Task<bool> UpdateHotelAsync(Hotel hotel,int hotelNr) {
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
                    throw new DatabaseException(sqlEx.Message);
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
                catch( SqlException sqlEx ) {
                    throw new DatabaseException(sqlEx.Message);
                }
            }
            return null;
        }
        
        public async Task<List<Hotel>> GetHotelsByNameAsync(string name) {
            List<Hotel> hoteller = new List<Hotel>();
            using( SqlConnection connection = new SqlConnection(connectionString) ) {
                try {
                    SqlCommand command = new SqlCommand(queryNameString,connection);
                    command.Parameters.AddWithValue("@Navn", '%' + name + '%');
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
                    throw new DatabaseException(sqlEx.Message);
                }
                return hoteller;
            }
        }

        
    }
}
