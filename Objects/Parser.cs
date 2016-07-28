using System;
using System.Collections.Generic;

namespace Arms
{
  public class Parser
  {
    private static string[] hyphenate = new string[] {"bend sinister", "pall reversed", "bleu celeste"};
    private static Dictionary<string, string> numberTable = new Dictionary<string, string> {
        {"a", "1"},
        {"an", "1"},
        {"one", "1"},
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
        {"twenty", "20"}
      };
    private static Dictionary<string,string> termTypes = new Dictionary<string,string> {
        {"argent", "tincture"},
        {"gules", "tincture"},
        {"sable", "tincture"},
        {"vert", "tincture"},
        {"azure", "tincture"},
        {"purpure", "tincture"},
        {"murrey", "tincture"},
        {"cendree", "tincture"},
        {"brunatre", "tincture"},
        {"bleu-celeste", "tincture"},
        {"amaranth", "tincture"},
        {"or", "tincture"},
        {"ermine", "tincture"},
        {"vair", "tincture"},
        {"per", "division"},
        {"quarterly", "division"},
        {"fess", "ordinary"},
        {"chief", "ordinary"},
        {"pile", "ordinary"},
        {"pale", "ordinary"},
        {"bend", "ordinary"},
        {"bend-sinister", "ordinary"},
        {"saltire", "ordinary"},
        {"cross", "ordinary"},
        {"chevron", "ordinary"},
        {"pall", "ordinary"},
        {"pall-reversed", "ordinary"},
        {"lozenge", "charge"},
        {"inescutcheon", "charge"},
        {"escutcheon", "charge"},
        {"mullet", "charge"},
        {"and", "grammar"},
        {"on", "grammar"},
        {"overall", "grammar"},
        {"i", "grammar"},
        {"ii", "grammar"},
        {"iii", "grammar"},
        {"iv", "grammar"}
      };
    private static Stack<Division> divStack = new Stack<Division>();
    private static string GetFromDictionary(Dictionary<string,string> d, string searchTerm, string defaultString)
    {
      string val;
      bool inDictionary = d.TryGetValue(searchTerm, out val);
      val = inDictionary ? val : defaultString;
      return val;
    }
    private static string GetTermType(string[] blazon, int index, string commandType)
    {
      string termType = GetFromDictionary(termTypes, blazon[index], "none");
      // Term Type Exceptions
      if(termType=="ordinary")
      {
        termType = "charge";
        if(blazon[index]=="fess" || blazon[index]=="pale" || blazon[index]=="bend" || blazon[index]=="bend-sinister")
        {
          termType = commandType=="division" ? "division" : "charge";
        }
      }
      if(blazon[index]=="and")
      {
        if(index>=2)
        {
          string oldTermType = GetFromDictionary(termTypes, blazon[index-2], "notTincture");
          if(oldTermType=="tincture")
          {
            termType = "tincture";
          }
        }
      }
      if(termType=="none")
      {
        if(IsNumber(blazon[index]))
        {
          if(blazon[index-1]=="of")
          {
            termType = "tincture";
          }
          else
          {
            termType = "charge";
          }
        }
      }
      return termType;
    }
    private static bool IsNumber(string str)
    {
      foreach(char c in str)
      {
        if(c < '0' || c > '9')
        {
          return false;
        }
      }
      return true;
    }
    private static bool TryPop()
    {
      if(divStack.Count>1)
      {
        divStack.Pop();
        return true;
      }
      return false;
    }
    private static bool usingOn;
    private static int ExecuteCommand(List<string> command, string commandType, int modifyingCharge)
    {
      if(modifyingCharge==-1){return modifyingCharge;}
      Console.WriteLine("Charges: {0} "+commandType,modifyingCharge);
      if(commandType=="grammar" && command[0]!="i" && command[0]!="on")
      {
        if(!TryPop()){return -1;}
        if(divStack.Peek().subdivisions.Length > 0 && command[0] != "overall")
        {
          if(!TryPop()){return -1;}
        }
      }
      else if(command[0]=="on")
      {
        usingOn = true;
      }
      else if(commandType=="tincture")
      {
        if(command.Count > 1){return -1;}
        if(command[0]=="and"){return -1;}
        if(modifyingCharge==0)
        {
          divStack.Peek().ExecuteCommand(command, commandType);
        }
        while(modifyingCharge > 0)
        {
          divStack.Peek().ExecuteCommand(command, commandType);
          if(!usingOn)
          {
            if(!TryPop()){return -1;}
          }
          else
          {
            usingOn = false;
          }
          modifyingCharge -= 1;
        }
      }
      else
      {
        if(commandType=="charge")
        {
          if(command.Count != 2){return -1;}
          int num;
          if(!Int32.TryParse(command[0], out num)){return -1;}
          int num2;
          if(Int32.TryParse(command[1], out num2)){return -1;}
          modifyingCharge += num;
        }
        if(commandType=="division")
        {
          if(command.Count != 2 && command[0]!="quarterly"){return -1;}
          if(command[0]!="quarterly"){if(termTypes[command[1]]!="ordinary"){return -1;}}
        }
        Division[] newDivisions = divStack.Peek().ExecuteCommand(command, commandType);
        Array.Reverse(newDivisions);
        foreach(Division d in newDivisions)
        {
          divStack.Push(d);
        }
      }
      Console.WriteLine(divStack.Count);
      return modifyingCharge;
    }
    private static bool AllTermsInDict(string[] blazon)
    {
      foreach(string term in blazon)
      {
        int num;
        bool isNum = Int32.TryParse(term, out num);
        if(isNum && term==blazon[0])
        {
          return false;
        }
        if(!termTypes.ContainsKey(term) && !isNum)
        {
          return false;
        }
      }
      return true;
    }
    public static string Parse(string blazonString, Division div)
    {
      Console.WriteLine("---NEW ARMS BEGIN---");
      divStack.Clear();
      divStack.Push(div);
      string[] blazon = FormatBlazon(blazonString);
      Console.WriteLine(string.Join(" ",blazon));
      Console.WriteLine("on {0}", usingOn);
      if(!AllTermsInDict(blazon))
      {
        return "Not all words are recognized";
      }
      string commandType = termTypes[blazon[0]];
      List<string> command = new List<string> {};
      int modifyingCharge = 0;
      usingOn = false;
      for(int i=0; i<blazon.Length; i++)
      {
        if(blazon[i]=="quarterly" && i!=0)
        {
          return "Only one quartering allowed, use 'per pale' and 'per fess' instead";
        }
        command.Add(blazon[i]);
        // Get Term Type
        string termType = GetTermType(blazon, i, commandType);
        Console.WriteLine("Stack Size: {0}",divStack.Count);
        Console.WriteLine(blazon[i]+": "+termType);
        commandType = commandType=="none" ? termType : commandType;
        // Check for command completeness
        bool complete = false;
        if(i==blazon.Length-1)
        {
          complete = true;
        }
        else if(termType=="division" && (command.Count==2 || blazon[i]=="quarterly"))
        {
          complete = true;
        }
        else if(blazon[i]=="on" || blazon[i+1]=="on")
        {
          complete = true;
        }
        else
        {
          string nextTermType = GetTermType(blazon, i+1, commandType);
          complete = nextTermType!=commandType ? true : complete;
        }

        // Execute command if complete
        if(complete)
        {
          Console.WriteLine("Executing: "+string.Join(" ",command));
          modifyingCharge = ExecuteCommand(command, commandType, modifyingCharge);
          if(modifyingCharge==-1){return "Problem at word "+i.ToString();}
          commandType = "none";
          command = new List<string> {};
        }
        Console.WriteLine("Finished with Term");
      }
      Console.WriteLine("---------");
      return "";
    }

    public static string[] FormatBlazon(string newBlazon)
    {
      string replacedBlazon = newBlazon.Replace(",","");

      for(int i=0; i<hyphenate.Length; i++)
      {
        if (replacedBlazon.Contains(hyphenate[i]))
        {
          string arrayWords = hyphenate[i].Replace(" ", "-");
          replacedBlazon = replacedBlazon.Replace(hyphenate[i], arrayWords);
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
