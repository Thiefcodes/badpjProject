# badpjProject


-- Threads Table
CREATE TABLE Threads (
    ThreadID INT IDENTITY(1,1) PRIMARY KEY, -- Auto-incrementing Thread ID
    Title NVARCHAR(255) NOT NULL, -- Thread title
    CreatedBy INT NOT NULL, -- User who created the thread
    CreatedAt DATETIME DEFAULT GETDATE(), -- Thread creation date
    IsDeleted BIT DEFAULT 0, -- Soft delete flag
    FOREIGN KEY (CreatedBy) REFERENCES Table(Id) -- Link to 'Table' table, place 'Table' in square brackets since Table is a keyword
);

-- Posts Table
CREATE TABLE Posts (
    PostID INT IDENTITY(1,1) PRIMARY KEY, -- Auto-incrementing Post ID
    ThreadID INT NOT NULL, -- Thread this post belongs to
    Content NVARCHAR(MAX) NOT NULL, -- Post content
    CreatedBy INT NOT NULL, -- User who created the post
    CreatedAt DATETIME DEFAULT GETDATE(), -- Post creation date
    IsDeleted BIT DEFAULT 0, -- Soft delete flag
    FOREIGN KEY (ThreadID) REFERENCES Threads(ThreadID) ON DELETE CASCADE, -- Cascade delete
    FOREIGN KEY (CreatedBy) REFERENCES Table(Id) -- Link to 'Table' table, place 'Table' in square brackets since Table is a keyword
);

Test Data below:
-- Insert test data into Threads
INSERT INTO Threads (Title, CreatedBy, CreatedAt, IsDeleted)
VALUES
('Latest Tech Trends', 1, GETDATE(), 0),
('Gaming Discussions', 2, GETDATE(), 0),
('General Topics', 3, GETDATE(), 0);

-- Insert test data into Posts
INSERT INTO Posts (ThreadID, Content, CreatedBy, CreatedAt, IsDeleted)
VALUES
(1, 'What are your thoughts on AI advancements?', 1, GETDATE(), 0),
(1, 'I think AI has a lot of potential but also risks.', 3, GETDATE(), 0),
(2, 'What is everyone playing this week?', 2, GETDATE(), 0),
(2, 'I just started a new RPG game. It’s amazing!', 1, GETDATE(), 0),
(3, 'Anyone has good book recommendations?', 3, GETDATE(), 0),
(3, 'You should check out "The Art of Computer Programming."', 2, GETDATE(), 0);
