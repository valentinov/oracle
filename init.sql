-- Create the schema
CREATE USER booking_db
IDENTIFIED BY my_password
DEFAULT TABLESPACE users
TEMPORARY TABLESPACE temp;

-- Grant privileges
GRANT CREATE SESSION TO booking_db;
GRANT CREATE TABLE TO booking_db;
GRANT CREATE SEQUENCE TO booking_db;
GRANT CREATE TRIGGER TO booking_db;
GRANT CREATE PROCEDURE TO booking_db;
GRANT UNLIMITED TABLESPACE TO booking_db;

CREATE TABLE booking_db.Amenities (
  AmenityID NUMBER,
  AmenityName VARCHAR2(50) NOT NULL,
  CONSTRAINT PK_Amenities_AmenityID PRIMARY KEY (AmenityID)
);

CREATE TABLE booking_db.ListingTypes (
  ListingTypeID NUMBER,
  ListingTypeName VARCHAR2(100) NOT NULL,
  CONSTRAINT PK_ListingTypes_ListingTypeID PRIMARY KEY (ListingTypeID)
);

CREATE TABLE booking_db.Roles (
  RoleID NUMBER,
  RoleName VARCHAR2(50) NOT NULL,
  CONSTRAINT PK_Roles_RoleID PRIMARY KEY (RoleID),
  CONSTRAINT CK_Roles_RoleName CHECK (RoleName IN ('admin','contributor','customer'))
);

CREATE TABLE booking_db.Users (
  UserID NUMBER,
  RoleID NUMBER NOT NULL,
  FirstName VARCHAR2(100) NOT NULL,
  LastName VARCHAR2(100) NOT NULL,
  DateOfBirth DATE NOT NULL,
  Created DATE NOT NULL,
  Status VARCHAR2(9) NOT NULL,
  Email VARCHAR2(100) NOT NULL,
  Phone VARCHAR2(20) NOT NULL,
  CONSTRAINT PK_Users_UserID PRIMARY KEY (UserID),
  CONSTRAINT CK_Users_Status CHECK (Status IN ('active','inactive','suspended')),
  CONSTRAINT FK_Users_RoleID FOREIGN KEY (RoleID) REFERENCES booking_db.Roles (RoleID)
);

CREATE TABLE booking_db.Listings (
  ListingID NUMBER,
  ListingTypeID NUMBER NOT NULL,
  UserID NUMBER NOT NULL,
  Title VARCHAR2(100) NOT NULL,
  IsActive NUMBER(1) NOT NULL,
  PricePerNight NUMBER(6) NOT NULL,
  MinNight NUMBER(1) NOT NULL,
  MaxGuests NUMBER(4) NOT NULL,
  CONSTRAINT PK_Listings_ListingID PRIMARY KEY (ListingID),
  CONSTRAINT FK_Listings_ListingTypeID FOREIGN KEY (ListingTypeID) REFERENCES booking_db.ListingTypes (ListingTypeID),
  CONSTRAINT FK_Listings_UserID FOREIGN KEY (UserID) REFERENCES booking_db.Users (UserID),
  CONSTRAINT CK_Listings_IsActive CHECK (IsActive IN (0,1)),
  CONSTRAINT CK_Listings_MinNight CHECK (MinNight BETWEEN 1 AND 9)
);

CREATE TABLE booking_db.ListingsDetails (
  ListingID NUMBER NOT NULL,
  Email VARCHAR2(50) NOT NULL,
  Phone VARCHAR2(20) NOT NULL,
  AddressLine1 VARCHAR2(100) NOT NULL,
  City VARCHAR2(50) NOT NULL,
  County VARCHAR2(50) NOT NULL,
  ZipCode VARCHAR2(20) NOT NULL,
  CONSTRAINT FK_Listings_ListingID FOREIGN KEY (ListingID) REFERENCES booking_db.Listings (ListingID)
);

CREATE TABLE booking_db.ListingAmenities (
  ListingID NUMBER NOT NULL,
  AmenityID NUMBER NOT NULL,
  CONSTRAINT PK_ListingAmenities PRIMARY KEY (ListingID, AmenityID),
  CONSTRAINT FK_ListingAmenities_Listing FOREIGN KEY (ListingID) REFERENCES booking_db.Listings (ListingID),
  CONSTRAINT FK_ListingAmenities_Amenity FOREIGN KEY (AmenityID) REFERENCES booking_db.Amenities (AmenityID)
);

CREATE TABLE booking_db.Bookings (
  BookingID NUMBER,
  UserID NUMBER NOT NULL,
  ListingID NUMBER NOT NULL,
  BookingStatus VARCHAR2(15) NOT NULL,
  BookingDate DATE NOT NULL,
  Catering VARCHAR2(10) NOT NULL,
  CheckInDate DATE NOT NULL,
  CheckOutDate DATE NOT NULL,
  NumAdults NUMBER(3) NOT NULL,
  NumChildren NUMBER(3) NOT NULL,
  PaymentMethod VARCHAR2(20) NOT NULL,
  CONSTRAINT PK_Booking_BookingID PRIMARY KEY (BookingID),
  CONSTRAINT FK_Booking_ListingID FOREIGN KEY (ListingID) REFERENCES booking_db.Listings (ListingID),
  CONSTRAINT FK_Booking_UserID FOREIGN KEY (UserID) REFERENCES booking_db.Users (UserID),
  CONSTRAINT CK_Booking_PaymentMethod CHECK (PaymentMethod IN ('cash','SZÉP card','debit card','credit card','bank transfer')),
  CONSTRAINT CK_Booking_Catering CHECK (Catering IN ('none','breakfast','half-board')),
  CONSTRAINT CK_Booking_BookingStatus CHECK (BookingStatus IN ('pending','cancelled','confirmed','completed')),
  CONSTRAINT CK_Bookings_NumAdults CHECK (NumAdults > 0),
  CONSTRAINT CK_Bookings_CheckOutCheckInDate CHECK (CheckOutDate > CheckInDate)
);

CREATE TABLE booking_db.Invoices (
  InvoiceID NUMBER,
  BookingID NUMBER NOT NULL,
  CreatedDate DATE NOT NULL,
  PaymentDate DATE NOT NULL,
  TotalAmount DECIMAL(10,2) NOT NULL,
  CONSTRAINT PK_Invoices_InvoiceID PRIMARY KEY (InvoiceID),
  CONSTRAINT FK_BookingID FOREIGN KEY (BookingID) REFERENCES booking_db.Bookings (BookingID)
);

CREATE TABLE booking_db.Reviews (
  ReviewID NUMBER,
  UserID NUMBER NOT NULL,
  BookingID NUMBER NOT NULL,
  ReviewRating DECIMAL(4,2) NOT NULL,
  ReviewComment VARCHAR2(250),
  ReviewDate DATE NOT NULL,
  CONSTRAINT PK_Reviews_ReviewID PRIMARY KEY (ReviewID),
  CONSTRAINT FK_Reviews_UserID FOREIGN KEY (UserID) REFERENCES booking_db.Users (UserID),
  CONSTRAINT FK_Reviews_BookingID FOREIGN KEY (BookingID) REFERENCES booking_db.Bookings (BookingID)
);
