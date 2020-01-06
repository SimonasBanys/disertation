using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
namespace ProtoMessage
{
    [ContractVerification(true)] 
    public static class Battle
    {
        /// <summary>
        /// Calculates whether the attacking army is able to successfully bring the defending army to battle
        /// </summary>
        /// <returns>bool indicating whether battle has commenced</returns>
        /// <param name="attackerValue">uint containing attacking army battle value</param>
        /// <param name="defenderValue">uint containing defending army battle value</param>
        /// <param name="circumstance">string indicating circumstance of battle</param>
        private static bool BringToBattle(uint attackerValue, uint defenderValue, string circumstance = "battle")
        {
            bool battleHasCommenced = false;
            double[] combatOdds = Globals_Server.battleProbabilities["odds"];
            double[] battleChances = Globals_Server.battleProbabilities[circumstance];
            double thisChance = 0;

            for (int i = 0; i < combatOdds.Length; i++)
            {
                if (i < combatOdds.Length - 1)
                {
                    // ReSharper disable once PossibleLossOfFraction
                    if (attackerValue / defenderValue < combatOdds[i])
                    {
                        thisChance = battleChances[i];
                        break;
                    }
                }
                else
                {
                    thisChance = battleChances[i];
                    break;
                }
            }

            // generate random percentage
            int randomPercentage = Globals_Game.myRand.Next(101);

            // compare random percentage to battleChance
            if (randomPercentage <= thisChance)
            {
                battleHasCommenced = true;
            }
            return battleHasCommenced;
        }

        /// <summary>
        /// Determines whether the attacking army is victorious in a battle
        /// </summary>
        /// <returns>bool indicating whether attacking army is victorious</returns>
        /// <param name="attackerValue">uint containing attacking army battle value</param>
        /// <param name="defenderValue">uint containing defending army battle value</param>
        public static bool DecideBattleVictory(uint attackerValue, uint defenderValue)
        {
            bool attackerVictorious = false;

            // calculate chance of victory
            double attackerVictoryChance = Battle.CalcVictoryChance(attackerValue, defenderValue);

            // generate random percentage
            int randomPercentage = Globals_Game.myRand.Next(101);

            // compare random percentage to attackerVictoryChance
            if (randomPercentage <= attackerVictoryChance)
            {
                attackerVictorious = true;
            }

            return attackerVictorious;
        }

        /// <summary>
        /// Calculates chance that the attacking army will be victorious in a battle
        /// </summary>
        /// <returns>double containing percentage chance of victory</returns>
        /// <param name="attackerValue">uint containing attacking army battle value</param>
        /// <param name="defenderValue">uint containing defending army battle value</param>
        public static double CalcVictoryChance(uint attackerValue, uint defenderValue)
        {
            return (attackerValue / (Convert.ToDouble(attackerValue + defenderValue))) * 100;
        }

        /// <summary>
        /// Calculates casualties from a battle for both sides
        /// </summary>
        /// <returns>double[] containing percentage loss modifier for each side</returns>
        /// <param name="attackerTroops">uint containing attacking army troop numbers</param>
        /// <param name="defenderTroops">uint containing defending army troop numbers</param>
        /// <param name="attackerValue">uint containing attacking army battle value</param>
        /// <param name="defenderValue">uint containing defending army battle value</param>
        /// <param name="attackerVictorious">bool indicating whether attacking army was victorious</param>
        public static double[] CalculateBattleCasualties(uint attackerTroops, uint defenderTroops, uint attackerValue, uint defenderValue, bool attackerVictorious)
        {
            double[] battleCasualties = new double[2];
            double largeArmyModifier = 0;
            bool largestWon = true;

            // determine highest/lowest battle value
            double maxBV = Math.Max(attackerValue, defenderValue);
            double minBV = Math.Min(attackerValue, defenderValue);

            // use BVs to determine high mark for base casualty rate of army with smallest battle value (see below)
            double highCasualtyRate = maxBV / (maxBV + minBV);

            // determine base casualty rate for army with smallest battle value
            double smallestModifier = Utility_Methods.GetRandomDouble(highCasualtyRate, min: 0.1);

            // determine if army with largest battle value won
            if (maxBV == attackerValue)
            {
                if (!attackerVictorious)
                {
                    largestWon = false;
                }
            }
            else
            {
                if (attackerVictorious)
                {
                    largestWon = false;
                }
            }

            // if army with largest battle value won
            if (largestWon)
            {
                // calculate casualty modifier for army with largest battle value
                // (based on adapted version of Lanchester's Square Law - i.e. largest army loses less troops than smallest)
                largeArmyModifier = (1 + ((minBV * minBV) / (maxBV * maxBV))) / 2;

                // attacker is large army
                if (attackerVictorious)
                {
                    battleCasualties[1] = smallestModifier;
                    // determine actual troop losses for largest army based on smallest army losses,
                    // modified by largeArmyModifier
                    uint largeArmyLosses = Convert.ToUInt32((defenderTroops * battleCasualties[1]) * largeArmyModifier);
                    // derive final casualty modifier for largest army
                    battleCasualties[0] = largeArmyLosses / (double)attackerTroops;
                }
                // defender is large army
                else
                {
                    battleCasualties[0] = smallestModifier;
                    uint largeArmyLosses = Convert.ToUInt32((attackerTroops * battleCasualties[0]) * largeArmyModifier);
                    battleCasualties[1] = largeArmyLosses / (double)defenderTroops;
                }
            }

            // if army with smallest battle value won
            else
            {
                // calculate casualty modifier for army with largest battle value
                // this ensures its losses will be roughly the same as the smallest army (because it lost)
                largeArmyModifier = Utility_Methods.GetRandomDouble(1.20, min: 0.8);

                // defender is large army
                if (attackerVictorious)
                {
                    // smallest army losses reduced because they won
                    battleCasualties[0] = smallestModifier / 2;
                    // determine actual troop losses for largest army based on smallest army losses,
                    // modified by largeArmyModifier
                    uint largeArmyLosses = Convert.ToUInt32((attackerTroops * battleCasualties[0]) * largeArmyModifier);
                    // derive final casualty modifier for largest army
                    battleCasualties[1] = largeArmyLosses / (double)defenderTroops;
                }
                // attacker is large army
                else
                {
                    battleCasualties[1] = smallestModifier / 2;
                    uint largeArmyLosses = Convert.ToUInt32((defenderTroops * battleCasualties[1]) * largeArmyModifier);
                    battleCasualties[0] = largeArmyLosses / (double)attackerTroops;
                }
            }


            return battleCasualties;
        }

