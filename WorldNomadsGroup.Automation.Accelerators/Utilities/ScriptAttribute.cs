using System;
using System.Collections.Generic;

namespace WorldNomadsGroup.Automation.Accelerators
{
    public enum RunOption { None, Script, Setup, Both }

    /// <summary>
    /// Represents the attribute to define script properties for a test script/class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class ScriptAttribute : Attribute
    {
        /// <summary>
        /// Option whether to run the script, setup method, or both
        /// </summary>
        public RunOption RunOption { get; set; }

        /// <summary>
        /// The name of the script
        /// </summary>
        public string ScriptName { get; set; }

        /// <summary>
        /// The name of the script
        /// </summary>
        public string ModuleName { get; set; }

        /// <summary>
        /// The description of the script
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Categories for selection or grouping of tests by the suite runner
        /// </summary>
        public string Categories { get; set; }


        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="scriptModuleName">Script Module name - </param>
        /// <param name="scriptName">Script name - overrrides script class name</param>
        /// <param name="description">Description - placed in report</param>
        /// <param name="categories">Categories - can define a comma delimted set of categories selected by suite runner to determine scripts to run</param>       
        public ScriptAttribute(string scriptModuleName="",string scriptName = "", string description = "", string categories = "")
        {
            this.ModuleName = scriptModuleName;
            this.ScriptName = scriptName;
            this.Description = description;
            this.Categories = categories;
        }

    }
}

