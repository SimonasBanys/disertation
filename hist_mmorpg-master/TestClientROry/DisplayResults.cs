using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using hist_mmorpg;

namespace TestClientROry
{
    class DisplayResults
    {
        public void DisplayMove(ProtoFief moveProtoFief)
        {
            Console.WriteLine("New Fief ID: " + moveProtoFief.fiefID);
        }

        public void DisplayArmyStatus(ProtoGenericArray<ProtoArmyOverview> armiesProtoBuf)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Army Report");
            Console.WriteLine("-----------------------------");
            var counter = 0;
            if (armiesProtoBuf.fields != null)
            {
                foreach (var army in armiesProtoBuf.fields)
                {
                    counter++;
                    Console.WriteLine("Army " + counter);
                    Console.WriteLine("Army ID: " + army.armyID);
                    Console.WriteLine("Owner: " + army.ownerName);
                    Console.WriteLine("Size: " + army.armySize);
                    Console.WriteLine("Troops: ");
                    for (int i = 0; i < army.troops.Length; i++)
                    {
                        switch (i)
                        {
                            case 0:
                                {
                                    Console.WriteLine("  Knights: " + army.troops[i]);
                                    break;
                                }
                            case 1:
                                {
                                    Console.WriteLine("  Men at Arms: " + army.troops[i]);
                                    break;
                                }
                            case 2:
                                {
                                    Console.WriteLine("  Light Cavalry: " + army.troops[i]);
                                    break;
                                }
                            case 3:
                                {
                                    Console.WriteLine("  Longbowmen: " + army.troops[i]);
                                    break;
                                }
                            case 4:
                                {
                                    Console.WriteLine("  Crossbowmen: " + army.troops[4]);
                                    break;
                                }
                            case 5:
                                {
                                    Console.WriteLine("  Footmen: " + army.troops[5]);
                                    break;
                                }
                            case 6:
                                {
                                    Console.WriteLine("  Rabble: " + army.troops[6]);
                                    break;
                                }
                            default:
                                break;
                        }
                    }
                    Console.WriteLine("Location : " + army.locationID);
                    Console.WriteLine("Auto Support when ally attacks in same Fief: " + army.autoSupportAttack);
                    Console.WriteLine("Auto Support when ally defends in same Fief: " + army.autoSupportDefence);
                    Console.WriteLine("Auto Pillage to maintain army: " + army.autoPillage);
                    Console.WriteLine("Army loyalty: " + (army.loyalty+1)*100);
                    Console.WriteLine("Army morale: " + (army.morale+1)*50);
                    Console.WriteLine("-----------------------------");
                }
            }
        }

