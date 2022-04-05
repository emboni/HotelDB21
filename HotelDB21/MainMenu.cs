using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HotelDBConsole21.Models;
using HotelDBConsole21.Services;

namespace HotelDBConsole21
{
    public static class MainMenu
    {
        //Lav selv flere menupunkter til at vælge funktioner for Rooms
        public static void showOptions()
        {
            Console.Clear();
            Console.WriteLine("Vælg et menupunkt");
            Console.WriteLine("1) List hoteller");
            Console.WriteLine("1a) List hoteller async");
            Console.WriteLine("2) Opret nyt Hotel");
            Console.WriteLine("2a) Opret nyt Hotel async");
            Console.WriteLine("3) Fjern Hotel");
            Console.WriteLine("4) Søg efter hotel udfra hotelnr");
            Console.WriteLine("5) Opdater et hotel");
            Console.WriteLine("6) List alle værelser");
            Console.WriteLine("7) List alle til et bestemt hotel");
            Console.WriteLine("8) Opret nyt værelse");
            Console.WriteLine("9) Fjern et værelse");
            Console.WriteLine("10) Søg efter et givent værelse");
            Console.WriteLine("11) Opdater et værelse");
            Console.WriteLine("12) kommer snart");
            Console.WriteLine("Q) Afslut");

            //Console.Clear();
            //Console.WriteLine("Vælg et menupunkt");
            //Console.WriteLine("1) List hoteller");
            //Console.WriteLine("1a) List hoteller async");
            //Console.WriteLine("2) Opret nyt Hotel");
            //Console.WriteLine("3) Fjern Hotel");
            //Console.WriteLine("4) Søg efter hotel udfra hotelnr");
            //Console.WriteLine("5) Opdater et hotel");
            //Console.WriteLine("6) List alle værelser");
            //Console.WriteLine("7) List alle værelser til et bestemt hotel");
            //Console.WriteLine("8) Flere menupunkter kommer snart :) ");
            //Console.WriteLine("Q) Afslut");
        }

        public static bool Menu()
        {
            showOptions();
            switch (Console.ReadLine())
            {
                case "1":
                    ShowHotels();
                    return true;
                case "1a":
                    ShowHotelsAsync();
                    DoSomething();
                    return true;
                case "2":
                    CreateHotel();
                    return true;
                case "2a":
                    CreateHotelAsync();
                    return true;
                case "3":
                    DeleteHotel();
                    return true;
                case "4":
                    FindHotelByNum();
                    return true;
                case "5":
                    UpdateHotel();
                    return true;
                case "6":
                    ListAllRooms();
                    return true;
                case "7":
                    ListAllRoomsByHotel();
                    return true;
                case "8":
                    CreateRoom();
                    return true;
                case "9":
                    RemoveRoom();
                    return true;
                case "10":
                    GetRoomFromId();
                    return true;
                case "11":
                    UpdateRoom();
                    return true;
                case "Q": 
                case "q": return false;
                default: return true;
            }

        }

        private static void UpdateRoom() {
            Console.Clear();
            Console.WriteLine("Indlæs room-nr");
            int roomnr = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Indlæs hotelnr");
            int hotelnr = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Indlæs type");
            char type = Console.ReadLine()[0];
            Console.WriteLine("Indlæs pris");
            double pris = Convert.ToDouble(Console.ReadLine());
            Room updatedRoom = new Room(roomnr,type,pris,hotelnr);
            new RoomService().UpdateRoom(updatedRoom,roomnr,hotelnr);
            Console.WriteLine("Det her værelse er nu opdateret.");
        }

        private static void GetRoomFromId() {
            Console.Clear();
            Console.WriteLine("Indlæs room-nr: ");
            int roomnr = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Indlæs hotelnr: ");
            int hotelnr = Convert.ToInt32(Console.ReadLine());
            Room room = new RoomService().GetRoomFromId(roomnr,hotelnr);
            if( room == null ) {
                Console.WriteLine($"Et værelse med dette nummer på hotelnr. {hotelnr} eksisterer ikke.");
            }
            else {
                Console.WriteLine($"RoomNr: {room.RoomNr}, HotelNr: {room.HotelNr}, " +
                    $"Type: {room.Types} & Pris: { room.Pris}.");
            }
        }

        private static void RemoveRoom() {
            Console.Clear();
            Console.WriteLine("Indlæs room-nr");
            int roomnr = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Indlæs hotelnr");
            int hotelnr = Convert.ToInt32(Console.ReadLine());
            Room room = new RoomService().DeleteRoom(roomnr,hotelnr);
            if( room == null ) {
                Console.WriteLine("Et værelse med dette nummer eksisterer ikke.");
            }
            else {
                Console.WriteLine($"RoomNr: { room.RoomNr}, HotelNr: { room.HotelNr}, Type: {room.Types}" +
                    $" & Adresse: { room.Pris}.");
                Console.WriteLine("Dette værelse er fjernet fra listen.");
            }
        }

        private static void CreateRoom() {
            Console.Clear();
            Console.WriteLine("Indlæs room-nr");
            int roomnr = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Indlæs hotelnr");
            int hotelnr = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Indlæs type");
            char type = Console.ReadLine()[0];
            Console.WriteLine("Indlæs pris");
            double pris = Convert.ToDouble(Console.ReadLine());

            RoomService rs = new RoomService();
            bool ok = rs.CreateRoom(hotelnr,new Room(roomnr,type,pris,hotelnr));
            if( ok ) {
                Console.WriteLine("Værelset blev oprettet!");
            }
            else {
                Console.WriteLine("Fejl. Værelset blev ikke oprettet! " +
                    "Der eksisterer et værelse med dette nummer.");
            }
        }

