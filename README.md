CS 4730 / Team Game Final Submission			

Adventuring Party
Computing ID
 Nate Pawlas
pxx2rz
 Hunter McGuire
hhm9vn
Wyley Petrus
wcp2rbf
 




**Game Title:** Offset

**GitHub Repo Link:** https://github.com/cs4730-sp24-uva/game-project-lock-key.git

**Brief Elevator Pitch Describing Game:** This is a puzzle game where the goal is to maneuver two characters simultaneously through multiple levels of puzzles. One set of controls controlling both characters leads to a need for meticulous movement choices to ensure each character can progress through the level.

**Game Instructions:** 
Both characters are controlled with the wasd keys such that a single input will move both simultaneously. The left (a) and right (d) are mirrored depending on what side of the board the character is on.
 
Pressing esc will skip the current level and pressing R will reset the current level. 

_The levels are ordered in a way to introduce new obstacles/interactables such that they need little instruction and can be experimented with, but below is an explanation of how each works._

There are various obstacles/interactables you can find in each level:

**Walls:** Impassable and immovable, if a player attempts to move into a wall it will prevent only that player from moving (not both players)
**Doors/Keys:** A door can only be passed through if one player is occupying the corresponding key the move before the attempt to pass the door is made. If the corresponding key is not occupied, then it will act as a wall.
**Movable Blocks:** These blocks can be pushed around by the players, (even moved across the center line!). If the block is pushed against a wall it can not be moved into that wall, it will respond like an immovable wall in that case.


**Available Content:**
There are 9 levels created that can be played. Hitting play will load the first level and from there you can try and beat as many levels as possible. As you progress through the levels, various mechanics such as doors and movable blocks will be added to increase the complexity.


**Lessons Learned:**

When developing for this project, we chose to create our game grid at runtime. This was so that we could store levels as simple 2d integer arrays in json files. This method had scalability in mind, making it easy for new levels to be created. This scalability came at the cost of new feature velocity. This system we developed had a major flaw: that the logic was handled all by a single object. When multiple people wanted to create new features, they were most likely fiddling with the same lines of code, leading to multiple conflicts. This meant development was slow, which was a problem given the tight deadline of the assignment. It would have been better to prioritize speed of development as opposed to scalability (at least for the beta).

When designing levels, we found that solutions intuitive to some, were not intuitive to everyone. This meant that for introducing new features to the player, we had to focus on creating as straightforward as possible levels to acclimate the player to the new concept, then ramp up the difficulty from there. Finding this balance was much more difficult than we had expected.
