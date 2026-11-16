# Replay Value

In Replay Value, you are an experiment. You are instructed to go through doors that will unravel either your worst nightmare or your best dream. Remember, which door you pick will decide your fate.

Experience Replay Value and see if you can survive the scientist's mystery and madness.

## Controls:

WASD - Move

E - Interact with doors/buttons

Shift - Sprint

In order to advance to the next door, press one of the buttons and open the corresponding door, repeating until either your demise or your commencement.

# Instructions for download & playing
If you wish to download the game through this repo:
1. Navigate to the main page
2. Click the code button
3. Select "Download ZIP"
4. Extract the downloaded ZIP file
5. Open Unity Hub
6. Click Add -> Add Project from disk
7. Browse to the extracted project folder
8. Select the folder and click "Add Project"
9. Open the project through Unity Hub

Make sure you have Unity Hub installed; the version of this game is 6000.4.2f1

If you wish to download the game through the Itch.io game page, go to https://nullprod.itch.io/replayvaluepc and press the download button available on screen.

# Technical Documentation

## The Player:
The player system uses Unity's new input system for movement and interaction: WASD for moving; E for interactions; Shift for sprinting.

The player is able to walk at any given time, but in order to sprint, they need to have stamina.
When stamina is depleted, the player must wait until they regenerate to about 20 stamina points in order to sprint again.

Walking speed is 5, and the sprinting speed is 7.

Camera Rotation uses both horizontal and vertical mouse movements; the pitch is between -80 and 80 in order to prevent weird camera issues.

### Jumping:
The user can press space to jump when they are grounded
jumpHeight determines the height; gravity controls downward acceleration

Gravity is continuously applied to the player in order to prevent any issues and make the game feel realistic.

### Collisions for Player:
The player uses a CharacterController for stability, while objects use Rigidbodies for easy physics

When the player interacts with an object containing a rigibody the following is done:
direction between the player and the object is calculated, vertical force is removed, allowing for a horizontal force, and an impulse effect is applied to the rigid body

Players can throw and pick up objects
A sphere cast is fired from the camera in the direction it is looking
Spherecast checks for objects within the specific range (pickupRange)
If the object has a tag (Throwable), the rigidbody is stored in heldObject (property in player)
Rigidbody is set to kinematic in order to prevent physics from affecting it during its holding period
position is updated to match holdPoint every frame

### Components needed:
Charcontroller
PlayerLocomotionInput (for managing input through script)
Camera
HoldPoint (empty game object)
AudioSources (all four audio sources picked; they should be stored in a "hub" inside the scene)
Rigidbody (required on pushable/throwable objects)
"Throwable" tag is required for throwable objects to work.

## Door System:

To simply explain, each room has a "doorSpawn" object where the next lobby will spawn once the room spawns in. Each door in the lobby has its own spawn (redSpawn, blueSpawn) where a random room from a list is chosen to be the next room. The room is then removed from the list once it spawns in. Once the player loads up a lobby after opening the room's door, that room will then disappear, freeing up memory.

The player must press a button before opening a door
When Open() is called, the script checks whether the player is within 1.5 units (the buttonProximity range) of either button, resulting in the selected button being stored as pressedButton. An animation then plays by doing this through Vector3.MoveTowards()

If the player is close enough to the corresponding door and has pressed a button, the door will then open, using Vector3.RotateTowards

### The room selection in depth:
A random index is generated through Random.Range()
Existing room objects under the roomParent (where rooms spawn) are destroyed
A random room from the hubScript.rooms (the rooms list) is spawned in (instantiated)
The room is positioned and rotated using the door's spawn point
hubScript.removeRoom(randomIndex) removes the selected room from the available room list

When there are no remaining rooms, the script instantiates the finalRoom instead.

###Important note: Open() must be called by the interaction/input system when the player attempts to interact; the script handles proximity checks/logic/audio/animation/room spawning

I hope you enjoy experiencing this and consider it for a Steam release!
If we are going to proceed with a public release, we plan on adding more rooms (using original assets), revamping UI elements to improve user experience, and more.

Even if we don't release on Steam, we still have plans to improve the user experience and add content, just not as much as with a Steam release.

We're aiming for a late February/early March release; we are confident that we can hit this deadline.
