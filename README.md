# Edge of the Galaxy code
This is for our COMP565 game. Contains all the Enemy AI, Player Controller, and UI scripts.

# Code Description

* EnemyController.cs is in charge of the Enemy AI. It uses NavMeshAgents to walk around set path points and can detect the player based on the given radius.

* EnemyHealth.cs is in charge of the Enemy health stats. This class also takes care of when the enemy takes damage.

* EnemySpawnController.cs is the wave spawning mechanism that was built to test the AI. This can also be repurposed to spawn adds.

* GameCamera.cs is attached to the main camera. The script mainly adjust the camera and locks it onto the player.

* GameManager.cs is a script that handles only the exit program logic.

* Gun.cs is the class that controls our weapons.

* PlayerController.cs is the class that enables the user to move using either KBM or with an Xbox controller.

* PlayerDetectController.cs is the class that checks which input the user is giving to the game.

* PlayerHUD.cs is the script that is in charge of the Player's UI. It displays the players' stats.

* PlayerStats.cs is the script that handles the player's health and energy systems.
