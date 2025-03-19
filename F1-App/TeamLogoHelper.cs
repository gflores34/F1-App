using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace F1_App
{
    internal class TeamLogoHelper
    {

        private static Dictionary<string, string> teamLogos = new Dictionary<string, string>()
        {
            { "Alpine", "/images/logos/alpine-logo.png" },
            { "Aston Martin", "/images/logos/aston-martin-logo.png" },
            { "Ferrari", "/images/logos/ferrari-logo.png" },
            { "Haas F1 Team", "/images/logos/haas-logo.png" },
            { "Kick Sauber", "/images/logos/kick-sauber-logo.png" },
            { "McLaren", "/images/logos/mclaren-logo.png" },
            { "Mercedes", "/images/logos/mercedes-logo.png" },
            { "Racing Bulls", "/images/logos/racing-bulls-logo.png" },
            { "Red Bull Racing", "/images/logos/red-bull-racing-logo.png" },
            { "Williams", "/images/logos/williams-logo.png" }
        };

        public static string GetTeamLogo(string teamName)
        {
            if (teamLogos.ContainsKey(teamName))
            {
                return teamLogos[teamName];
            }
            else
            {
                return "/images/logos/no-image.png";
            }
        }
    }
}