        /// <summary>
        /// Calculates whether either army has retreated due to the outcome of a battle
        /// </summary>
        /// <returns>int[] indicating the retreat distance (fiefs) of each army. First index is attacker, second is defender</returns>
        /// <param name="attacker">The attacking army</param>
        /// <param name="defender">The defending army</param>
        /// <param name="aCasualties">The attacking army casualty modifier</param>
        /// <param name="dCasualties">The defending army casualty modifier</param>
        /// <param name="attackerVictorious">bool indicating if attacking army was victorious</param>
        public static int[] CheckForRetreat(Army attacker, Army defender, double aCasualties, double dCasualties, bool attackerVictorious)
        {
            Contract.Requires(defender!=null);
            bool[] hasRetreated = { false, false };
            int[] retreatDistance = { 0, 0 };

            // check if loser retreats due to battlefield casualties
            if (!attackerVictorious)
            {
                // if have >= 20% casualties
                if (aCasualties >= 0.2)
                {
                    // indicate attacker has retreated
                    hasRetreated[0] = true;

                    // generate random 1-2 to determine retreat distance
                    retreatDistance[0] = Globals_Game.myRand.Next(1, 3);
                }
            }
            else
            {
                // if have >= 20% casualties
                if (dCasualties >= 0.2)
                {
                    // indicate defender has retreated
                    hasRetreated[1] = true;

                    // generate random 1-2 to determine retreat distance
                    retreatDistance[1] = Globals_Game.myRand.Next(1, 3);
                }
            }

            // check to see if defender retreats due to aggression setting (i.e. was forced into battle)
            // NOTE: this will only happen if attacker and defender still present in fief
            if ((defender.aggression == 0) && (!hasRetreated[0] && !hasRetreated[1]))
            {
                // indicate defender has retreated
                hasRetreated[1] = true;

                // indicate retreat distance
                retreatDistance[1] = 1;
            }

            return retreatDistance;
        }

        /// <summary>
        /// Calculates rough battle odds between two armies (i.e ratio of attacking army combat
        /// value to defending army combat value).  NOTE: does not involve leadership values
        /// </summary>
        /// <returns>int containing battle odds</returns>
        /// <param name="attacker">The attacking army</param>
        /// <param name="defender">The defending army</param>
        public static int GetBattleOdds(Army attacker, Army defender)
        {
            Contract.Requires(attacker!=null&&defender!=null);
            double battleOdds = 0;

            battleOdds = Math.Floor(attacker.CalculateCombatValue() / defender.CalculateCombatValue());

            return Convert.ToInt32(battleOdds);
        }

