using System;
using System.Collections.Generic;
using System.Linq;

namespace SPMS
{
    public class User
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }

    public enum SlotStatus { Free, Reserved, Occupied }

    public class ParkingSlot
    {
        public int SlotId { get; set; }
        public SlotStatus Status { get; set; }
    }

    public class Reservation
    {
        public int ReservationId { get; set; }
        public required User User { get; set; }
        public required ParkingSlot Slot { get; set; }
        public required string Status { get; set; }
    }

    class Program
    {
        static List<User> users = new List<User>();
        static List<ParkingSlot> slots = new List<ParkingSlot>();
        static List<Reservation> reservations = new List<Reservation>();
        static User? currentUser = null;

        static void Main(string[] args)
        {
            SeedData();
            Console.WriteLine("Welcome to Smart Parking Management System!");

            while (true)
            {
                if (currentUser == null)
                {
                    Login();
                }
                else
                {
                    if (currentUser.Role == "Admin")
                        AdminMenu();
                    else
                        UserMenu();
                }
            }
        }

        static void SeedData()
        {
            users.Add(new User { Username = "user1", Password = "pass1", Role = "User" });
            users.Add(new User { Username = "admin", Password = "admin", Role = "Admin" });
            for (int i = 1; i <= 5; i++)
                slots.Add(new ParkingSlot { SlotId = i, Status = SlotStatus.Free });
        }

        static void Login()
        {
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = Console.ReadLine();

            var user = users.FirstOrDefault(u => u.Username == username && u.Password == password);
            if (user != null)
            {
                currentUser = user;
                Console.WriteLine($"Login successful! Welcome, {currentUser.Username} ({currentUser.Role})");
            }
            else
            {
                Console.WriteLine("Invalid credentials. Try again.");
            }
        }

        static void UserMenu()
        {
            Console.WriteLine("\n--- User Menu ---");
            Console.WriteLine("1. View Parking Slots");
            Console.WriteLine("2. Reserve a Slot");
            Console.WriteLine("3. Cancel Reservation");
            Console.WriteLine("4. View My Reservations");
            Console.WriteLine("5. Logout");
            Console.Write("Select option: ");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1": ViewSlots(); break;
                case "2": ReserveSlot(); break;
                case "3": CancelReservation(); break;
                case "4": ViewMyReservations(); break;
                case "5": currentUser = null; break;
                default: Console.WriteLine("Invalid option."); break;
            }
        }

        static void AdminMenu()
        {
            Console.WriteLine("\n--- Admin Menu ---");
            Console.WriteLine("1. View All Reservations");
            Console.WriteLine("2. Reset All Slot Statuses");
            Console.WriteLine("3. Logout");
            Console.Write("Select option: ");
            string input = Console.ReadLine();
            switch (input)
            {
                case "1": ViewAllReservations(); break;
                case "2": ResetSlots(); break;
                case "3": currentUser = null; break;
                default: Console.WriteLine("Invalid option."); break;
            }
        }

        static void ViewSlots()
        {
            Console.WriteLine("\nSlotID\tStatus");
            foreach (var slot in slots)
                Console.WriteLine($"{slot.SlotId}\t{slot.Status}");
        }

        static void ReserveSlot()
        {
            ViewSlots();
            Console.Write("Enter SlotID to reserve: ");
            if (int.TryParse(Console.ReadLine(), out int slotId))
            {
                var slot = slots.FirstOrDefault(s => s.SlotId == slotId);
                if (slot == null)
                {
                    Console.WriteLine("Slot not found.");
                }
                else if (slot.Status != SlotStatus.Free)
                {
                    Console.WriteLine("Slot is not available.");
                }
                else
                {
                    slot.Status = SlotStatus.Reserved;
                    var reservation = new Reservation
                    {
                        ReservationId = reservations.Count + 1,
                        User = currentUser,
                        Slot = slot,
                        Status = "Active"
                    };
                    reservations.Add(reservation);
                    Console.WriteLine("Reservation successful!");
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }

        static void CancelReservation()
        {
            var myReservations = reservations.Where(r => r.User == currentUser && r.Status == "Active").ToList();
            if (!myReservations.Any())
            {
                Console.WriteLine("No active reservations found.");
                return;
            }
            foreach (var r in myReservations)
                Console.WriteLine($"ReservationID: {r.ReservationId}, SlotID: {r.Slot.SlotId}");

            Console.Write("Enter ReservationID to cancel: ");
            if (int.TryParse(Console.ReadLine(), out int resId))
            {
                var reservation = myReservations.FirstOrDefault(r => r.ReservationId == resId);
                if (reservation != null)
                {
                    reservation.Status = "Cancelled";
                    reservation.Slot.Status = SlotStatus.Free;
                    Console.WriteLine("Reservation cancelled.");
                }
                else
                {
                    Console.WriteLine("Reservation not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid input.");
            }
        }

        static void ViewMyReservations()
        {
            var myReservations = reservations.Where(r => r.User == currentUser).ToList();
            if (!myReservations.Any())
            {
                Console.WriteLine("No reservations found.");
                return;
            }
            foreach (var r in myReservations)
                Console.WriteLine($"ReservationID: {r.ReservationId}, SlotID: {r.Slot.SlotId}, Status: {r.Status}");
        }

        static void ViewAllReservations()
        {
            if (!reservations.Any())
            {
                Console.WriteLine("No reservations found.");
                return;
            }
            foreach (var r in reservations)
                Console.WriteLine($"ReservationID: {r.ReservationId}, User: {r.User.Username}, SlotID: {r.Slot.SlotId}, Status: {r.Status}");
        }

        static void ResetSlots()
        {
            foreach (var slot in slots)
                slot.Status = SlotStatus.Free;
            foreach (var res in reservations)
                if (res.Status == "Active")
                    res.Status = "Cancelled";
            Console.WriteLine("All slots reset to Free and active reservations cancelled.");
        }
    }
}