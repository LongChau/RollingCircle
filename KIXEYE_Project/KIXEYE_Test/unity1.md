# KIXEYE Unity/C# Coding Challenge

## Description

Implement a 2D side-scrolling infinite "roller" game, where a player controls a rolling circle jumping over square obstacles.

## Assumptions

Please state any reasonable assumptions regarding your implementation (around any supporting services or infrastructure) in a README.md

## User Stories:

1. As a player I should load the game and see a start menu including the options "Start", and "Quit"
1. As a player I should be able to select "Start" or "Quit": "Start" have a runner character begin running, while "Quit" will exit the game.
1. As a player I should be able to jump over obstacles.
1. As a player I should be presented with obstacles that are random and varied, but there should also always be a way around them.
1. As a player I should gain 10 points for every obstacle I jump over.
1. As a player I should be able to see my cumulative score on the screen throughout the game.
1. As a player I should see a "Game Over" screen when I collide with an obstacle. The game over screen should show my final score. The game should attempt to send my score to a leaderboard service.
1. As a player I should be able to pause the game, with an option to quit, which would exit the game.


## Leaderboard Service Information

Assume that the leaderboard service doesn't exist yet, but will be built at a future time. It will expose an HTTP REST API as follows:

### Request

`POST /leaderboard`

Body:

```json
{
    "userName": "some-user-name",
    "score": 120
}
``` 

### Response Codes

* 404 - Username not found (user has not registered with the leaderboard service)
* 405 - Invalid Username supplied
* 200 - Ok

## Deliverables

Submit your work in one of the following formats:

1. Tar/Zip/Rar'ed application
2. Link to an open source repository (bitbucket, github, etc)

## Additional Details

* Use of object pooling and coroutines is strongly encouraged.
* The goals of this challenge can be found in the [goals section](goals/developer.md).  It includes how we evaluate submissions.