using System;

namespace CityEngine
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            //var e = new OpenPharaoh.Files.TextENG(@"C:\Users\bbdnet6039\Downloads\OpenPharaoh\Pharaoh - Cleopatra\Pharaoh_Text.eng");

#if WINDOWS || XBOX

            using (GameMain game = new GameMain())
            {
                game.Run();
            }
#endif
        }
    }
}

