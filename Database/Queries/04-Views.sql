-------------------
-- [ Views ]
-------------------

-- Posts
alter View PostsView as
SELECT        Posts.PostID, Posts.UserID, Users.UserName, Users.Email, UserProfilePictures.PictureData, Posts.[Content], Posts.MediaType, Posts.CreatedAt, Posts.LikesCount, Posts.LoveCount, Posts.DisLikeCount, 
                         Posts.HahaCount, Posts.WowCount, Posts.SadCount, Posts.AngryCount
FROM            Posts Left JOIN
                         Users ON Posts.UserID = Users.UserId Left JOIN
                         UserProfilePictures ON Users.UserId = UserProfilePictures.UserId 
------------------------------------------------------------------------------------------

-- Comments
Create View CommentsView as 
SELECT        Comments.CommentId, Comments.PostId, Comments.UserId, Comments.CommentText, UserProfilePictures.PictureData, Users.UserName, Users.Email
FROM            Comments Left JOIN
                         Users ON Comments.UserId = Users.UserId right JOIN
                         UserProfilePictures ON  Users.UserId = UserProfilePictures.UserId

select * from CommentsView
select * from Users

Alter table Users
drop column UserBackgroundPictureId

Alter table Users
drop Constraint  FK__Users__UserBackg__440B1D61

select * from UserProfilePictures
-- Messages

-- User
Alter View UsersView as
SELECT        Users.UserId, Users.UserName, Users.Email,Users.CountryId, UserProfilePictures.PictureData, UserProfilePictures.MediaType
FROM            Users INNER JOIN
                         UserProfilePictures ON Users.UserId = UserProfilePictures.UserId
-- UserReactions

-- UserRelationships
alter view UserRelationshipView as
SELECT        UserRelationships.RelationshipId, UserRelationships.UserId1, UserRelationships.UserId2, UserRelationships.CreatedAt,UserRelationships.RelationshipTypeId ,RelationshipTypes.RelationshipTypeName
FROM            UserRelationships INNER JOIN
                         RelationshipTypes ON UserRelationships.RelationshipTypeId = RelationshipTypes.RelationshipTypeId

-- Find foreign key constraints
SELECT
    fk.name AS FK_Name,
    fk.parent_object_id,
    fk.referenced_object_id
FROM sys.foreign_keys fk
WHERE fk.parent_object_id = OBJECT_ID('Users') 
  AND fk.referenced_object_id = OBJECT_ID('Users');

-- Find unique or other constraints
SELECT
    con.name AS Constraint_Name,
    col.name AS Column_Name
FROM sys.constraints con
JOIN sys.columns col
    ON con.parent_object_id = col.object_id
WHERE col.object_id = OBJECT_ID('Users')
  AND col.name = 'Users';
