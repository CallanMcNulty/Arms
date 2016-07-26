using System;
using System.Collections.Generic;

namespace Arms
{
  public class Parser
  {
    private static string[] hyphenate = new string[] {"bend sinister", "pall reversed", "blue celeste"};
    private static Dictionary<string, string> numberTable = new Dictionary<string, string> {{"one", "1"},
                                                                                  {"two", "2"},
                                                                                  {"three", "3"},
                                                                                  {"four", "4"},
                                                                                  {"five", "5"},
                                                                                  {"six", "6"},
                                                                                  {"seven", "7"},
                                                                                  {"eight", "8"},
                                                                                  {"nine", "9"},
                                                                                  {"ten", "10"},
                                                                                  {"eleven", "11"},
                                                                                  {"twelve", "12"},
                                                                                  {"thirteen", "13"},
                                                                                  {"fourteen", "14"},
                                                                                  {"fifteen", "15"},
                                                                                  {"sixteen", "16"},
                                                                                  {"seventeen", "17"},
                                                                                  {"eighteen", "18"},
                                                                                  {"nineteen", "19"},
                                                                                  {"twenty", "20"}};
    private static Dictionary<string,string> termTypes = new Dictionary<string,string> {
        {"argent", "tincture"},
        {"gules", "tincture"},
        {"sable", "tincture"},
        {"vert", "tincture"},
        {"per", "division"},
        {"fess", "ordinary"},
        {"pale", "ordinary"},
        {"bend", "ordinary"},
        {"saltire", "ordinary"},
        {"cross", "ordinary"},
        {"chevron", "ordinary"},
        {"pall", "ordinary"},
        {"quarterly", "division"}
      };
    private static Stack<Division> divStack = new Stack<Division>();
    public static void Parse(string blazonString, Division div)
    {
      divStack.Push(div);
      string[] blazon = FormatBlazon(blazonString);
      string commandType = termTypes[blazon[0]];
      List<string> command = new List<string> {};
      bool modifyingCharge = false;
      for(int i=0; i<blazon.Length; i++)
      {
        // Console.WriteLine("Start "+blazon[i]);
        string termType;
        bool inDictionary = termTypes.TryGetValue(blazon[i], out termType);
        termType = inDictionary ? termType : "charge";
        if(termType=="ordinary")
        {
          termType = commandType=="division" ? "division" : "charge";
        }
        bool ToPop = false;
        if(blazon[i]=="and")
        {
          termType = "tincture";
          if(i>=2)
          {
            string oldTermType;
            inDictionary = termTypes.TryGetValue(blazon[i-2], out oldTermType);
            oldTermType = inDictionary ? oldTermType : "notTincture";
            if(oldTermType!="tincture")
            {
              // Console.WriteLine(divStack.Count);
              ToPop = true;
              // Console.WriteLine(divStack.Count);
            }
            termType = "no";
          }
        }
        if(termType==commandType)
        {
          command.Add(blazon[i]);
        }
        else
        {
          // Console.WriteLine(string.Join(" ",command));
          // Console.WriteLine(commandType);
          Division[] newDivisions = divStack.Peek().ExecuteCommand(command, commandType);
          modifyingCharge = commandType=="charge" ? true : modifyingCharge;
          Array.Reverse(newDivisions);
          foreach(Division d in newDivisions)
          {
            divStack.Push(d);
          }
          commandType = termType;
          command = new List<string> {blazon[i]};
        }
        if(ToPop)
        {
          divStack.Pop();
          if(modifyingCharge)
          {
            divStack.Pop();
            modifyingCharge = false;
          }
          ToPop = false;
        }
      }
      // Console.WriteLine(string.Join(" ",command));
      // Console.WriteLine(commandType);
      divStack.Peek().ExecuteCommand(command, commandType);
    }
    public static string[] FormatBlazon(string newBlazon)
    {
      string replacedBlazon = newBlazon.Replace(",","");

      for(int i=0; i<hyphenate.Length; i++)
      {
        if (replacedBlazon.Contains(hyphenate[i]))
        {
          string arrayWords = hyphenate[i];
          hyphenate[i].Replace(" ", "-");
          replacedBlazon.Replace(arrayWords, hyphenate[i]);
          Console.WriteLine(replacedBlazon);
        }
      }

      string[] formatBlazon = replacedBlazon.ToLower().Split(' ');
      for(int i = 0; i<formatBlazon.Length; i++)
      {
        if (!termTypes.ContainsKey(formatBlazon[i]))
        {
          formatBlazon[i] = formatBlazon[i].TrimEnd('s');
        }
        if (numberTable.ContainsKey(formatBlazon[i]))
        {
         formatBlazon[i]=numberTable[formatBlazon[i]];
        }
      }
      return formatBlazon;
    }
  }
}
