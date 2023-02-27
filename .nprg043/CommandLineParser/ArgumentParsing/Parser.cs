using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentParsing
{
    public class Parser
    {
        protected OptionSet options;
        public Parser(OptionSet optionSet)
        {
            this.options = optionSet;
        }
        public bool ParseCommandLine(string [] args)
        {
            throw new NotImplementedException();
        }

        public void GetPlainParameters()
        {
            throw new NotImplementedException();

        }
    }

    

    /// <summary>
    /// The OptionSet class is used to define the set of options that can be parsed by the Parser class.
    /// 
    /// <Remarks>If two or more options with the same identifier are added to the OptionSet, Find method coud behave undeterminisically.</Remarks>
    /// </summary>
    public class OptionSet
    {
        private Dictionary<string, Option> longOptions = new();
        private Dictionary<char, Option> shortOptions = new();

        /// <summary>
        /// Adds the option to the OptionSet.
        /// </summary>
        /// <param name="option"><see cref="Option"> instance to be added.</param>
        /// <returns>True </returns>
        public bool Add(Option option)
        {
            throw new NotImplementedException();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionIdentifier"></param>
        /// <returns></returns>
        public Option Find(string optionIdentifier)
        {
            throw new NotImplementedException();
        }

       
        
    }



}
