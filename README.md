# Extensions to Functionality of a Historical MMORPG by Simonas Banys

1. Functions added:
  1.1 Alliances - Alliances have been added to the game. Certain functions are modified to players who are in an alliance with someone else. For example players are not allowed to attack their allies.
  1.2 At war status - while there is currently no punishment for attacking those who players are not at war with, for ease of information and potential further extensions to functionality players have a list of families they have attacked and haven't made peace with.
  1.3 Supporting allies in battle - Players are able to set their armies to automatically support allies in their battles when defending or attacking in the same Fief. It is only possible for attacking an army or besieging a keep.
  1.4 Assassinations - players can send one of the characters belonging to them to assassinate another character.
  1.5 Terrain affect - Fief terrain is now affecting army combat value. For example plains are highly beneficial to knights, men at arms and light cavalry, however produces a small disadvantage to longbowmen and crossbowmen, or such as forests which provide benefits to ranged units, footmen and rabble, but impede mounted ones.
  1.6 Morale - morale now affects armies during combat. With default morale value the armies armies are at their normal strength. The higher morale value, the stronger army combat value becomes, and vice versa.
  1.7 Army Loyalty - armies now have loyalty, which is affected by the player ability to maintain them and pay them. if the player is not able to maintain the army as he is lacking funds, the loyalty drops proportionaly to the amount missing to maintain it.
  1.8 Desertions - if the army loyalty drops bellow a certain value, the troops start deserting an army. Desertions are proportional to loyalty value.
  1.9 Automatic pillaging - armies can be set to automatically pillage a fief they are in if the player is not able to pay the maintenance fee at the end of a season.
  
## HOW TO ##
The server and text client can both be run on the same machine at the same time. To run the server go to ../RepairHist_mmo/bin/Debug/ and run hist_mmorpg.exe. To run text client go to ../TestClientROry/bin/Debug/ and launch TestClientROry.exe. There are currently 3 accounts created for the server:
      1. login: test; password: tomato
      2. login: helen; pasword: potato
      3. login: simon; password: farshas
  The IP address when running on the same machine is localhost.
  
  Once the player is logged in, they should prest "enter" on their keyboard. This will allow the text client to get their character information from the server. Then typing "help" will give all the possible actions for the players to perform. For actions such as attack/changeAtt/changeDef the text client requires exact army ID, which is also case sensitive.
