﻿using System;
using System.Threading.Tasks;
using hist_mmorpg;

namespace TestClientRory
{
    public class PlayerOperations
    {
        public enum MoveDirections
        {
            E, W, Se, Sw, Ne, Nw, SyntaxError
        }
        public ProtoFief Move(MoveDirections directions, TextTestClient client)
        {
            ProtoTravelTo protoTravel = new ProtoTravelTo();
            protoTravel.travelVia = new[] {directions.ToString()};
            protoTravel.characterID = client.charID;
            client.net.Send(protoTravel);
            var reply = GetActionReply(Actions.TravelTo, client);
            var travel = (ProtoFief) reply.Result;
            return travel;
        }

        public ProtoGenericArray<ProtoFief> Check(TextTestClient client)
        {
            ProtoMessage checkMessage = new ProtoMessage();
            checkMessage.ActionType = Actions.ViewMyFiefs;
            client.net.Send(checkMessage);
            var reply = GetActionReply(Actions.ViewMyFiefs, client);
            var fiefs = (ProtoGenericArray<ProtoFief>) reply.Result;
            return fiefs;
        }

        public ProtoGenericArray<ProtoArmyOverview> ArmyStatus(TextTestClient client)
        {
            ProtoArmy proto = new ProtoArmy();
            proto.ownerID = client.charID;
            proto.ActionType = Actions.ListArmies;
            client.net.Send(proto);
            var reply = GetActionReply(Actions.ListArmies, client);
            var armies = (ProtoGenericArray<ProtoArmyOverview>) reply.Result;
            return armies;
        }



        public ProtoMessage changeAttack(string aID, TextTestClient client)
        {
            ProtoArmy proto = new ProtoArmy();
            proto.owner = client.charID;
            proto.Message = aID;
            proto.ActionType = Actions.ChangeAttackAutoSupport;
            client.net.Send(proto);
            var armyReply = GetActionReply(Actions.ChangeAttackAutoSupport, client);
            var armyResult = (ProtoMessage)armyReply.Result;
            return armyResult;
        }

        public ProtoMessage changeDefence(string aID, TextTestClient client)
        {
            ProtoArmy proto = new ProtoArmy();
            proto.owner = client.charID;
            proto.Message = aID;
            proto.ActionType = Actions.ChangeDefenceAutoSupport;
            client.net.Send(proto);
            var armyReply = GetActionReply(Actions.ChangeDefenceAutoSupport, client);
            var armyResult = (ProtoMessage)armyReply.Result;
            return armyResult;
        }

        public ProtoMessage sendAssassin(string assassin, string target, TextTestClient client)
        {
            ProtoMessage proto = new ProtoMessage();
            proto.Message = assassin;
            proto.MessageFields = new string[] { target };
            proto.ActionType = Actions.PlanAssassination;
            client.net.Send(proto);
            var protoReply = GetActionReply(Actions.PlanAssassination, client);
            var assassinResult = (ProtoMessage)protoReply.Result;
            return assassinResult;
        }

        public ProtoMessage changePillage(string aID, TextTestClient client)
        {
            ProtoArmy proto = new ProtoArmy();
            proto.owner = client.charID;
            proto.Message = aID;
            proto.ActionType = Actions.ChangeAutoPillage;
            client.net.Send(proto);
            var armyReply = GetActionReply(Actions.ChangeAutoPillage, client);
            var armyResult = (ProtoMessage)armyReply.Result;
            return armyResult;
        }

        public Task<ProtoMessage> GetActionReply(Actions action, TextTestClient client)
        {
            Task<ProtoMessage> responseTask = client.GetReply();
            responseTask.Wait();
            while (responseTask.Result.ActionType != action)
            {
                responseTask = client.GetReply();
                responseTask.Wait();
            }
            client.ClearMessageQueues();
            return responseTask;
        }

        public ProtoMessage HireTroops(int amount, TextTestClient client)
        {
            ProtoPlayerCharacter protoMessage = new ProtoPlayerCharacter();
            protoMessage.Message = client.charID;
            protoMessage.ActionType = Actions.ViewChar;
            client.net.Send(protoMessage);
            var armyReply = GetActionReply(Actions.ViewChar, client);
            var armyResult = (ProtoPlayerCharacter)armyReply.Result;
            ProtoRecruit protoRecruit = new ProtoRecruit();
            protoRecruit.ActionType = Actions.RecruitTroops;
            if (amount > 0)
            {
                protoRecruit.amount = (uint) amount;
            }
            protoRecruit.armyID = armyResult.armyID;
            protoRecruit.isConfirm = true;
            client.net.Send(protoRecruit);
            var reply = GetActionReply(Actions.RecruitTroops, client);
            return reply.Result;
        }

        public ProtoMessage HireNew(int amount, TextTestClient client)
        {
            ProtoPlayerCharacter protoMessage = new ProtoPlayerCharacter();
            protoMessage.Message = client.charID;
            protoMessage.ActionType = Actions.ViewChar;
            client.net.Send(protoMessage);
            var armyReply = GetActionReply(Actions.ViewChar, client);
            var armyResult = (ProtoPlayerCharacter)armyReply.Result;
            ProtoRecruit protoRecruitNew = new ProtoRecruit();
            protoRecruitNew.ActionType = Actions.RecruitNew;
            if (amount > 0)
            {
                protoRecruitNew.amount = (uint) amount;
            }
            protoRecruitNew.isConfirm = true;
            client.net.Send(protoRecruitNew);
            var reply = GetActionReply(Actions.RecruitNew, client);
            return reply.Result;
        }

