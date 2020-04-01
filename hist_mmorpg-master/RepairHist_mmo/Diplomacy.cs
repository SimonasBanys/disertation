using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hist_mmorpg
{
    class Diplomacy
    {
        /// <summary>
        /// forges alliance between character a and character b families. depending on how the alliance was forged an identifier would be used to determine so
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="x"></param>
        /// <returns></returns>
        public static void forgeAlliance(String a, String b)
        {
            Character aC = Globals_Game.getCharFromID(a);
            Character bC = Globals_Game.getCharFromID(b);
            if (!aC.isAlly(bC) && !bC.isAlly(aC))
            {
                for (int i = 0; i < Globals_Game.npcMasterList.Count; i++)
                {
                    if (Globals_Game.npcMasterList.Values.ElementAt(i).familyID.Equals(bC.familyID))
                    {
                        Globals_Game.npcMasterList.Values.ElementAt(i).allies.Add(aC.familyID);
                    }
                    if (Globals_Game.npcMasterList.Values.ElementAt(i).familyID.Equals(aC.familyID))
                    {
                        Globals_Game.npcMasterList.Values.ElementAt(i).allies.Add(bC.familyID);
                    }
                }
                for (int i = 0; i < Globals_Game.pcMasterList.Count; i++)
                {
                    if (Globals_Game.pcMasterList.Values.ElementAt(i).familyID.Equals(bC.familyID))
                    {
                        Globals_Game.pcMasterList.Values.ElementAt(i).allies.Add(aC.familyID);
                    }
                    if (Globals_Game.pcMasterList.Values.ElementAt(i).familyID.Equals(aC.familyID))
                    {
                        Globals_Game.pcMasterList.Values.ElementAt(i).allies.Add(bC.familyID);
                    }
                }
            }
        }

        /// <summary>
        /// Breaks alliance between Character a family and Character b family
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void removeAlly(String a, String b)
        {
            Character aC = Globals_Game.getCharFromID(a);
            Character bC = Globals_Game.getCharFromID(b);

            if (aC.isAlly(bC) && bC.isAlly(aC))
            {
                for (int i = 0; i < Globals_Game.npcMasterList.Count; i++)
                {
                    if (Globals_Game.npcMasterList.Values.ElementAt(i).familyID.Equals(bC.familyID))
                    {
                        Globals_Game.npcMasterList.Values.ElementAt(i).allies.Remove(aC.familyID);
                    }
                    if (Globals_Game.npcMasterList.Values.ElementAt(i).familyID.Equals(aC.familyID))
                    {
                        Globals_Game.npcMasterList.Values.ElementAt(i).allies.Remove(bC.familyID);
                    }
                }
                for (int i = 0; i < Globals_Game.pcMasterList.Count; i++)
                {
                    if (Globals_Game.pcMasterList.Values.ElementAt(i).familyID.Equals(bC.familyID))
                    {
                        Globals_Game.pcMasterList.Values.ElementAt(i).allies.Remove(aC.familyID);
                    }
                    if (Globals_Game.pcMasterList.Values.ElementAt(i).familyID.Equals(aC.familyID))
                    {
                        Globals_Game.pcMasterList.Values.ElementAt(i).allies.Remove(bC.familyID);
                    }
                }
            }
        }
        /// <summary>
        /// Adds all characters from family A and B to each others "At War" lists
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void declareWar(string a, string b)
        {
            Character aC = Globals_Game.getCharFromID(a);
            Character bC = Globals_Game.getCharFromID(b);

            if (!aC.wagingWar(bC) && !bC.wagingWar(aC))
            {
                for (int i = 0; i < Globals_Game.npcMasterList.Count; i++)
                {
                    if (Globals_Game.npcMasterList.Values.ElementAt(i).familyID.Equals(bC.familyID))
                    {
                        Globals_Game.npcMasterList.Values.ElementAt(i).atWar.Add(aC.familyID);
                    }
                    if (Globals_Game.npcMasterList.Values.ElementAt(i).familyID.Equals(aC.familyID))
                    {
                        Globals_Game.npcMasterList.Values.ElementAt(i).atWar.Add(bC.familyID);
                    }
                }
                for (int i = 0; i < Globals_Game.pcMasterList.Count; i++)
                {
                    if (Globals_Game.pcMasterList.Values.ElementAt(i).familyID.Equals(bC.familyID))
                    {
                        Globals_Game.pcMasterList.Values.ElementAt(i).atWar.Add(aC.familyID);
                    }
                    if (Globals_Game.pcMasterList.Values.ElementAt(i).familyID.Equals(aC.familyID))
                    {
                        Globals_Game.pcMasterList.Values.ElementAt(i).atWar.Add(bC.familyID);
                    }
                }
            }
        }
        /// <summary>
        /// Removes character A and B families from each other "At War" lists
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void makePeace(string a, string b)
        {
            Character aC = Globals_Game.getCharFromID(a);
            Character bC = Globals_Game.getCharFromID(b);

            if (aC.wagingWar(bC) && bC.wagingWar(aC))
            {
                for (int i = 0; i < Globals_Game.npcMasterList.Count; i++)
                {
                    if (Globals_Game.npcMasterList.Values.ElementAt(i).familyID.Equals(bC.familyID))
                    {
                        Globals_Game.npcMasterList.Values.ElementAt(i).atWar.Remove(aC.familyID);
                    }
                    if (Globals_Game.npcMasterList.Values.ElementAt(i).familyID.Equals(aC.familyID))
                    {
                        Globals_Game.npcMasterList.Values.ElementAt(i).atWar.Remove(bC.familyID);
                    }
                }
                for (int i = 0; i < Globals_Game.pcMasterList.Count; i++)
                {
                    if (Globals_Game.pcMasterList.Values.ElementAt(i).familyID.Equals(bC.familyID))
                    {
                        Globals_Game.pcMasterList.Values.ElementAt(i).atWar.Remove(aC.familyID);
                    }
                    if (Globals_Game.pcMasterList.Values.ElementAt(i).familyID.Equals(aC.familyID))
                    {
                        Globals_Game.pcMasterList.Values.ElementAt(i).atWar.Remove(bC.familyID);
                    }
                }
            }
        }
    }
}
