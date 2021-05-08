using BepInEx;
using HarmonyLib;
using System.IO;

namespace BugFables.SaveRedirection
{
  [BepInPlugin("com.aldelaro5.BugFables.plugins.SaveRedirection", "Save Redirection", "1.0.0")]
  [BepInProcess("Bug Fables.exe")]
  public class SaveRedirectionPlugin : BaseUnityPlugin
  {
    public void Awake()
    {
      InputIOPatches.folderRedirect = Paths.BepInExRootPath + "\\";
      var harmony = new Harmony("com.aldelaro5.BugFables.plugins.SaveRedirection");
      harmony.PatchAll(typeof(InputIOPatches));
    }
  }
}
