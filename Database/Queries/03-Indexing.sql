--CREATE INDEX idx_Users_Email ON Users(Email);

CREATE INDEX idx_Users_UserName ON Users(UserName);

CREATE INDEX idx_Posts_UserID ON Posts(UserID);

--CREATE INDEX idx_Posts_CreatedAt ON Posts(CreatedAt);

CREATE INDEX idx_UserRelationships_UserId1_UserId2 ON UserRelationships(UserId1, UserId2);

CREATE INDEX idx_Messages_SenderId ON Messages(SenderId);

CREATE INDEX idx_Messages_ReceiverId ON Messages(ReceiverId);

CREATE INDEX idx_Comments_PostId ON Comments(PostId);

CREATE INDEX idx_Comments_UserId ON Comments(UserId);
