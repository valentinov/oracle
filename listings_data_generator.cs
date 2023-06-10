using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookingListsts
{

    class Listing
    {
        public int ListingID { get; set; }
        public int ListingTypeID { get; set; }
        public string UserID { get; set; }
        public string Title { get; set; }
        public int IsActive { get; set; }
        public decimal PricePerNight { get; set; }
        public int MinimumNight { get; set; }
        public int MaximumGuests { get; set; }
    }

    public class ExcelReader
    {
        public List<string> ReadFirstColumn(string filePath)
        {
            List<string> values = new List<string>();

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var dataSet = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true // Assuming the first column contain a header
                        }
                    });

                    var dataTable = dataSet.Tables[0];

                    foreach (DataRow row in dataTable.Rows)
                    {
                      
                            if(row[1].ToString() == "2") // RoleID
                            {
                                string value = row[0].ToString(); // Assuming the first column is at index 0
                                values.Add(value);
                            }           
                    }
                }
            }

            return values;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {

            string filePath = "full-path-to-exportUsers.xlsx";

            ExcelReader excelReader = new ExcelReader();
            List<string> columnValues = excelReader.ReadFirstColumn(filePath);
            // Display the values
            foreach (string value in columnValues)
            {
                Console.WriteLine(value);
            }
            Console.WriteLine(columnValues.Count);
            //Console.ReadKey();

            Random random = new Random();
            DateTime today = DateTime.Today;

            // Set the number of listings to generate
            int numListings = columnValues.Count;
            string fileName = $"listing_data.txt";

            // Set the range of possible values for each field
            string[] titles = { "Cozy","Amuse", "Luxury Villa", "Beach House", "Mountain Retreat", "City Loft", "Moonlight", "Exalted", "Balance", "Mirage", "Refresh", "Modern", "Cosy", "Deluxe", "Grand", "Charming", "Exquisite", "Homely", "Serene", "Rustic" };
            string[] Hoteltitles = { "Hotel", "Residence", "Boutique Suites", "Boutique Residence", "Boutique Hotel", "Lodge", "Resort", "Lodge", "Oasis", "Retreat", "Suites" };
            string[] Othertitles = { "Inn", "Lodge", "House", "Manor", "GuestHouse", "Cottage","Haven","Hideaway","Retreat" };
            int minMinimumNight = 1;
            int maxMinimumNight = 7;

            // Create a list to store the generated listings
            List<Listing> listings = new List<Listing>();
            StreamWriter writer = new StreamWriter(fileName);

            string firstLineListing = "ListingID,ListingTypeID,UserID,Title,IsActive,PricePerNight,MinNight,MaxGuests";
            writer.WriteLine(firstLineListing);

            // Generate the listings
            for (int i = 0; i < numListings; i++)
            {
                Listing l = new Listing();
                int tempi = i + 1;
                //ID
                l.ListingID = tempi;

                //Type
                int randomType = random.Next(1, 101);
                if (randomType <= 30)
                {
                    l.ListingTypeID = 1;
                }
                else if (randomType > 30 && randomType <= 50)
                {
                    l.ListingTypeID = 2;
                }
                else if (randomType > 50 && randomType <= 65)
                {
                    l.ListingTypeID = 3;
                }
                else if (randomType > 65 && randomType <= 90)
                {
                    l.ListingTypeID = 4;
                }
                else if (randomType > 90)
                {
                    l.ListingTypeID = 5;
                }

                //Add UserID from Excel list
                l.UserID = columnValues[i];

                // Generate a random title
                l.Title = titles[random.Next(titles.Length)];
                char space = ' ';
                if (l.ListingTypeID.Equals(1))
                {
                    
                    l.Title += space + Hoteltitles[random.Next(Hoteltitles.Length)];
                }
                else
                {
                    l.Title += space + Othertitles[random.Next(Othertitles.Length)];
                }

                //Generate max nummber of guests
                if (l.ListingTypeID.Equals(1))
                {

                    l.MaximumGuests = random.Next(50, 161);

                }
                else if (l.ListingTypeID.Equals(3))
                {
                    l.MaximumGuests = random.Next(10, 31);
                }
                else
                {
                    l.MaximumGuests = random.Next(2, 15);
                }

                //Generate a random IsActive value
                int randomActive = random.Next(1, 101);
                if (randomActive <= 98)
                {
                    l.IsActive = 1;
                }
                else if (randomActive > 98)
                {
                    l.IsActive = 0;
                }

                //Generate a random price per night
                int randomPrice = random.Next(1, 101);
                int price = 0;
                if (randomPrice <= 33)
                {
                    price = random.Next(5000, 10001);
                }
                else if (randomPrice > 33 && randomPrice<= 66)
                {
                    price = random.Next(10000, 30001);
                }
                else
                {
                    price = random.Next(30001, 65001);
                }

                l.PricePerNight = (int)Math.Round(price / 100.00) * 100;

                // Generate a random minimum night stay
                l.MinimumNight = random.Next(minMinimumNight, maxMinimumNight + 1);

                listings.Add(l);
            }
            for (int i = 0; i < listings.Count; i++)
            {
                writer.WriteLine($"{listings[i].ListingID},{listings[i].ListingTypeID},{listings[i].UserID},{listings[i].Title},{listings[i].IsActive},{listings[i].PricePerNight},{listings[i].MinimumNight},{listings[i].MaximumGuests}");
            }
            writer.Close();

        }
    }
}
