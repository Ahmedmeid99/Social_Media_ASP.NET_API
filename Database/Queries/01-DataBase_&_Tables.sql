---------------------------------------
-- Create Social Media DataBase Design
---------------------------------------

-- [ DataBase ]

Create DataBase Social_MediaDB

-- [ Tables ]

-- [1] Countries
Create Table Countries
(
	CountryId int identity(1,1) not null,
	CountryName nvarchar(250) not null

	primary key(CountryId)
);
alter table UserBackgroundPictures  
add  MediaType NVARCHAR(250) DEFAULT 'None'--
-- [2] UserProfilePictures
Create Table UserProfilePictures
	(
		UserProfilePictureId int identity(1,1) NOT null,
		UserId INT REFERENCES Users(UserId) NOT NULL,
		MediaType NVARCHAR(250) DEFAULT 'None',
		PictureData VarBinary(MAX) NOT NULL,
		CreatedAt DateTime DEFAULT GetDate() NOT NULL,
		UpdatedAt DateTime DEFAULT GETDATE() NOT NULL, -- Trigger After Update
		primary key(UserProfilePictureId)

	)

-- [3] UserProfilePictures
Create Table UserBackgroundPictures
	(
		UserBackgroundPictureId int identity(1,1) not null,
		UserId INT REFERENCES Users(UserId) NOT NULL,
		PictureData VarBinary(MAX) NOT NULL,
		MediaType NVARCHAR(250) DEFAULT 'None',
		CreatedAt DateTime DEFAULT GetDate() NOT NULL,
		UpdatedAt DateTime DEFAULT GetDate() NOT NULL, -- Trigger After Update
		primary key(UserBackgroundPictureId)

	)

-- [4] Users
CREATE TABLE Users
(
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    UserName NVARCHAR(250) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(250) UNIQUE NOT NULL, -- 
    Gender CHAR(1) NULL, --'M' , 'F'
    Email NVARCHAR(250) UNIQUE NOT NULL,
    DateOfBirth DATETIME NOT NULL,
    Phone VARCHAR(250) NOT NULL,
    Address NVARCHAR(500) NULL,
    CountryId INT REFERENCES Countries(CountryId)  NOT NULL,
    UserProfilePictureId INT REFERENCES UserProfilePictures(UserProfilePictureId) NULL, -- delete
    UserBackgroundPictureId INT REFERENCES UserBackgroundPictures(UserBackgroundPictureId) NULL, -- delete
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedAt DATETIME DEFAULT GETDATE() -- Trigger After Update
);

-- [5] RelationshipTypes
CREATE TABLE RelationshipTypes (
    RelationshipTypeId INT PRIMARY KEY IDENTITY(1,1),
    RelationshipTypeName VARCHAR(50) NOT NULL UNIQUE
);
  

-- [6] UserRelationships
CREATE TABLE UserRelationships
(
    RelationshipId INT IDENTITY(1,1) PRIMARY KEY,
    UserId1 INT NOT NULL,
    UserId2 INT NOT NULL,
    RelationshipTypeId int REFERENCES RelationshipTypes(RelationshipTypeId),
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY(UserId1) REFERENCES Users(UserId),
    FOREIGN KEY(UserId2) REFERENCES Users(UserId)
);


-- [7] Posts
CREATE TABLE Posts
(
    PostID INT IDENTITY(1,1) PRIMARY KEY, -- 
    UserID INT NOT NULL, -- 
    Content NVARCHAR(MAX), -- 
    MediaType NVARCHAR(10) DEFAULT 'None', --  
    MediaData VarBinary(MAX) Null, --    
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP, -- 
    UpdatedAt DATETIME DEFAULT NULL, -- 
	LikesCount Int default 0,
	LoveCount Int default 0,
	DisLikeCount Int default 0,
    FOREIGN KEY (UserID) REFERENCES Users(UserID),
    CONSTRAINT CHK_MediaType CHECK (MediaType IN ('image/jpeg', 'image/png', 'video/mp4', 'application/pdf','None')) -- 
);

