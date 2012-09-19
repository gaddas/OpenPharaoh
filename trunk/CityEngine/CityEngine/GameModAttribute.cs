// -----------------------------------------------------------------------
// <copyright file="GameModAttribute.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace CityEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Game Mod attribtue identificator
    /// </summary>
    [AttributeUsage(System.AttributeTargets.Class)]
    public class GameModAttribute : Attribute
    {
        public string name;
        public string version;

        public GameModAttribute(string name, string version)
        {
            this.name = name;
            this.version = version;
        }
    }
}
