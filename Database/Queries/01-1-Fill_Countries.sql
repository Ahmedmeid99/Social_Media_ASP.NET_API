-- Fill Countries

-- Use the destination database
USE Social_MediaDB;
GO

-- Allow explicit values to be inserted into the identity column
SET IDENTITY_INSERT Countries ON;
GO

-- Insert data into the destination table from the source table
INSERT INTO Countries (CountryId, CountryName)  -- List all columns you want to copy
SELECT CountryId, CountryName  -- List all columns you want to copy
FROM E_CommerceDB.dbo.Countries;  -- Use the full path to the source table
GO

-- Disallow explicit values to be inserted into the identity column
SET IDENTITY_INSERT Countries OFF;
GO