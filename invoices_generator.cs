            string fileInvoices = $"booking_invoices.txt";
            //read all the bookings data
            List<Booking> AllBookings = excelReader.ReadExcelSheet<Booking>(filePathBooking);
            StreamWriter wtw = new StreamWriter(fileInvoices);
            string firstLineInvoices = "InvoiceID,BookingID,CreatedDate,PaymentDate,TotalAmount";
            wtw.WriteLine(firstLineInvoices);
            int ct = 1;
            for (int i = 0; i < AllBookings.Count; i++)
            {
                Booking bk = new Booking();
                   bk = AllBookings[i];
                if (bk.CheckOutDate < today)
                {
                    int somedays = random.Next(-2, 6);
                    int somedays2 = random.Next(-2, 12);
                    Invoice iv = new Invoice();
                    Listing l = new Listing();
                       l = AllListingdata[bk.ListingID-1];
                    iv.InvoiceID = ct;
                    iv.BookingID = bk.BookingID;
                    iv.CreatedDate = bk.CheckInDate.AddDays(somedays);
                    iv.PaymentDate = bk.CheckOutDate.AddDays(somedays2);
                    int totalNights = (bk.CheckOutDate - bk.CheckInDate).Days;
                    int childPrice = Convert.ToInt32(l.PricePerNight * 0.5);
                    iv.TotalAmount = (bk.NumAdults * totalNights * l.PricePerNight) + (bk.NumChildren * totalNights * childPrice);
                    wtw.WriteLine($"{iv.InvoiceID},{iv.BookingID},{iv.CreatedDate.ToString("yyyy/MM/dd")},{iv.PaymentDate.ToString("yyyy/MM/dd")},{iv.TotalAmount}");
                    ct++;
                }
               
            }
            wtw.Close();
