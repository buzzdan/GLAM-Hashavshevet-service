using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GlamImporter
{
    public static class Extentions
    {
        public static bool TestConnection(this DbConnection connection)
        {
            try
            {
                var builder = new DbConnectionStringBuilder
                {
                    ConnectionString = connection.ConnectionString

                };
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
namespace System
{
    public static class Extentions
    {
        public static string CleanInvalidXmlChars(this string text)
        {
            // From xml spec valid chars: 
            // #x9 | #xA | #xD | [#x20-#xD7FF] | [#xE000-#xFFFD] | [#x10000-#x10FFFF]     
            // any Unicode character, excluding the surrogate blocks, FFFE, and FFFF. 
            string re = @"[^\x09\x0A\x0D\x20-\uD7FF\uE000-\uFFFD\u10000-u10FFFF]";
            return Regex.Replace(text, re, "");
        }
    }
}
