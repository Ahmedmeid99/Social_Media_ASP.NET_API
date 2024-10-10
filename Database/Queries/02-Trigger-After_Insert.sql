-- UserReactions
create TRIGGER trg_UpdatePostReactionCount_AfterInsert
ON UserReactions
AFTER INSERT
AS
BEGIN

    DECLARE @PostId INT, @ReactionTypeId INT;

    -- Get the inserted values
    SELECT @PostId = inserted.PostId, @ReactionTypeId = inserted.ReactionTypeId
    FROM inserted;

    -- Update the relevant reaction count
    IF @ReactionTypeId = 1
        UPDATE Posts SET LikesCount = LikesCount + 1 WHERE PostId = @PostId;
    ELSE IF @ReactionTypeId = 2
        UPDATE Posts SET LoveCount = LoveCount + 1 WHERE PostId = @PostId;
    ELSE IF @ReactionTypeId = 3
        UPDATE Posts SET HahaCount = HahaCount + 1 WHERE PostId = @PostId;
	ELSE IF @ReactionTypeId = 4
        UPDATE Posts SET WowCount = WowCount + 1 WHERE PostId = @PostId;
	ELSE IF @ReactionTypeId = 5
        UPDATE Posts SET SadCount = SadCount + 1 WHERE PostId = @PostId;
	ELSE IF @ReactionTypeId = 6
        UPDATE Posts SET AngryCount = AngryCount + 1 WHERE PostId = @PostId;
	ELSE IF @ReactionTypeId = 8
        UPDATE Posts SET DisLikeCount = DisLikeCount + 1 WHERE PostId = @PostId;
    -- Continue with other reaction types as needed
END;
-----------------------------------------------------

-- UserReactions
ALTER TRIGGER trg_UpdatePostReactionCount_AfterDelete
ON UserReactions
AFTER DELETE
AS
BEGIN
    DECLARE @PostId INT, @ReactionTypeId INT;

    -- Get the deleted values
    SELECT @PostId = deleted.PostId, @ReactionTypeId = deleted.ReactionTypeId
    FROM deleted;

    -- Update the relevant reaction count
    IF @ReactionTypeId = 1
        UPDATE Posts SET LikesCount = LikesCount - 1 WHERE PostId = @PostId;
    ELSE IF @ReactionTypeId = 2
        UPDATE Posts SET LoveCount = LoveCount - 1 WHERE PostId = @PostId;
    ELSE IF @ReactionTypeId = 3
        UPDATE Posts SET HahaCount = HahaCount - 1 WHERE PostId = @PostId;
    ELSE IF @ReactionTypeId = 4
        UPDATE Posts SET WowCount = WowCount - 1 WHERE PostId = @PostId;
    ELSE IF @ReactionTypeId = 5
        UPDATE Posts SET SadCount = SadCount - 1 WHERE PostId = @PostId;
    ELSE IF @ReactionTypeId = 6
        UPDATE Posts SET AngryCount = AngryCount - 1 WHERE PostId = @PostId;
    ELSE IF @ReactionTypeId = 8
        UPDATE Posts SET DisLikeCount = DisLikeCount - 1 WHERE PostId = @PostId;
    -- Continue with other reaction types as needed
END;