-- Alter Table Posts Add MediaData VarBinary(MAX) Null
-- [8] Comments
CREATE TABLE Comments
(
    CommentId INT IDENTITY(1,1) PRIMARY KEY,
    PostId INT NOT NULL,
    UserId INT NOT NULL,
    CommentText NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
	UpdatedAt DATETIME DEFAULT GETDATE() -- Trigger After Update
    FOREIGN KEY(PostId) REFERENCES Posts(PostId),
    FOREIGN KEY(UserId) REFERENCES Users(UserId)
);

-- [9]
CREATE TABLE ReactionTypes (
    ReactionTypeId INT IDENTITY(1,1) PRIMARY KEY,
    ReactionName NVARCHAR(50) NOT NULL UNIQUE
);

-- [10]
CREATE TABLE UserReactions (
    ReactionId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT REFERENCES Users(UserId) NOT NULL,
    PostId INT REFERENCES Posts(PostId) NOT NULL,
    ReactionTypeId INT REFERENCES ReactionTypes(ReactionTypeId) NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);

-- [11] Messages
CREATE TABLE Messages
(
    MessageId INT IDENTITY(1,1) PRIMARY KEY,
    SenderId INT NOT NULL,
    ReceiverId INT NOT NULL,
    MessageText NVARCHAR(MAX) NOT NULL,
    SentAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY(SenderId) REFERENCES Users(UserId),
    FOREIGN KEY(ReceiverId) REFERENCES Users(UserId)
);



-- [ Views ]

-- [ Stored Procedural ]

-- [ Functions ]

-- [ Triggers ] After (Done) , Insted ...

-- [ indexing ] (Done)


SELECT * 
FROM INFORMATION_SCHEMA.CONSTRAINT_COLUMN_USAGE 
WHERE TABLE_NAME = 'Posts'; --Posts UserProfilePictures UserBackgroundPictures
 

SELECT * 
FROM sys.check_constraints 
WHERE parent_object_id = OBJECT_ID('Posts');

ALTER TABLE Posts
DROP CONSTRAINT CHK_MediaType;

-- Step 1: Set the default value for the MediaType column
ALTER TABLE Posts
ADD CONSTRAINT DF_Posts_MediaType DEFAULT 'None' FOR MediaType;

-- Step 2: Add the check constraint to restrict the allowed values
ALTER TABLE posts
ADD CONSTRAINT CHK_MediaType CHECK (MediaType IN ('image/jpeg', 'image/png', 'video/mp4', 'application/pdf', 'None'));

ALTER TABLE UserBackgroundPictures
ADD CONSTRAINT CHK_User_bg_MediaType CHECK (MediaType IN ('image/jpeg', 'image/png', 'None'));

ALTER TABLE UserProfilePictures
ADD CONSTRAINT CHK_User_profile_MediaType CHECK (MediaType IN ('image/jpeg', 'image/png', 'None'));

-- 
update Posts set AngryCount = 0

-- ADD CONSTRAINT DEF_MediaType Default 'None' ;

SELECT 
    name AS ConstraintName
FROM 
    sys.default_constraints
WHERE 
    parent_object_id = OBJECT_ID('Posts') AND
    WowCount = (
        SELECT WowCount 
        FROM sys.columns 
        WHERE object_id = OBJECT_ID('Posts') AND name = 'HahaCount'
    );

use Social_MediaDB;


select * from ReactionTypes order by ReactionTypeId;

select * from Comments;

-------------------------------------------------------------

INSERT INTO RelationshipTypes (RelationshipTypeName)
VALUES ('Friend'), ('Follower'), ('Blocked'), ('Family'), ('Colleague'), ('Acquaintance'), ('Requested'), ('Muted');

-------------------------------------------------------------

INSERT INTO ReactionTypes (ReactionName) VALUES 
('Like'),
('Love'),
('Haha'),
('Wow'),
('Sad'),
('Angry'),
('Care'),
('Dislike'),
('Celebrate'),
('Support'),
('Question'),
('Insightful'),
('Excited'),
('Appreciate');




----------------------------------------
select * from RelationshipTypes
select * from UserRelationships


use Social_MediaDB
select * from Users
delete UserReactions where PostID > 4021
delete Comments where PostID > 4021

delete Posts where PostID > 4021
-- Check column length
SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'Posts' AND COLUMN_NAME = 'MediaType';

ALTER TABLE Posts
ALTER COLUMN MediaType VARCHAR(255);

ALTER TABLE Posts
DROP CONSTRAINT DF__Posts__MediaType__4D94879B;