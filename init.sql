CREATE TABLE ListingAmenities (
  ListingID NUMBER NOT NULL,
  AmenityID NUMBER NOT NULL,
  CONSTRAINT PK_ListingAmenities PRIMARY KEY (ListingID, AmenityID),
  CONSTRAINT FK_ListingAmenities_Listing FOREIGN KEY (ListingID) REFERENCES Listings (ListingID),
  CONSTRAINT FK_ListingAmenities_Amenity FOREIGN KEY (AmenityID) REFERENCES Amenities (AmenityID)
);

CREATE TABLE Amenities (
  AmenityID NUMBER,
  AmenityName VARCHAR2(50) NOT NULL
  CONSTRAINT PK_Amenities_AmenityID PRIMARY KEY (AmenityID)
);

CREATE TABLE Listings (
  ListingID NUMBER,
  ListingTypeID NUMBER,
  Title VARCHAR2(100) NOT NULL,
  PricePerNight NUMBER(5) NOT NULL,
  MaxGuests NUMBER NOT NULL,
  CONSTRAINT PK_Listings_ListingID PRIMARY KEY (ListingID),
  CONSTRAINT FK_Listings_ListingType FOREIGN KEY (ListingTypeID) REFERENCES Listings (ListingTypeID),
);

CREATE TABLE ListingTypes (
  ListingTypeID NUMBER,
  ListingTypeName VARCHAR2(100) NOT NULL,
  CONSTRAINT PK_ListingTypes_ListingTypeID PRIMARY KEY (ListingTypeID)
);

CREATE TABLE ListingsDetails (
  ListingID NUMBER NOT NULL,
  Email VARCHAR2(50) NOT NULL,
  Phone VARCHAR2(20) NOT NULL,
  AddressLine1 VARCHAR2(100) NOT NULL,
  City VARCHAR2(50) NOT NULL,
  County VARCHAR2(50) NOT NULL,
  ZipCode VARCHAR2(20) NOT NULL,
  CONSTRAINT FK_ListingAmenities_Listing FOREIGN KEY (ListingID) REFERENCES Listings (ListingID),
);

CREATE TABLE Users (
  UserID NUMBER,
  FirstName VARCHAR2(100) NOT NULL,
  LastName VARCHAR2(100) NOT NULL,
  DateOfBirth DATE NOT NULL,
  Created DATE NOT NULL,
  Status VARCHAR2(8) NOT NULL,
  Email VARCHAR2(100) NOT NULL,
  Phone VARCHAR2(20) NOT NULL,
  CONSTRAINT PK_Users_UserID PRIMARY KEY (UserID),
  CONSTRAINT CK_Users_Status CHECK (Status IN ('active','inactive','suspended'))
);

CREATE TABLE Roles (
  RoleID NUMBER,
  RoleName VARCHAR2(50) NOT NULL,
  CONSTRAINT PK_Roles_RoleID PRIMARY KEY (RoleID),
  CONSTRAINT CK_Roles_RoleName CHECK (Status IN ('admin','contributor','customer'))
);

CREATE TABLE Bookings (
  BookingID NUMBER,
  UserID NUMBER NOT NULL,
  ListingID NUMBER NOT NULL,
  BookingStatus VARCHAR2(15) NOT NULL,
  BookingDate DATE NOT NULL,
  Catering VARCHAR2(10) NOT NULL,
  CheckInDate DATE NOT NULL,
  CheckOutDate DATE NOT NULL,
  NumAdults NUMBER NOT NULL,
  NumChildren NUMBER NOT NULL,
  PaymentMethod VARCHAR2(20) NOT NULL,
  CONSTRAINT PK_Booking_BookingID PRIMARY KEY (BookingID),
  CONSTRAINT CK_Booking_PaymentMethod CHECK (PaymentMethod IN ('cash','SZÉP card','debit card','credit card','bank transfer')),
  CONSTRAINT CK_Booking_Catering CHECK (Catering IN ('none','breakfast','half-board')),
  CONSTRAINT CK_Booking_BookingStatus CHECK (BookingStatus IN ('pending','cancelled','confirmed','completed')),
  CONSTRAINT CK_Bookings_NumAdults CHECK (NumAdults > 0),
  CONSTRAINT CK_Bookings_CheckOutCheckInDate CHECK (CheckOutDate > CheckInDate)
);

CREATE TABLE Invoices (
  InvoiceID NUMBER,
  BookingID NUMBER,
  CreatedDate DATE NOT NULL,
  PaymentDate DATE NOT NULL,
  TotalAmount DECIMAL(10,2) NOT NULL,
  CONSTRAINT PK_Invoices_InvoiceID PRIMARY KEY (InvoiceID)
);

CREATE TABLE Reviews (
  ReviewID NUMBER,
  UserID NUMBER NOT NULL,
  ListingID NUMBER NOT NULL,
  Rating DECIMAL(4,2) NOT NULL,
  Comment VARCHAR2(250),
  ReviewDate DATE NOT NULL,
  CONSTRAINT PK_Reviews_ReviewID PRIMARY KEY (ReviewID),
  CONSTRAINT FK_UserID FOREIGN KEY (UserID) REFERENCES Users(UserID),
  CONSTRAINT FK_ListingID FOREIGN KEY (ListingID) REFERENCES Listings(ListingID)
);
