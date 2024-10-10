

-- [ Triggers ]
s
---------------------------
-- After Update Date 
---------------------------


-- UserProfilePictures
Alter TRIGGER trg_UpdateUserProfilePictures
ON UserProfilePictures
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE UserProfilePictures
    SET UpdatedAt = GETDATE()
    FROM UserProfilePictures
    INNER JOIN inserted ON UserProfilePictures.UserProfilePictureId = inserted.UserProfilePictureId;
END;
--------------------------------------------------------------------

--

-- UserBackgroundPictures
Alter TRIGGER trg_UpdateUserBackgroundPicture
ON UserBackgroundPictures
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE UserProfilePictures
    SET UpdatedAt = GETDATE()
    FROM UserBackgroundPictures
    INNER JOIN inserted ON UserBackgroundPictures.UserBackgroundPictureId = inserted.UserBackgroundPictureId;
END;
--------------------------------------------------------------------


-- User
Alter TRIGGER trg_UpdateUser
ON Users
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Users
    SET UpdatedAt = GETDATE()
    FROM Users
    INNER JOIN inserted ON Users.UserId = inserted.UserId;
END;
--------------------------------------------------------------------

-- Posts
Alter TRIGGER trg_UpdatePost
ON Posts
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Posts
    SET UpdatedAt = GETDATE()
    FROM Posts
    INNER JOIN inserted ON Posts.PostID = inserted.PostID;
END;
--------------------------------------------------------------------


-- Comments
Alter TRIGGER trg_UpdateComment
ON Comments
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Comments
    SET UpdatedAt = GETDATE()
    FROM Comments
    INNER JOIN inserted ON Comments.CommentId = inserted.CommentId;
END;
--------------------------------------------------------------------

select * from ReactionTypes

CREATE TRIGGER trg_UpdatePostReactionCount_AfterUpdate
ON UserReactions
AFTER UPDATE
AS
BEGIN
    DECLARE @PostId INT, @OldReactionTypeId INT, @NewReactionTypeId INT;

    -- Get the old and new values
    SELECT @PostId = deleted.PostId, @OldReactionTypeId = deleted.ReactionTypeId, @NewReactionTypeId = inserted.ReactionTypeId
    FROM inserted
    INNER JOIN deleted ON inserted.ReactionId = deleted.ReactionId;

    -- Decrement the old reaction count -- @OldReactionTypeId
    IF  @OldReactionTypeId = 1
        UPDATE Posts SET LikesCount = LikesCount - 1 WHERE PostId = @PostId;
    ELSE IF  @OldReactionTypeId = 2
        UPDATE Posts SET LoveCount = LoveCount - 1 WHERE PostId = @PostId;
    ELSE IF  @OldReactionTypeId = 3
        UPDATE Posts SET HahaCount = HahaCount - 1 WHERE PostId = @PostId;
	ELSE IF  @OldReactionTypeId = 4
        UPDATE Posts SET WowCount = WowCount - 1 WHERE PostId = @PostId;
	ELSE IF  @OldReactionTypeId = 5
        UPDATE Posts SET SadCount = SadCount - 1 WHERE PostId = @PostId;
	ELSE IF  @OldReactionTypeId = 6
        UPDATE Posts SET AngryCount = AngryCount - 1 WHERE PostId = @PostId;
	ELSE IF  @OldReactionTypeId = 8
        UPDATE Posts SET DisLikeCount = DisLikeCount - 1 WHERE PostId = @PostId;

    -- Increment the new reaction count -- @NewReactionTypeId
    IF @NewReactionTypeId = 1
        UPDATE Posts SET LikesCount = LikesCount + 1 WHERE PostId = @PostId;
    ELSE IF @NewReactionTypeId = 2
        UPDATE Posts SET LoveCount = LoveCount + 1 WHERE PostId = @PostId;
    ELSE IF @NewReactionTypeId = 3
        UPDATE Posts SET HahaCount = HahaCount + 1 WHERE PostId = @PostId;
	ELSE IF @NewReactionTypeId = 4
        UPDATE Posts SET WowCount = WowCount + 1 WHERE PostId = @PostId;
	ELSE IF @NewReactionTypeId = 5
        UPDATE Posts SET SadCount = SadCount + 1 WHERE PostId = @PostId;
	ELSE IF @NewReactionTypeId = 6
        UPDATE Posts SET AngryCount = AngryCount + 1 WHERE PostId = @PostId;
	ELSE IF @NewReactionTypeId = 8
        UPDATE Posts SET DisLikeCount = DisLikeCount + 1 WHERE PostId = @PostId;
END;

--------------------------------------------------------------------