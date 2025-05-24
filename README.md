### **Tournament API**

Gabe called. He was planning to have a bunch of CS tournaments for his thousand closest friends, and needed a web service that could keep track of the players and the tournaments. “RESTful!” was apparently important to work with Valve's Blackberries.

The requirements we are aware of are:
- Players have names and ages.
- Tournaments have a name and zero or more players. A tournament can have sub-tournaments, and sub-tournaments can have sub-tournaments (and so on, for up to five levels in total).
- Players should be able to register in tournaments. In order for a player to be able to register in a tournament that is a sub-tournament of another tournament, he must already be registered in the "parent tournament".
- You should be able to create, delete, list and retrieve details for both players and tournaments.
- The details for a tournament need to show all sub-tournaments.

This project is developed using below tools and software libraries

1. Visual studio
2. Asp.Net Core
3. C#
4. (.Net) 8.0
5. Postman

 ## Table Creation
### Database Name : TournamentDB
- Queries for table Creation
- Tournament, Players, TournamentPlayer, SubTournament
  
  ```
  CREATE TABLE Tournament (
    Id INT PRIMARY KEY IDENTITY(1,1),
    TournamentName NVARCHAR(255) NOT NULL,
    NumberOfPlayers INT NOT NULL,
    ParentTournamentId INT NULL,

    CONSTRAINT FK_Tournament_Parent
        FOREIGN KEY (ParentTournamentId)
        REFERENCES Tournament(Id)
        ON DELETE NO ACTION
   ); 
  
  CREATE TABLE Players (
    Id INT PRIMARY KEY IDENTITY(1,1),
    PlayerName NVARCHAR(100) NOT NULL,
    Age INT NOT NULL
   );
    
  CREATE TABLE TournamentPlayer (
    TournamentId INT,
    PlayerId INT,
    PRIMARY KEY (TournamentId, PlayerId),
    CONSTRAINT FK_Tournament FOREIGN KEY (TournamentId) REFERENCES Tournament(Id),
    CONSTRAINT FK_Player FOREIGN KEY (PlayerId) REFERENCES Player(Id)
  );

  CREATE TABLE SubTournament (
    ParentTournamentId INT,
    SubTournamentId INT,
    PRIMARY KEY (ParentTournamentId, SubTournamentId),
    CONSTRAINT FK_SubTournament FOREIGN KEY (ParentTournamentId) REFERENCES Tournament(Id),
    CONSTRAINT FK_Sub FOREIGN KEY (SubTournamentId) REFERENCES Tournament(Id)
  );
```
  ## Improvements needed:
1. Unit test(Xunit) has be added to test fake data
2. The same project can be implemented in the clean architecture
3. Service has to created to do the SQL query operations instead of doing in the controllers
  
   
