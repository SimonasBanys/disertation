using System;
using System.Collections.Generic;
using System.Net;
using hist_mmorpg;
using System.Threading;
using TestClientROry;

namespace TestClientRory
{
    class Program
    {
        private static WordRecogniser _wordRecogniser;
        private static TextTestClient _testClient;
        private static DisplayResults _displayResults;

        private static void Main(string[] args)
        {
            var encryptString = "_encrypted_";
            string datePatern = "dd_MM_H_mm_ss";
            Client c;
            TextTestClient client = new TextTestClient();
            _displayResults = new DisplayResults();
            var logFilePath = "TestRun_NoSessions" + encryptString + DateTime.Now.ToString(datePatern) +".txt";

            //Globals_Game.pcMasterList.Add("rory", new PlayerCharacter());
            using (Globals_Server.LogFile = new System.IO.StreamWriter(logFilePath))
            {
                _wordRecogniser = new WordRecogniser();
                _testClient = new TextTestClient();
                Console.Clear();
                LogInPrompt();
                while (_testClient.IsConnectedAndLoggedIn() == false)
                {
                    Thread.Sleep(0);
                }
                var command = TokenizeConsoleEntry();
                ProcessCommand(_wordRecogniser.CheckWord(command[0]), command);
                ProcessCommand(WordRecogniser.Tasks.Check, command);
                while (_wordRecogniser.CheckWord(command[0]) != WordRecogniser.Tasks.Exit)
                {
                    command = TokenizeConsoleEntry();
                    ProcessCommand(_wordRecogniser.CheckWord(command[0]), command);
                    Globals_Server.LogFile.WriteLine(command[0]); // to log all the tasks attempted by the user before exit or server/client crash
                }
                Shutdown();
            }
        }

        private static void LogInPrompt()
        {
            Console.WriteLine("Welcome to the JominiEngine Text Client! Please enter your username and password");
            Console.Write("What is your username: ");
            var usernameForReturn = Console.ReadLine();
            Console.Write("What is your password: ");
            var passwordForReturn = Console.ReadLine();
            Console.Write("What is the Server's IP Address?: ");
            var ipForReturn = Console.ReadLine();
            try
            {
                IPAddress.Parse(ipForReturn);
            }
            catch
            {
            }
            _testClient.LogInAndConnect(usernameForReturn, passwordForReturn, ipForReturn);
            Console.Clear();
        }

        private static void Shutdown()
        {
            _testClient.LogOut();
        }

        private static List<string> TokenizeConsoleEntry()
        {
            var command = Console.ReadLine();
            var commands = new List<string>();
            if (command != null)
            {
                foreach (var item in command.Split(' '))
                {
                    commands.Add(item);
                }
            }
            else
            {
                return null;
            }
            return commands;
        }

