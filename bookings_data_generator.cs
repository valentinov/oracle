  public class Booking
    {
        public int BookingID { get; set; }
        public int UserID { get; set; }
        public int ListingID { get; set; }
        public string BookingStatus { get; set; }
        public DateTime BookingDate { get; set; }
        public string Catering { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int NumAdults { get; set; }
        public int NumChildren { get; set; }
        public string PaymentMethod { get; set; }
    }

    public class Listing
    {
        public int ListingID { get; set; }
        public int ListingTypeID { get; set; }
        public int UserID { get; set; }
        public string Title { get; set; }
        public int IsActive { get; set; }
        public int PricePerNight { get; set; }
        public int MinimumNight { get; set; }
        public int MaximumGuests { get; set; }
    }

    public class User
    {
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DATEOFBIRTH { get; set; }
        public string Created { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
  
  
            DateTime startdate = new DateTime(2020, 1, 1);
            DateTime enddate = new DateTime(2024, 03, 31);

            string filename = $"booking_data.txt";
            StreamWriter writer = new StreamWriter(filename);
            string firstlinebooking = "bookingid,userid,listingid,bookingstatus,bookingdate,catering,checkindate,checkoutdate,numadults,numchildren,paymentmethod";
            writer.WriteLine(firstlinebooking);


            //booking generator
            Random random = new Random();
            DateTime today = DateTime.Today;
            //keep track of the last checkout dates for each users
            Dictionary<int, DateTime> lastcheckoutdates = new Dictionary<int, DateTime>();

            //generate 200.000 bookings
            for (int i = 1; i < 200001; i++)
            {

                //find a random user and a listing
                int randombookinguser = 0;
                int randomlisting = 0;

                do
                {
                    randomlisting = random.Next(1, AllListingdata.Count);
                } while (AllListingdata[randomlisting].IsActive != 0); //has to be active listing

                do
                {
                    randombookinguser = random.Next(100, AllUSers.Count);
                } 
                while (AllUSers[randombookinguser].RoleID != 1 && AllUSers[randombookinguser].RoleID != 2); //user has to be general user


                User u = AllUSers[randombookinguser];
                //Console.WriteLine(u.firstname);
                //Console.WriteLine(u.roleid);

                Listing l = AllListingdata[randomlisting];

                //generate the dates
                //date of booking
                DateTime bookingdate = GetRandomDate(startdate, today);
                DateTime checkindate;
                DateTime checkoutdate;
                do
                {
                    checkindate = GetRandomDate(startdate, enddate);
                    //generate some random nights based on the selected Listings minimum night field
                    int somenights = random.Next(l.MinimumNight, l.MinimumNight + 10);
                    checkoutdate = GetRandomDate(checkindate.AddDays(l.MinimumNight), checkindate.AddDays(somenights));

                }
                while (lastcheckoutdates.ContainsKey(randombookinguser) && checkoutdate < lastcheckoutdates[randombookinguser].AddDays(10));


                lastcheckoutdates[randombookinguser] = checkoutdate;

                string bookingstatus = "";

                //generate the status of the bookimg
                if (checkoutdate < today)
                {
                    string[] bookingstatuses = new string[] { "cancelled", "completed" };
                    int randomnum = random.Next(1, 101);
                    if (randomnum <= 90)
                    {
                        bookingstatus = "completed";
                    }
                    else
                    {
                        bookingstatus = "cancelled";
                    }
                    //bookingstatus = bookingstatuses[random.next(bookingstatuses.length)];


                }
                //if checkindate greater than today it has to be either confirmed or cancelled but not comppleted
                else if (checkindate > today && bookingdate < today.AddDays(-4))
                {

                    int randomnum = random.Next(1, 101);
                    if (randomnum <= 5)
                    {
                        bookingstatus = "cancelled";
                    }
                    else if (randomnum > 5 && randomnum <= 101)
                    {
                        bookingstatus = "confirmed";
                    }

                }
                else
                {

                    bookingstatus = "confirmed";
                }

                //catering
                string[] cateringoptions = new string[] { "none", "breakfast", "half-board" };
                string catering = cateringoptions[random.Next(cateringoptions.Length)];


                int numadults = 0;
                //max guests
                int maxguest = l.MaximumGuests;
                int randomnumadults = random.Next(1, 101);

                if (randomnumadults <= 5)
                {
                    numadults = 1;
                }
                else if (randomnumadults > 5 && randomnumadults <= 85)
                {
                    numadults = 2;
                }
                else
                {
                    numadults = random.Next(2, maxguest + 1);
                }

                int numchildren = 0;

                int randomnumchild = random.Next(1, 101);

                if (randomnumchild <= 25)
                {
                    numchildren = 1;
                }
                else if (randomnumchild > 25 && randomnumchild <= 80)
                {
                    numchildren = 0;
                }
                else
                {
                    numchildren = random.Next(2, 5);
                }

                string[] paymentmethods = new string[] { "cash", "szép card", "debit card", "credit card", "bank transfer" };

                string paymentmethod = paymentmethods[random.Next(paymentmethods.Length)];

                writer.WriteLine($"{i},{randombookinguser},{randomlisting},{bookingstatus},{bookingdate.ToString("yyyy/mm/dd")},{catering},{checkindate.ToString("yyyy/mm/dd")},{checkoutdate.ToString("yyyy/mm/dd")},{numadults},{numchildren},{paymentmethod}");

            }

            writer.Close();
            
            //helper function for random date
            static DateTime GetRandomDate(DateTime startDate, DateTime endDate)
            {
                Random r = new Random();
                TimeSpan timeSpan = endDate - startDate;
                int totalDays = timeSpan.Days;
                int randomDays = r.Next(0, totalDays);
                DateTime randoMDate = startDate.AddDays(randomDays);
                return randoMDate;
                //+ randomSpan;
            }
