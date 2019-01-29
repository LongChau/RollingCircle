# RollingCircle
An endless running game

Hi,
I want to make it funny rolling game. So I add some image and sound effect for it. Make the game more interest.
I also make an extra "highscore". To save user's highest score for future usage. 

The server I use for this game is GameSparks. You can find more details about in here: https://gamesparks.com/
GameSparks's server is not run in live-stage because I'm not contact to GameSparks yet, and the server is still in preview build.
However it contains full feature.

Account with permission:
Acc: gamejamvictor@gmail.com
Password: GameJam_2018

More about code in GameSparks:
1. Configuration => Cloud code.
2. My code in: 
	Requests: RegistrationRequest, 
	Responses: AuthenticationResponse,
	System: GS_PLAYER_CONNECT,
	Modules: GPacketType, GConst.
	
Note: 
*** To add/modify sound effect:
1. Go to ESoundEffect.
2. Add an enum (sound name) to it.
3. In MenuScene find "[SoundManager]" object.
4. Add audioSource as a child object.
5. In SoundManager's inspector add new sound in dictionary with enum and sound effect object.