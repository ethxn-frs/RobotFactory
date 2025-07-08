using RobotFactory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotFactory.Factory
{
    public class Factory
    {
        public Robot CreateRobot(string name)
        {
            return name switch
            {
                "XM-1" => new Robot("XM-1", new List<string>
                {
                    "Core_CM1", "Generator_GM1", "Arms_AM1", "Legs_LM1", "System_SB1"
                }, "M"),
                "RD-1" => new Robot("RD-1", new List<string>
                {
                "Core_CD1", "Generator_GD1", "Arms_AD1", "Legs_LD1", "System_SB1"
                }, "D"),
                        "WI-1" => new Robot("WI-1", new List<string>
                {
                "Core_CI1", "Generator_GI1", "Arms_AI1", "Legs_LI1", "System_SB1"
                }, "I"),
                _ => throw new ArgumentException($"Robot inconnu : {name}")
            };
        }

    }

}