        /// <summary>
        /// Return a string describing the results of a battle
        /// </summary>
        /// <param name="battle">Results of battle</param>
        /// <returns>String description</returns>
        public static String DisplayBattleResults(ProtoBattle battle)
        {
            Contract.Requires(battle!=null);
            // Battle introduction
            string toDisplay = "The fief garrison and militia";
            if (battle.attackerLeader != null)
            {
                toDisplay += ", led by " + battle.attackerLeader + ",";
            }
            switch (battle.circumstance)
            {
                // Normal battle
                case 0:
                {
                    toDisplay+=" moved to attack ";
                }
                    break;
                // Pillage
                case 1:
                {
                    toDisplay += " sallied forth to bring the pillaging army,";
                    
                }
                    break;
                // Siege
                case 2:
                {
                    toDisplay += ", sallied forth to bring the besieging army, ";
                }
                    break;
                default:
                    toDisplay = "Unrecognised circumstance";
                    break;
            }
            if (battle.defenderLeader != null)
            {
                toDisplay += " led by " + battle.defenderLeader + " and";
            }
            toDisplay += " owned by " + battle.defenderOwner
                         + ", to battle in the fief of " + Globals_Game.fiefMasterList[battle.battleLocation].name+"."
                + "\r\n\r\nOutcome: ";
            if (battle.battleTookPlace)
            {
                if (battle.attackerVictorious)
                {
                    toDisplay += battle.attackerOwner;
                }
                else
                {
                    toDisplay += battle.defenderOwner;
                }

                // Victory status
                toDisplay += "'s army was victorious.\r\n\r\n";

                // Casualties
                toDisplay += battle.attackerOwner + "'s army suffered " + battle.attackerCasualties +
                             " troop casualties.\n";
                toDisplay += battle.defenderLeader + "'s army suffered " + battle.defenderCasualties +
                             " troop casualties.\n";

                // Retreats
                foreach (var retreater in battle.retreatedArmies)
                {
                    toDisplay += retreater + "'s army retreated from the fief.\n";
                }

                // Disbands
                foreach (var disbander in battle.disbandedArmies)
                {
                    toDisplay += disbander + "'s army disbanded due to heavy casualties.\n";
                }

                toDisplay += string.Join(", ", battle.deaths) + " all died due to injuries received.\n";
                if (battle.circumstance == 1)
                {
                    toDisplay += "The pillage in " + Globals_Game.fiefMasterList[battle.battleLocation].name +
                                 " has been prevented";
                }

                // Siege results
                if (battle.circumstance == 2)
                {
                    if (battle.attackerVictorious || battle.retreatedArmies.Contains(battle.attackerOwner))
                    {
                        toDisplay += battle.attackerOwner + "'s defenders have defeated the forces of " +
                                     battle.defenderOwner + ", relieving the siege of " +
                                     Globals_Game.fiefMasterList[battle.battleLocation].name
                                     + ". " + battle.defenderOwner +
                                     " retains ownership of the fief. The siege has been raised.\n";

                    }
                    else if(battle.DefenderDeadNoHeir)
                    {
                        // add to message
                        toDisplay += "The siege in " + Globals_Game.fiefMasterList[battle.battleLocation].name + " has been raised";
                        toDisplay += " due to the death of the besieging party, ";
                        toDisplay += battle.siegeBesieger+ ".";
                    }
                }
            }
            else
            {
                if (battle.circumstance > 0)
                {
                    toDisplay += battle.attackerOwner + "'s forces failed to bring their aggressors to battle.\n";
                }
                else
                {
                    toDisplay += battle.defenderOwner +
                                 "'s forces successfully refused battle and retreated from the fief.";
                }
            }
            return toDisplay; 
        }

        /// <summary>
        /// Display the results of a siege (that has been resolved due to a battle) in a human readable format
        /// </summary>
        /// <param name="battle">Results of battle</param>
        /// <returns>String describing battle</returns>
        public static String DisplaySiegeResults(ProtoBattle battle)
        {
            Contract.Requires(battle!=null);
            string siegeDescription = "";
            if (battle.attackerVictorious || battle.retreatedArmies.Contains(battle.attackerOwner))
            {
                siegeDescription = "On this day of Our Lord the forces of ";
                siegeDescription += battle.siegeDefender;
                siegeDescription += " have defeated the forces of " + battle.siegeBesieger;
                siegeDescription += ", relieving the siege of " + Globals_Game.fiefMasterList[battle.battleLocation].name + ".";
                siegeDescription += " " + battle.siegeDefender;
                siegeDescription += " retains ownership of the fief.";
            }
            if (battle.DefenderDeadNoHeir)
            {
                // construct event description to be passed into siegeEnd
                siegeDescription = "On this day of Our Lord the forces of ";
                siegeDescription += battle.siegeBesieger;
                siegeDescription += " attacked the forces of " + battle.siegeDefender;
                siegeDescription += ", who was killed during the battle.";
                siegeDescription += "  Thus, despite losing the battle, " + battle.siegeBesieger;
                siegeDescription += " has succeeded in relieving the siege of " + Globals_Game.fiefMasterList[battle.battleLocation].name + ".";
                siegeDescription += " " + battle.siegeDefender;
                siegeDescription += " retains ownership of the fief.";
            }
            return siegeDescription;

        }

        /// <summary>
        /// Implements the processes involved in a battle between two armies in the field
        /// </summary>
        /// <returns>bool indicating whether attacking army is victorious</returns>
        /// <remarks>
        /// Predicate: assumes attacker has sufficient days
        /// Predicate: assumes attacker has leader
        /// Predicate: assumes attacker in same fief as defender
        /// Predicate: assumes defender not besieged in keep
        /// Predicate: assumes attacker and defender not same army
        /// </remarks>
        /// <param name="attacker">The attacking army</param>
        /// <param name="defender">The defending army</param>
        /// <param name="circumstance">string indicating circumstance of battle</param>

    }
}
