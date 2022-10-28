# HitMultiplayer

> by Nordup Ondar

Create or join room and play with your friends through LAN.

Attack opponents with LMB, earn more score and win the match.


## Run on Windows

1. Download and extract HitMultiplayer.zip
2. Run executable HitMultiplayer.exe
3. Allow access to private networks
##### Note. Executable and project might not be compatible


## Screenshots
#
<img src="Screenshots\Screenshot_1.png" width="500">

<img src="Screenshots\Screenshot_2.png" width="500">

<img src="Screenshots\Screenshot_3.png" width="500">


#
### Tweak parameters in Unity Editor

- Player movement speed, dash time and distance => `Player prefab > Movement component`
- Player color change time on hit => `Player prefab > Collision component (hitTime)`
- Win score => `Main scene > ScoreManager object`
- Match restart time => `Main scene > RestartMatch object`

### Used free plugins

- [Mirror](https://assetstore.unity.com/packages/tools/network/mirror-129321) - for networking
- [Prototyping Pack](https://assetstore.unity.com/packages/3d/prototyping-pack-free-94277) - for scene assets
- [In-game Debug Console](https://assetstore.unity.com/packages/tools/gui/in-game-debug-console-68068), [ParrelSync](https://github.com/VeriorPies/ParrelSync) - for debugging purposes