        public string DisplayCheck(ProtoGenericArray<ProtoFief> fiefsProtoBuf)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Fiefs Owned Report");
            Console.WriteLine("-----------------------------");
            Console.Write("Fiefs owned by ");
            bool written = false;
            string charID = "";
            foreach (var fief in fiefsProtoBuf.fields)
            {
                if (!written)
                {
                    Console.Write(fief.owner + ": \n");
                    written = true;
                    charID = fief.ownerID;
                }
                Console.WriteLine(fief.fiefID);
            }
            Console.WriteLine("-----------------------------");
            return charID;
        }

        public void DisplayChangeAtt(ProtoMessage message)
        {
            Console.WriteLine("-----------------------------");
            Console.Write("Change of auto support when ally attacks: ");
            Console.WriteLine(message.ResponseType);
            Console.WriteLine("-----------------------------");
        }

        public void DisplayChangeDef(ProtoMessage message)
        {
            Console.WriteLine("-----------------------------");
            Console.Write("Change of auto support when ally defends: ");
            Console.WriteLine(message.ResponseType);
            Console.WriteLine("-----------------------------");
        }

        public void DisplayChangePilllage(ProtoMessage message)
        {
            Console.WriteLine("-----------------------------");
            Console.Write("Change of auto pillage to maintain army: ");
            Console.WriteLine(message.ResponseType);
            Console.WriteLine("-----------------------------");
        }

        public void DisplayHire(ProtoMessage recruitProtoBuf)
        {
            if (recruitProtoBuf.ResponseType == DisplayMessages.CharacterRecruitOwn)
            {
                Console.WriteLine("Recruit from a fief you own!");
            }
            else if (recruitProtoBuf.ResponseType == DisplayMessages.CharacterRecruitAlready)
            {
                Console.WriteLine("You have already recruited!");
            }
            else if (recruitProtoBuf.ResponseType == DisplayMessages.CharacterRecruitInsufficientFunds)
            {
                Console.WriteLine("Insufficient recruitment funds!");
            }
            else
            {
                var recruitProtoBufCast = (ProtoRecruit)recruitProtoBuf;
                Console.WriteLine("-----------------------------");
                Console.WriteLine("Recruit Report");
                Console.WriteLine("-----------------------------");
                Console.WriteLine("Army ID: " + recruitProtoBufCast.armyID);
                Console.WriteLine("Recruitment Cost: " + recruitProtoBufCast.cost);
                Console.WriteLine("Amount of Recruits: " + recruitProtoBufCast.amount);
                Console.WriteLine("-----------------------------");
            }
        }

        public void Help()
        {
            Console.WriteLine("Command List: \n siege\n army \n check \n profile \n fief \n move [direction parameter ne,nw,e,w,se,sw] \n hire [amount] \n hirenew [amount] \n fiefs" +
                " \n changeAtt [Army ID] (changes auto support for allied armies when attacking, should heavy losses be incured the army could potentially be disbanded)" +
                " \n changeDef [Army ID] (changes auto support for allied armies when defending, should heavy loses be incurred the army could potentially be disbanded)" +
                " \n sendAssassin [assassinID] [targetID] (assassin may die during the attempt)");
        }

        public void DisplaySiege(ProtoSiegeDisplay siegeDisplayProtoBuf)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Siege Report");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Besieged Fief: " + siegeDisplayProtoBuf.besiegedFief);
            Console.WriteLine("Besieged Army: " + siegeDisplayProtoBuf.besiegerArmy);
            Console.WriteLine("Siege Successful: " + siegeDisplayProtoBuf.besiegerWon);
            Console.WriteLine("Siege Length: " + siegeDisplayProtoBuf.days + " days");
            Console.WriteLine("Loot Lost: " + siegeDisplayProtoBuf.lootLost);
            if (siegeDisplayProtoBuf.attackerAllies.Count > 0 && siegeDisplayProtoBuf.attackerAllies != null)
            {
                Console.WriteLine("Attacker allies participating in the battle: ");
                for (int i = 0; i < siegeDisplayProtoBuf.attackerAllies.Count; i++)
                {
                    Console.Write(siegeDisplayProtoBuf.attackerAllies[i]);
                    if (i != siegeDisplayProtoBuf.attackerAllies.Count - 1)
                    {
                        Console.Write(", ");
                    }
                }
            }
            if (siegeDisplayProtoBuf.defenderAllies.Count > 0 && siegeDisplayProtoBuf.defenderAllies != null)
            {
                Console.WriteLine("Defender allies participating in the battle: ");
                for (int i = 0; i < siegeDisplayProtoBuf.defenderAllies.Count; i++)
                {
                    Console.Write(siegeDisplayProtoBuf.defenderAllies[i]);
                    if (i != siegeDisplayProtoBuf.defenderAllies.Count - 1)
                    {
                        Console.Write(", ");
                    }
                }
            }
            Console.WriteLine("-----------------------------");
        }

        public void DisplaySendAssassin(ProtoMessage reply)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Send Assassin result:");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Making plans result: " + reply.ResponseType);
            Console.WriteLine("Assassin ID: " + reply.MessageFields[0]);
            Console.WriteLine("Target ID: " + reply.MessageFields[1]);
            Console.WriteLine("-----------------------------");
        }

        public void DisplayFief(ProtoFief fiefProtoBuf)
        {
            var armys = fiefProtoBuf.armies;
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Fief Report");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Fief ID: " + fiefProtoBuf.fiefID);
            Console.WriteLine("Owner: " + fiefProtoBuf.owner);
            Console.WriteLine("Owner ID: " + fiefProtoBuf.ownerID);
            Console.WriteLine("Industry Level: " + fiefProtoBuf.industry);
            var characters = fiefProtoBuf.charactersInFief;
            Console.WriteLine("Characters in Fief: ");
            foreach (var character in characters)
            {
                Console.WriteLine("-----------------------------");
                Console.WriteLine("ID: " + character.charID);
                Console.WriteLine("Name :" + character.charName);
                Console.WriteLine("Role: " + character.role);
            }
            Console.WriteLine("-----------------------------");
            if (armys != null)
            {
                Console.WriteLine("Armies in Fief: ");
                foreach (var army in armys)
                {
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine("ID: " + army.armyID);
                    Console.WriteLine("Size :" + army.armySize); // for some reason displays different number than when checking armies 
                    Console.WriteLine("Leader: " + army.leaderName);
                    Console.WriteLine("Owner: " + army.ownerName);
                }
                Console.WriteLine("-----------------------------");
            }
            var keep = fiefProtoBuf.keepLevel;
            Console.WriteLine("Keep Level: " + keep);
            Console.WriteLine("-----------------------------");
            var militia = fiefProtoBuf.militia;
            Console.WriteLine("Number of recruits available: " + militia);
            Console.WriteLine("Number of troops in fief: " + fiefProtoBuf.troops);
            Console.WriteLine("-----------------------------");
        }

        public void DisplayPlayers(ProtoGenericArray<ProtoPlayer> playersProtoBuf)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Players on Server Report");
            Console.WriteLine("-----------------------------");
            var counter = 0;
            foreach (var player in playersProtoBuf.fields)
            {
                counter++;
                Console.WriteLine("Player " + counter);
                Console.WriteLine("Player ID: " + player.playerID);
                Console.WriteLine("Player Name: " + player.pcName);
                Console.WriteLine("-----------------------------");
            }
        }

        public void DisplayProfile(ProtoPlayerCharacter characterProtoBuf)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Player Profile");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Player ID: " + characterProtoBuf.playerID);
            Console.WriteLine("Player Name: " + characterProtoBuf.firstName + " " + characterProtoBuf.familyName);
            Console.WriteLine("-----------------------------");
            Console.Write("Owned Fiefs: ");
            bool written = false;
            foreach (var fief in characterProtoBuf.ownedFiefs)
            {
                if (written == false)
                {
                    Console.Write(fief);
                    written = true;
                }
                else
                    Console.Write(" , " + fief);
            }
            Console.Write("\n");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Location: " + characterProtoBuf.location);
            Console.WriteLine("Army: " + characterProtoBuf.armyID);
            Console.WriteLine("Purse: " + characterProtoBuf.purse);
            Console.Write("Allies: ");
            if (characterProtoBuf.allies != null && characterProtoBuf.allies.Count > 0)
            {
                for (int i = 0; i < characterProtoBuf.allies.Count; i++)
                {
                    Console.Write(characterProtoBuf.allies[i]);
                    if (i != characterProtoBuf.allies.Count - 1)
                    {
                        Console.Write(" , ");
                    }
                }
            }
            Console.WriteLine();
            Console.Write("At war with: ");
            if (characterProtoBuf.atWar != null && characterProtoBuf.atWar.Count > 0)
            {
                for (int i = 0; i < characterProtoBuf.atWar.Count; i++)
                {
                    Console.Write(characterProtoBuf.atWar[i]);
                    if (i != characterProtoBuf.atWar.Count - 1)
                    {
                        Console.Write(", ");
                    }
                }
            }
            Console.WriteLine();
            Console.WriteLine("-----------------------------");
        }

        public void DisplaySeasonUpdate()
        {
            Console.WriteLine("Season Updated!");
        }

        public void DisplaySiegeResult(ProtoGenericArray<ProtoSiegeOverview> siegeResultProtobuf)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Sieges Overview");
            Console.WriteLine("-----------------------------");
            if (siegeResultProtobuf.fields != null)
            {
                foreach (var siege in siegeResultProtobuf.fields)
                {
                    Console.Write(siege.siegeID + ": " + siege.besiegingPlayer
                        + " vs. " + siege.defendingPlayer + " in " + siege.besiegedFief + "\n");
                    Console.WriteLine("-----------------------------");
                }
            }
        }

        public void DisplayAttack(ProtoBattle battleResults)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Battle Overview");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Attacker leader: " + battleResults.attackerLeader);
            Console.WriteLine("Defender leader: " + battleResults.defenderLeader);
            Console.WriteLine("Attacker victorious:" + battleResults.attackerVictorious);
            Console.WriteLine("Attacker calsualties: " + battleResults.attackerCasualties);
            Console.WriteLine("Defender Casualties: " + battleResults.defenderCasualties);
            Console.WriteLine("Attacker stature change: " + battleResults.statureChangeAttacker);
            Console.WriteLine("Defender stature change: " + battleResults.statureChangeDefender);
            if (battleResults.attackerAllies.Count > 0 && battleResults.attackerAllies != null)
            {
                Console.WriteLine("Attacker allies participating in the battle: ");
                for (int i = 0; i < battleResults.attackerAllies.Count; i++)
                {
                    Console.Write(battleResults.attackerAllies[i]);
                    if (i != battleResults.attackerAllies.Count - 1)
                    {
                        Console.Write(", ");
                    }
                }
            }
            if (battleResults.defenderAllies.Count > 0 && battleResults.defenderAllies != null)
            {
                Console.WriteLine("Defender allies participating in the battle: ");
                for (int i = 0; i < battleResults.defenderAllies.Count; i++)
                {
                    Console.Write(battleResults.defenderAllies[i]);
                    if (i != battleResults.defenderAllies.Count - 1)
                    {
                        Console.Write(", ");
                    }
                }
            }
            Console.WriteLine("-----------------------------");
        }

        public void DisplayJournalEntries(ProtoGenericArray<ProtoJournalEntry> journalEntriesProtoBuf)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Journal Entries");
            Console.WriteLine("-----------------------------");
            if (journalEntriesProtoBuf.fields != null)
            {
                foreach (var journal in journalEntriesProtoBuf.fields)
                {
                    Console.WriteLine("-----------------------------");
                    Console.WriteLine("Journal Entry ID: " + journal.jEntryID);
                    Console.WriteLine("Journal Event Year: " + journal.year);
                    Console.WriteLine("Journal Event Location: " + journal.location);
                    Console.WriteLine("Journal Personae: " + journal.personae);
                    Console.WriteLine("-----------------------------");
                }
            }
            else
            {
                Console.WriteLine("No Journal Entries found.");
            }
        }

        public void DisplayJournalEntry(ProtoJournalEntry journalEntryProtoBuf)
        {
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Journal Entry " + journalEntryProtoBuf.jEntryID);
            Console.WriteLine("-----------------------------");
            Console.WriteLine("-----------------------------");
            Console.WriteLine("Journal Entry ID: " + journalEntryProtoBuf.jEntryID);
            Console.WriteLine("Journal Event Year: " + journalEntryProtoBuf.year);
            Console.WriteLine("Journal Event Location: " + journalEntryProtoBuf.location);
            Console.WriteLine("Journal Personae: " + journalEntryProtoBuf.personae);
            Console.WriteLine("-----------------------------");
        }
    }
}
