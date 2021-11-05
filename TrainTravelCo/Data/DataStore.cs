using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrainTravelCo.Models;

namespace TrainTravelCo.Data
{
    public class DataStore
    {
        private static DataStore _instance;
        private string pathString = @"C:\Users\Mathias\Source\Repos\TrainTravel 2.0\TrainTravelCo\Data\Traindata";

        public static DataStore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DataStore();
                }
                return _instance;
            }
        }

        private List<Train> _trains;
        private List<Trip> _trips;

      

        private DataStore()
        {
            Directory.CreateDirectory(pathString);

            _trains = new List<Train>();
            _trips = new List<Trip>();

            Train train1 = new Train() { MaxSeats = 10, RegNumber = "ABC" };
            Train train2 = new Train() { MaxSeats = 2, RegNumber = "DEF" };
            Train train3 = new Train(100);
            Train train4 = new Train(200);
            SaveTrain(train1);
            SaveTrain(train2);
            SaveTrain(train3);
            SaveTrain(train4);
            Trip trip1 = new Trip()
            {
                From = "Lund",
                To = "Stockholm",
                Time = "2021-01-01",
                Train = train1
            };
            Trip trip2 = new Trip()
            {
                From = "Arvika",
                To = "Goteborg",
                Time = "2021-11-04",
                Train = train1
            };
            Trip trip3 = new Trip()
            {
                From = "Malmo",
                To = "Motala",
                Time = "2021-01-07",
                Train = train2
            };
            SaveTrip(trip1);
            SaveTrip(trip2);
            SaveTrip(trip3);
        }

        public List<Train> ListTrains()
        {
            string[] listofFiles = (Directory.GetFiles(pathString));
            for (int i = 0; i < listofFiles.Length; i++)
            {
                listofFiles[i] = Path.GetFileName(listofFiles[i]);
            }
            foreach (var files in listofFiles)
            {
                if (files.StartsWith("train_"))
                {
                    FileStream sf = File.OpenRead($"{pathString}\\{files}");
                    using (StreamReader sw = new StreamReader(sf))
                    {
                        sw.ReadToEnd();
                    }
                }
            }

            return _trains;
        }

        public void SaveTrain(Train train)
        {
            _trains.Add(train);
            var trainIdToSave = train.Id;
            var regToSave = train.RegNumber;
            var seatsToSave = train.MaxSeats;

            using (FileStream fs = File.Create($"{pathString}\\train_{trainIdToSave}.txt"))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine($"{trainIdToSave}\n{regToSave}\n{seatsToSave}");
                }
            }
        }

        public List<Trip> ListTrips()
        {
            return _trips;
        }

        public void SaveTrip(Trip trip)
        {
            _trips.Add(trip);
            var tripIDToSave = trip.Id;
            var trainFrom = trip.From;
            var trainTo = trip.To;
            var tripTime = trip.Time;
            var TrainId = trip.Train.Id;
            var tripBookings = trip.Bookings;
            

            using (FileStream fs = File.Create($"{pathString}\\trip_{tripIDToSave}.txt"))
            {
                using (StreamWriter sr = new StreamWriter(fs))
            {
                sr.WriteLine($"{TrainId}\n{trainFrom}\n{trainTo}\n{tripTime}\n{tripBookings.Count}");

                    for (int i = 0; i < tripBookings.Count; i++)
                   
                {
                    sr.WriteLine($"{tripBookings[i].Customer.Name}\n");
                }
            }
            }



        }
    }
}