        public static void ProcessCommand(WordRecogniser.Tasks task, List<String> arguments)
        {
            WordRecogniser wordRecogniser = new WordRecogniser();
            PlayerOperations player = new PlayerOperations();
            switch (task)
            {
                case WordRecogniser.Tasks.ArmyStatus:
                    var armyStatusResult = player.ArmyStatus(_testClient);
                    _displayResults.DisplayArmyStatus(armyStatusResult);
                    break;
                case WordRecogniser.Tasks.ChangeAttackSupport:
                    {
                        if (ValidateArgs(arguments))
                        {
                            var armyStatusRes = player.changeAttack(arguments[1], _testClient);
                            _displayResults.DisplayChangeAtt(armyStatusRes);
                        }
                        break;
                    }
                case WordRecogniser.Tasks.ChangeDefenceSupport:
                    {
                        if (ValidateArgs(arguments))
                        {
                            var armyStatusRes = player.changeDefence(arguments[1], _testClient);
                            _displayResults.DisplayChangeDef(armyStatusRes);
                        }
                        break;
                    }
                case WordRecogniser.Tasks.ChangeAutoPillage:
                    {
                        if (ValidateArgs(arguments))
                        {
                            var armyStatusRes = player.changePillage(arguments[1], _testClient);
                            _displayResults.DisplayChangePilllage(armyStatusRes);
                        }
                        break;
                    }
                case WordRecogniser.Tasks.Check:
                    var checkResult = player.Check(_testClient);
                    _testClient.charID = _displayResults.DisplayCheck(checkResult);
                    break;
                case WordRecogniser.Tasks.SendAssassin:
                    {
                        var assassinResult = player.sendAssassin(arguments[1], arguments[2], _testClient);
                        _displayResults.DisplaySendAssassin(assassinResult);
                        break;
                    }
                case WordRecogniser.Tasks.Fief:
                    var fiefResult = player.FiefDetails(_testClient);
                    _displayResults.DisplayFief(fiefResult);
                    break;
                case WordRecogniser.Tasks.Help:
                    _displayResults.Help();
                    break;
                case WordRecogniser.Tasks.Move:
                    if (ValidateArgs(arguments))
                    {
                        var moveResult = player.Move(wordRecogniser.CheckDirections(arguments[1]),
                            _testClient);
                        _displayResults.DisplayMove(moveResult);
                    }
                    else
                    {
                        SyntaxError();
                    }
                    break;
                case WordRecogniser.Tasks.Hire:
                    if (ValidateArgs(arguments))
                    {
                        var hireResult = player.HireTroops(Convert.ToInt32(arguments[1]), _testClient);
                        _displayResults.DisplayHire(hireResult);
                        var armyUpdate = player.ArmyStatus(_testClient);
                        _displayResults.DisplayArmyStatus(armyUpdate);
                    }
                    break;

                case WordRecogniser.Tasks.HireNew:
                    {
                        if (ValidateArgs(arguments))
                        {
                            var hireResult = player.HireNew(Convert.ToInt32(arguments[1]), _testClient);
                            _displayResults.DisplayHire(hireResult);
                            var armyUpdate = player.ArmyStatus(_testClient);
                            _displayResults.DisplayArmyStatus(armyUpdate);
                        }
                        break;
                    }
                case WordRecogniser.Tasks.Siege:
                    var siegeResult = player.SiegeCurrentFief(_testClient);
                    if (siegeResult.GetType() == typeof(ProtoSiegeDisplay))
                    {
                        var siegeDisplay = (ProtoSiegeDisplay)siegeResult;
                        _displayResults.DisplaySiege(siegeDisplay);

                    }
                    else
                    {
                        switch (siegeResult.ResponseType)
                        {
                            case DisplayMessages.PillageSiegeAlready:
                                Console.WriteLine("Already sieged this turn!");
                                break;
                            case DisplayMessages.PillageUnderSiege:
                                Console.WriteLine("Already under siege!");
                                break;
                            case DisplayMessages.ArmyNoLeader:
                                Console.WriteLine("Army has no leader!");
                                break;
                            default:
                                Console.WriteLine(siegeResult.ResponseType);
                                break;
                        }
                    }
                    break;
                case WordRecogniser.Tasks.AttackArmy:
                    {
                        var attackResult = player.AttackArmy(arguments[1], _testClient);
                        var attackDisplay = (ProtoBattle)attackResult;
                        _displayResults.DisplayAttack(attackDisplay);
                        break;
                    }
                case WordRecogniser.Tasks.Players:
                    var playersResult = player.Players(_testClient);
                    _displayResults.DisplayPlayers(playersResult);
                    break;
                case WordRecogniser.Tasks.Profile:
                    var profileResult = player.Profile(_testClient);
                    _displayResults.DisplayProfile(profileResult);
                    break;
                case WordRecogniser.Tasks.SeasonUpdate:
                    var seasonUpdateResult = player.SeasonUpdate(_testClient);
                    _displayResults.DisplaySeasonUpdate();
                    break;
                case WordRecogniser.Tasks.Sieges:
                    var siegeListResult = player.SiegeList(_testClient);
                    _displayResults.DisplaySiegeResult(siegeListResult);
                    break;
                case WordRecogniser.Tasks.JournalEntries:
                    var journalEntriesResult = player.JournalEntries(_testClient);
                    _displayResults.DisplayJournalEntries(journalEntriesResult);
                    break;
                case WordRecogniser.Tasks.Journal:
                    if (ValidateArgs(arguments))
                    {
                        var journalResult = player.Journal(arguments[1], _testClient);
                        _displayResults.DisplayJournalEntry(journalResult);
                    }
                    else
                    {
                        SyntaxError();
                    }
                    break;
                case WordRecogniser.Tasks.FiefExpenditure:
                    if (ValidateArgs(arguments))
                    {
                        var fiefExpendResult = player.AdjustFiefExpenditure(arguments[1], _testClient);
                    }
                    else
                    {
                        SyntaxError();
                    }
                    break;
                case WordRecogniser.Tasks.Exit:
                    break;
            }
            Console.WriteLine();
        }

        static bool ValidateArgs(List<String> argumentList)
        {
            var counter = 0;
            foreach (var argument in argumentList)
            {
                counter++;
            }
            if (counter >= 2)
            {
                return true;
            }
            return false;
        }

        static void SyntaxError()
        {
            Console.WriteLine("Syntax Error: No argument provided");
        }
    }
}
