using System;
using System.Collections.Generic;

namespace Arms
{
  public class Parser
  {
    private static Dictionary<string,string> termTypes = new Dictionary<string,string> {{"argent", "tincture"},
                                                                                        {"gules", "tincture"},
                                                                                        {"sable", "tincture"},
                                                                                        {"vert", "tincture"},
                                                                                        {"per", "division"},
                                                                                        {"fess", "division"},
                                                                                        {"pale", "division"},
                                                                                        {"quarterly", "division"}};
    private static Stack<Division> divStack = new Stack<Division>();
    public static void Parse(string blazonString, Division div)
    {
      divStack.Push(div);
      string[] blazon = blazonString.Split(' ');
      string commandType = termTypes[blazon[0]];
      List<string> command = new List<string> {};
      for(int i=0; i<blazon.Length; i++)
      {
        Console.WriteLine("Start "+blazon[i]);
        string termType;
        bool inDictionary = termTypes.TryGetValue(blazon[i], out termType);
        termType = inDictionary ? termType : "ordinary";
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
              Console.WriteLine(divStack.Count);
              ToPop = true;
              Console.WriteLine(divStack.Count);
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
          Console.WriteLine(string.Join(" ",command));
          Console.WriteLine(commandType);
          Division[] newDivisions = divStack.Peek().ExecuteCommand(command, commandType);
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
          ToPop = false;
        }
      }
      Console.WriteLine(string.Join(" ",command));
      Console.WriteLine(commandType);
      divStack.Peek().ExecuteCommand(command, commandType);
    }
  }
}