        public ProtoMessage SiegeCurrentFief(TextTestClient client)
        {
            ProtoPlayerCharacter protoMessage = new ProtoPlayerCharacter();
            protoMessage.Message = client.charID;
            protoMessage.ActionType = Actions.ViewChar;
            client.net.Send(protoMessage);
            var locReply = GetActionReply(Actions.ViewChar, client);
            var locResult = (ProtoPlayerCharacter)locReply.Result;
            ProtoMessage protoSiegeStart = new ProtoMessage();
            protoSiegeStart.ActionType = Actions.BesiegeFief;
            protoSiegeStart.Message = locResult.armyID;
            client.net.Send(protoSiegeStart);
            var reply = GetActionReply(Actions.BesiegeFief, client);
            if (reply.GetType() == typeof(ProtoSiegeDisplay))
            {
                return reply.Result as ProtoSiegeDisplay;
            }
            else
            {
                return reply.Result;
            }
        }

        public ProtoMessage AttackArmy(string aID, TextTestClient client)
        {
            ProtoPlayerCharacter protoMessage = new ProtoPlayerCharacter();
            protoMessage.Message = client.charID;
            protoMessage.ActionType = Actions.ViewChar;
            client.net.Send(protoMessage);
            var locReply = GetActionReply(Actions.ViewChar, client);
            var locResult = (ProtoPlayerCharacter)locReply.Result;
            ProtoMessage protoAttack = new ProtoMessage();
            protoAttack.ActionType = Actions.Attack;
            protoAttack.Message = locResult.armyID;
            protoAttack.MessageFields = new string[] { aID };
            client.net.Send(protoAttack);
            var reply = GetActionReply(Actions.Attack, client);
            if (reply.GetType() == typeof(ProtoBattle))
            {
                return reply.Result as ProtoBattle;
            } else
            {
                return reply.Result;
            }

        }

        public ProtoFief FiefDetails(TextTestClient client)
        {
            ProtoPlayerCharacter protoMessage = new ProtoPlayerCharacter();
            protoMessage.Message = client.charID;
            protoMessage.ActionType = Actions.ViewChar;
            client.net.Send(protoMessage);
            var locReply = GetActionReply(Actions.ViewChar, client);
            var locResult = (ProtoPlayerCharacter)locReply.Result;
            ProtoFief protoFief = new ProtoFief();
            protoFief.Message = locResult.location;
            protoFief.ActionType = Actions.ViewFief;
            client.net.Send(protoFief);
            var reply = GetActionReply(Actions.ViewFief, client);
            return (ProtoFief) reply.Result;
        }

        public ProtoGenericArray<ProtoPlayer> Players(TextTestClient client)
        {
            ProtoPlayer protoPlayer = new ProtoPlayer();
            protoPlayer.ActionType = Actions.GetPlayers;
            client.net.Send(protoPlayer);
            var reply = GetActionReply(Actions.GetPlayers, client);
            return (ProtoGenericArray<ProtoPlayer>) reply.Result;
        }

        public ProtoPlayerCharacter Profile(TextTestClient client)
        {
            ProtoPlayerCharacter protoMessage = new ProtoPlayerCharacter();
            protoMessage.Message = client.charID;
            protoMessage.ActionType = Actions.ViewChar;
            client.net.Send(protoMessage);
            var reply = GetActionReply(Actions.ViewChar, client);
            return (ProtoPlayerCharacter) reply.Result;
        }

        public ProtoMessage SeasonUpdate(TextTestClient client)
        {
            ProtoMessage protoMessage = new ProtoMessage();
            protoMessage.ActionType = Actions.SeasonUpdate;
            client.net.Send(protoMessage);
            var reply = GetActionReply(Actions.SeasonUpdate, client);
            return reply.Result;
        }

        public ProtoGenericArray<ProtoSiegeOverview> SiegeList(TextTestClient client)
        {
            ProtoMessage protoMessage = new ProtoMessage();
            protoMessage.ActionType = Actions.SiegeList;
            client.net.Send(protoMessage);
            var reply = GetActionReply(Actions.SiegeList, client);
            return (ProtoGenericArray<ProtoSiegeOverview>) reply.Result;
        }

        public ProtoGenericArray<ProtoJournalEntry> JournalEntries(TextTestClient client)
        {
            ProtoMessage protoMessage = new ProtoMessage();
            protoMessage.ActionType = Actions.ViewJournalEntries;
            client.net.Send(protoMessage);
            var reply = GetActionReply(Actions.ViewJournalEntries, client);
            return (ProtoGenericArray<ProtoJournalEntry>) reply.Result;
        }

        public ProtoJournalEntry Journal(string Journal, TextTestClient client){
            ProtoMessage protoMessage = new ProtoMessage();
            protoMessage.ActionType = Actions.ViewJournalEntry;
            protoMessage.Message = Journal;
            client.net.Send(protoMessage);
            var reply = GetActionReply(Actions.ViewJournalEntry, client);
            return (ProtoJournalEntry) reply.Result;
        }

        public ProtoGenericArray<ProtoNPC> AdjustFiefExpenditure(string type, TextTestClient client)
        {
            ProtoNPC protoMessage = new ProtoNPC();
            protoMessage.ActionType = Actions.AdjustExpenditure;
            client.net.Send(protoMessage);
            var reply = GetActionReply(Actions.GetNPCList, client);
            return (ProtoGenericArray<ProtoNPC>) reply.Result;
        }
    }
}
