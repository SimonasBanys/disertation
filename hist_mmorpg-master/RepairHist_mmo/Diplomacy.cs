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
            if (!aC.allies.Contains(b) && !bC.allies.Contains(a))
            {
                for (int i = 0; i < Globals_Game.npcMasterList.Count; i++)
                {
                    if (Globals_Game.npcMasterList.Values.ElementAt(i).familyID.Equals(b))
                    {
                        Globals_Game.npcMasterList.Values.ElementAt(i).allies.Add(a);
                    }
                    if (Globals_Game.npcMasterList.Values.ElementAt(i).familyID.Equals(a))
                    {
                        Globals_Game.npcMasterList.Values.ElementAt(i).allies.Add(b);
                    }
                }
                for (int i = 0; i < Globals_Game.pcMasterList.Count; i++)
                {
                    if (Globals_Game.pcMasterList.Values.ElementAt(i).familyID.Equals(b))
                    {
                        Globals_Game.pcMasterList.Values.ElementAt(i).allies.Add(a);
                    }
                    if (Globals_Game.pcMasterList.Values.ElementAt(i).familyID.Equals(a))
                    {
                        Globals_Game.pcMasterList.Values.ElementAt(i).allies.Add(b);
                    }
                }
            }
        }

        /// <summary>
        /// Breaks alliance between Character a family and Character b family
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public void removeAlly(String a, String b)
        {
            Character aC = Globals_Game.getCharFromID(a);
            Character bC = Globals_Game.getCharFromID(b);

            if (aC.allies.Contains(b) && bC.allies.Contains(a))
            {
                for (int i = 0; i < Globals_Game.npcMasterList.Count; i++)
                {
                    if (Globals_Game.npcMasterList.Values.ElementAt(i).familyID.Equals(b))
                    {
                        Globals_Game.npcMasterList.Values.ElementAt(i).allies.Remove(a);
                    }
                    if (Globals_Game.npcMasterList.Values.ElementAt(i).familyID.Equals(a))
                    {
                        Globals_Game.npcMasterList.Values.ElementAt(i).allies.Remove(b);
                    }
                }
                for (int i = 0; i < Globals_Game.pcMasterList.Count; i++)
                {
                    if (Globals_Game.pcMasterList.Values.ElementAt(i).familyID.Equals(b))
                    {
                        Globals_Game.pcMasterList.Values.ElementAt(i).allies.Remove(a);
                    }
                    if (Globals_Game.pcMasterList.Values.ElementAt(i).familyID.Equals(a))
                    {
                        Globals_Game.pcMasterList.Values.ElementAt(i).allies.Remove(b);
                    }
                }
            }
        }
    }
}
