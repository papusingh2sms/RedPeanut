﻿//
// Author: B4rtik (@b4rtik)
// Project: RedPeanut (https://github.com/b4rtik/RedPeanut)
// License: BSD 3-Clause
//

using System;
using System.Collections.Generic;
using System.Linq;
using static RedPeanut.Utility;

namespace RedPeanut
{
    class RubeusMonitorManager : IMenu
    {
        public static Dictionary<string, string> mainmenu = new Dictionary<string, string>
        {
            { "set interval", "Set interval in seconds, default 60" },
            { "set filteruser", "Set result filtered by user" },
            { "set registry", "Set softwarename" },
            { "run", "Execute module" },
            { "options", "Print current config" },
            { "info", "Print help" },
            { "back", "Back to lateral menu" }
        };

        public void RePrintCLI()
        {
            Utility.RePrintCLI(agent, modulename);
            return;
        }

        IAgentInstance agent = null;
        string modulename = "monitor";
        int interval = 60;
        string filteruser;
        string registry;

        public RubeusMonitorManager()
        {

        }

        public RubeusMonitorManager(IAgentInstance agent)
        {
            this.agent = agent;
        }

        public void Execute()
        {
            string input;
            SetAutoCompletionHandler(mainmenu);
            do
            {
                input = RedPeanutCLI(agent, modulename);
                RubeusMonitorMenu(input);
            } while (input != "back");
        }

        private void RubeusMonitorMenu(string input)
        {
            string[] a_input = input.Split(' ');
            string f_input = ParseSelection(input);

            if (!string.IsNullOrEmpty(input))
            {
                if (mainmenu.ContainsKey(f_input.TrimEnd()))
                {
                    switch (f_input.TrimEnd())
                    {
                        case "set interval":
                            interval = GetParsedSetInt(input);
                            break;
                        case "set filteruser":
                            filteruser = GetParsedSetString(input);
                            break;
                        case "set registry":
                            registry = GetParsedSetString(input);
                            break;
                        case "run":
                            Run();
                            break;
                        case "options":
                            PrintCurrentConfig();
                            break;
                        case "info":
                            PrintOptions("info", mainmenu);
                            break;
                        case "back":
                            Program.GetMenuStack().Pop();
                            return;
                        default:
                            Console.WriteLine("We had a woodoo");
                            break;
                    }
                }
                else
                {
                    PrintOptions("Command not found", mainmenu);
                }
            }
        }
        
        private void Run()
        {
            List<string> args = new List<string>();
            args.Add("monitor");

            if (interval != 0)
            {
                args.Add("/interval:" + interval);
            }

            if (!string.IsNullOrEmpty(registry))
            {
                args.Add("/registry:" + registry);
            }

            if (!string.IsNullOrEmpty(filteruser))
            {
                args.Add("/filteruser:" + filteruser);
            }

            RunAssembly(PL_MODULE_RUBEUS, "Rubeus.Program", args.ToArray<string>(), agent);

        }

        private void PrintCurrentConfig()
        {
            Dictionary<string, string> properties = new Dictionary<string, string>
            {
                { "interval", interval.ToString() },
                { "registry", registry },
                { "filteruser", filteruser }
            };

            Utility.PrintCurrentConfig(modulename, properties);
        }
    }
}

