INSERT INTO AspNetUsers (Id, FullName, Email, PhoneNumber, PasswordHash, Role, Address)
VALUES 
('owner-1', 'Mona Adel', 'mona.owner@example.com', '01111222333', 'HASHED_PASSWORD_1', 'Owner', 'Nasr City'),
('owner-2', 'Yasser Fathy', 'yasser.owner@example.com', '01010101010', 'HASHED_PASSWORD_2', 'Owner', 'Maadi'),
('owner-3', 'Sara Ali', 'sara.owner@example.com', '01234567890', 'HASHED_PASSWORD_3', 'Owner', 'Zamalek');

-- إدخال مستأجرين (Tenants)
INSERT INTO AspNetUsers (Id, FullName, Email, PhoneNumber, PasswordHash, Role, Address)
VALUES 
('tenant-1', 'Ahmed Samir', 'ahmed.tenant@example.com', '01099887766', 'HASHED_PASSWORD_4', 'Tenant', 'Heliopolis'),
('tenant-2', 'Nour Hassan', 'nour.tenant@example.com', '01298765432', 'HASHED_PASSWORD_5', 'Tenant', 'October'),
('tenant-3', 'Karim Nabil', 'karim.tenant@example.com', '01555555555', 'HASHED_PASSWORD_6', 'Tenant', 'Shoubra');

-- إدخال سكنات مرتبطة بالمؤجرين
INSERT INTO houses (
    h_Id, Type, Price, Address, Status, FurnishingStatus, TargetCustomers, 
    RentalPeriod, Deposit, Rent, Insurance, Commission, ParticipationLink, 
    RentalType, Photo, InspectionDate, LandlordId
)
VALUES
(1, 'Apartment', 5000.00, '6th October, Giza', 'Available', 'Furnished', 'Students', 
 'Monthly', 1000.00, 5000.00, 200.00, 250.00, 'https://example.com/house1', 
 'New', '/images/house1.jpg', GETDATE(), 'owner-1'),

(2, 'Studio', 3000.00, 'Maadi, Cairo', 'Available', 'Empty', 'Employees', 
 'Monthly', 500.00, 3000.00, 150.00, 200.00, 'https://example.com/house2', 
 'Old', '/images/house2.jpg', GETDATE(), 'owner-2'),

(3, 'Room', 1500.00, 'Zamalek, Cairo', 'Reserved', 'Furnished', 'Students', 
 'Weekly', 200.00, 1500.00, 100.00, 100.00, 'https://example.com/house3', 
 'New', '/images/house3.jpg', GETDATE(), 'owner-3');