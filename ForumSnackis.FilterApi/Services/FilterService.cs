using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSnackis.FilterApi.Services
{
    public class FilterService
    {
        private HashSet<string> Bannedwords;

        public FilterService()
        {
            Bannedwords = new();
            Bannedwords.Add("kuk");
        }

        public string CheckString(string content)
        {
            char [] delims = { ' ', '.', ';' };
            var toCheck = content.ToLower().Split(delims);
            string result = content;
            foreach (var c in toCheck)
            {
                if (Bannedwords.Contains(c))
                {
                    result = result.Replace(c, "*****",StringComparison.OrdinalIgnoreCase);
                }               
            }
            return result;
        }
    }
}
