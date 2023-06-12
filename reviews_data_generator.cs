//First, going to read all the bookings data then for each bookings' check in date which is older than todays date minus 20 days going to generate a random review.
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

    public class Review
    {
        public int ReviewID { get; set; }
        public int UserID { get; set; }
        public int BookingID { get; set; }
        public double ReviewRating { get; set; }
        public string ReviewComment { get; set; }
        public DateTime ReviewDate { get; set; }

    }
           string filePathUsers = "path-to-Users_data.xlsx";
           string filePathBooking = "path-to-bookings_data.xlsx";

            ExcelReader excelReader = new ExcelReader();
            List<Booking> AllBookings = excelReader.ReadExcelSheet<Booking>(filePathBooking);

            foreach (var row in AllBookings)
            {
                Console.WriteLine(row.BookingID);
                Console.WriteLine();
            }
            Console.ReadKey();

            string[] reviews =
            {
                "I had a wonderful stay. The staff was friendly and helpful, the room was clean and comfortable. The location was convenient, with easy access to nearby attractions.",
                "Amazing! The staff went above and beyond to make our stay memorable. Beautiful rooms, great amenities, and excellent service. Highly recommend!",
                "Outstanding experience! The stay exceeded our expectations in every way. Impeccable cleanliness, friendly staff, and a stunning view. Will definitely return!",
                "Fantastic stay! The location was perfect, right in the heart of the city. Comfortable rooms, delicious breakfast, and attentive staff. Loved every moment!",
                 "Disappointing stay. The room was not as advertised, and the service was lacking. Uncomfortable beds, outdated facilities, and poor communication.",
                "Not recommended. The stay was noisy, and the staff was unfriendly. Room cleanliness was subpar, and amenities were inadequate. Won't be returning.",
                "Below expectations. The stay had maintenance issues, slow Wi-Fi, and unresponsive staff. Lack of attention to detail and limited dining options.",
                "Unforgettable stay! The unique design and artistic ambiance were captivating. Impeccable cleanliness, friendly staff, and a memorable culinary journey.",
                "A hidden gem! Charming stay with a cozy atmosphere. The attention to detail was impressive, and the staff made us feel like family. Will definitely return!",
                "Below par service. We faced multiple issues during our stay, including slow response to requests and unhelpful staff. Room cleanliness and Wi-Fi connectivity were disappointing."
            };
            string fileReview = $"booking_reviews.txt";

            StreamWriter wt = new StreamWriter(fileReview);
            string firstLineReviews = "ReviewID;UserID;BookingID;ReviewRating;ReviewComment;ReviewDate";
            wt.WriteLine(firstLineReviews);

            Random random = new Random();
            DateTime today = DateTime.Today;
            today.AddDays(-20);
            int randomBooking = 0;
            randomBooking = random.Next(1, AllBookings.Count + 1);


            for (int i = 0; i < AllBookings.Count; i++)
            {
                if (AllBookings[i].CheckInDate < today)
                {
                    Review r = new Review();
                    int rev = random.Next(reviews.Length);
                    string review = reviews[rev];
                    r.ReviewID = i;
                    r.BookingID = AllBookings[i].BookingID;
                    r.UserID = AllBookings[i].UserID;
                    r.ReviewComment = review;
                    int somedays = random.Next(1, 20);
                    r.ReviewDate = AllBookings[i].CheckOutDate.AddDays(somedays);
                    if (rev <= 3)
                    {
                        r.ReviewRating = Math.Round(random.NextDouble() * (10 - 8.9) + 8.9,2);
                    }
                    else if (rev >= 4 && rev <= 6)
                    {
                        r.ReviewRating = Math.Round(random.NextDouble() * (8.5 - 2.5) + 2.5,2);
                    }
                    else
                    {
                        r.ReviewRating = Math.Round(random.NextDouble() * (10 - 8.5) + 8.5,2);
                    }
                    wt.WriteLine($"{r.ReviewID};{r.UserID};{r.BookingID};{r.ReviewRating};{r.ReviewComment};{r.ReviewDate.ToString("yyyy/MM/dd")}");
                }

            }
