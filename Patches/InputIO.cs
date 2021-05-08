using HarmonyLib;
using InputIOManager;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace BugFables.SaveRedirection
{
  public class InputIOPatches
  {
    public static string folderRedirect { get; set; }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InputIO), nameof(InputIO.CreateFile))]
    static bool RedirectCreateFile(ref string path, string content)
    {
      path = folderRedirect + path;
      return true;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(InputIO), nameof(InputIO.ReadFile))]
    [HarmonyPatch(typeof(InputIO), nameof(InputIO.DeleteFile))]
    static bool RedirectReadDelete(ref string path)
    {
      path = folderRedirect + path;
      return true;
    }
    
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(InputIO), nameof(InputIO.LoadSettings))]
    static IEnumerable<CodeInstruction> RedirectConfig(IEnumerable<CodeInstruction> instructions)
    {
      var codeMatch = new CodeMatcher(instructions);
      codeMatch = codeMatch.MatchForward(false, new CodeMatch(OpCodes.Ldstr, "config.dat"));
      codeMatch = codeMatch.Repeat(matcher =>
      {
        matcher.SetOperandAndAdvance(folderRedirect + matcher.Operand);
      });
      return codeMatch.InstructionEnumeration();
    }
    
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(InputIO), nameof(InputIO.Save))]
    [HarmonyPatch(typeof(InputIO), nameof(InputIO.SaveExists))]
    static IEnumerable<CodeInstruction> RedirectSaves(IEnumerable<CodeInstruction> instructions)
    {
      var codeMatch = new CodeMatcher(instructions);
      codeMatch = codeMatch.MatchForward(false, new CodeMatch(OpCodes.Ldstr, "save"));
      codeMatch = codeMatch.Repeat(matcher =>
      {
        matcher.SetOperandAndAdvance(folderRedirect + matcher.Operand);
      });
      return codeMatch.InstructionEnumeration();
    }
  }
}