        private static void ListAllRoomsByHotel() {
            Console.Clear();
            RoomService rs = new RoomService();
            Console.WriteLine("Indlæs hotelnr");
            int hotelnr = Convert.ToInt32(Console.ReadLine());
            List<Room> rooms = rs.GetAllRoomsFromHotel(hotelnr);
            if( rooms.Count != 0 ) {
                foreach( Room room in rooms ) {
                    Console.WriteLine($"RoomNr {room.RoomNr}, Type: {room.Types}, " +
                        $"Price: {room.Pris} & HotelNr: {room.HotelNr}.");
                }
            }
            else {
                Console.WriteLine("Der eksisterer ikke nogle værelser på " +
                    "dette hotel eller hotellet eksisterer ikke.");
            }
        }

        private static void ListAllRooms() {
            Console.Clear();
            RoomService rs = new RoomService();
            List<Room> rooms = rs.GetAllRooms();
            foreach( Room room in rooms ) {
                Console.WriteLine($"RoomNr {room.RoomNr}, Type: {room.Types}, Price: {room.Pris} & HotelNr: {room.HotelNr}.");
            }
        }

        private static async void CreateHotelAsync()
        {
            Console.Clear();
            Console.WriteLine("Indlæs hotelnr");
            int hotelnr = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Indlæs hotelnavn");
            string navn = Console.ReadLine();
            Console.WriteLine("Indlæs hotel adresse");
            string adresse = Console.ReadLine();

            //Kalde hotelservice vise resultatet
            HotelServiceAsync hs = new HotelServiceAsync();
            bool ok = await hs.CreateHotelAsync(new Hotel(hotelnr, navn, adresse));
            if (ok) {
                Console.WriteLine("Hotellet blev oprettet!");
            }
            else { 
                Console.WriteLine("Fejl. Hotellet blev ikke oprettet!");
            }
        }

        private static void DoSomething()
        {
            for (int i = 0; i < 100; i++)
            {
                Thread.Sleep(100);
                Console.WriteLine(i + " i GUI i main thread");
            }
        }

        private async static void ShowHotelsAsync()
        {
            Console.Clear();
            HotelServiceAsync hs = new HotelServiceAsync();
            List<Hotel> hotels = await hs.GetAllHotelAsync();
            foreach (Hotel hotel in hotels)
            {
                Console.WriteLine($"HotelNr: {hotel.HotelNr}, Name: {hotel.Navn} & Address: {hotel.Adresse}.");
            }
        }

        private static void UpdateHotel()
        {
            Console.Clear();
            Console.WriteLine("Indlæs hotelnr");
            int hotelnr = Convert.ToInt32(Console.ReadLine());
            Hotel hotel = new HotelService().GetHotelFromId(hotelnr);
            Console.WriteLine($"Det nuværende ID er: {hotel.HotelNr}.");
            int newHotelNr = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine($"Navnet er {hotel.Navn}.");
            string hotelName = Console.ReadLine();
            Console.WriteLine($"Adressen er {hotel.Adresse}.");
            string hotelAddress = Console.ReadLine();
            Hotel updatedHotel = new Hotel(newHotelNr, hotelName, hotelAddress);
            new HotelService().UpdateHotel(updatedHotel, hotelnr);
        }

        private static void FindHotelByNum()
        {
            Console.Clear();
            Console.WriteLine("Indlæs hotelnr");
            int hotelnr = Convert.ToInt32(Console.ReadLine());
            Hotel hotel = new HotelService().GetHotelFromId(hotelnr);
            //Console.WriteLine($"HotelNr {hotel.HotelNr}, Name: {hotel.Navn} & Adresse: {hotel.Adresse}.");
            if (hotel == null)
            {
                Console.WriteLine("Et hotel med dette nummer eksisterer ikke.");
            }
            else
            {
                Console.WriteLine($"HotelNr: { hotel.HotelNr}, Name: { hotel.Navn} &Adresse: { hotel.Adresse}.");
            }
        }

        private static void DeleteHotel()
        {
            Console.Clear();
            Console.WriteLine("Indlæs hotelnr");
            int hotelnr = Convert.ToInt32(Console.ReadLine());
            Hotel hotel = new HotelService().DeleteHotel(hotelnr);
            if (hotel == null)
            {
                Console.WriteLine("Et hotel med dette nummer eksisterer ikke.");
            }
            else
            {
                Console.WriteLine($"HotelNr: { hotel.HotelNr}, Name: { hotel.Navn} & Adresse: { hotel.Adresse}.");
                Console.WriteLine("Dette hotel er fjernet fra listen.");
            }
        }

        private static void CreateHotel()
        {
            //Indlæs data
            Console.Clear();
            Console.WriteLine("Indlæs hotelnr");
            int hotelnr = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Indlæs hotelnavn");
            string navn = Console.ReadLine();
            Console.WriteLine("Indlæs hotel adresse");
            string adresse = Console.ReadLine();

            //Kalde hotelservice vise resultatet
            HotelService hs = new HotelService();
            bool ok = hs.CreateHotel(new Hotel(hotelnr, navn, adresse));
            if (ok)
            {
                Console.WriteLine("Hotellet blev oprettet!");
            }
            else
            {
                Console.WriteLine("Fejl. Hotellet blev ikke oprettet!");
            }
        }



    private static void ShowHotels()
        {
            Console.Clear();
            HotelService hs = new HotelService();
            List<Hotel> hotels = hs.GetAllHotel();
            foreach (Hotel hotel in hotels)
            {
                Console.WriteLine($"HotelNr {hotel.HotelNr}, Name: {hotel.Navn} & Adresse: {hotel.Adresse}.");

            }
        }
    }
}
