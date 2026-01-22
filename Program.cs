using SklepSamochodowy.Data;
using SklepSamochodowy.UI;

namespace SklepSamochodowy
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var repo = new CarPartRepository();
            var menu = new ConsoleMenu(repo);
            menu.Run();
        }
    }
}
