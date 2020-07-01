using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.ClearScript.V8;

namespace HRR
{
    class Program
    {
        private static V8ScriptEngine v8;

        [STAThread]
        static void Main(string[] args)
        {
            v8 = new V8ScriptEngine();
            ConfigureJavaScriptEngine(ref v8);
            Console.Write("Content-Type: text/html\n\n");
            if (args.Length > 0)
            {
                if (File.Exists(args[0])) 
                    ParseScript(args[0]);
            }
        }

        private static void ConfigureJavaScriptEngine(ref V8ScriptEngine v8)
        {
            
        }

        private static void ParseScript(string v)
        {
            var script = File.ReadAllText(v);
            var pattern = new Regex("<{(.*?)}>", RegexOptions.Singleline | RegexOptions.Compiled);
            var matches = pattern.Matches(script);
            foreach (Match match in matches)
            {
                if (match.Success)
                {
                    var expression = match.Groups[1].Value;
                    var value = v8.Evaluate(expression);
                    script = script.Replace(match.Captures[0].Value.ToString(), value.ToString());
                }
            }
            Console.Write(script);
        }
    }
}
