# badpjProject


-- Users Table
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY, -- Auto-incrementing User ID
    Username NVARCHAR(50) NOT NULL UNIQUE, -- Unique Username
    Email NVARCHAR(100) NOT NULL UNIQUE, -- Unique Email
    PasswordHash NVARCHAR(255) NOT NULL, -- Encrypted Password
    CreatedAt DATETIME DEFAULT GETDATE(), -- Account creation date
    LastLogin DATETIME NULL -- Last login timestamp
);

-- Threads Table
CREATE TABLE Threads (
    ThreadID INT IDENTITY(1,1) PRIMARY KEY, -- Auto-incrementing Thread ID
    Title NVARCHAR(255) NOT NULL, -- Thread title
    CreatedBy INT NOT NULL, -- User who created the thread
    CreatedAt DATETIME DEFAULT GETDATE(), -- Thread creation date
    FOREIGN KEY (CreatedBy) REFERENCES Users(UserID) -- Link to Users table
);

-- Posts Table
CREATE TABLE Posts (
    PostID INT IDENTITY(1,1) PRIMARY KEY, -- Auto-incrementing Post ID
    ThreadID INT NOT NULL, -- Thread this post belongs to
    Content NVARCHAR(MAX) NOT NULL, -- Post content
    CreatedBy INT NOT NULL, -- User who created the post
    CreatedAt DATETIME DEFAULT GETDATE(), -- Post creation date
    FOREIGN KEY (ThreadID) REFERENCES Threads(ThreadID) ON DELETE CASCADE, -- Cascade delete
    FOREIGN KEY (CreatedBy) REFERENCES Users(UserID) -- Link to Users table
);

Test Data below:
-- Insert Users
INSERT INTO Users (Username, Email, PasswordHash) 
VALUES 
('JohnDoe', 'john.doe@example.com', 'hashedpassword1'),
('JaneSmith', 'jane.smith@example.com', 'hashedpassword2');

-- Insert Threads
INSERT INTO Threads (Title, CreatedBy) 
VALUES 
('Welcome to the Forums', 1),
('ASP.NET Tips and Tricks', 2);

-- Insert Posts
INSERT INTO Posts (ThreadID, Content, CreatedBy) 
VALUES 
(1, 'This is the first post in the welcome thread!', 1),
(1, 'Thanks for joining the forums!', 2),
(2, 'Here are some great tips for ASP.NET development.', 2),
(2, 'Thanks for sharing these tips!', 1); 