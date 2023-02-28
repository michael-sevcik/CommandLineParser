using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentParsing
{
    /// <summary>
    /// Abstract base class implementation.
    /// </summary>
    public abstract class Option
    {
        /// <summary>
        /// Determines whether a given option must occur in a parsed command. 
        /// </summary>
        public bool IsMandatory { get; init; }

        /// <summary>
        /// Determines whether a given option may, or must have parameters.
        /// </summary>
        public bool IsParametrized { get; init; }
    }

    /// <summary>
    /// Instance class of options with no parameters.
    /// </summary>
    public class NoParameterOption : Option
    {
        Action action;

        /// <summary>
        /// Constructs instance of <see cref="NoParameterOption"/>.
        /// </summary>
        /// <param name="isMandatory">Determines whether a given option must occur in a parsed command.</param>
        /// <param name="action">Encapsulated method to call, when the option occurs in the parsed command.</param>
        public NoParameterOption(bool isMandatory, Action action)
        {
            this.IsMandatory = isMandatory;
            this.action = action;
        }

        /// <summary>
        /// Method to call when option occurs in the parsed command-line.
        /// </summary>
        public void TakeAction()
        {
            action();
        }
    }

    /// <summary>
    /// Abstract class for parametrized options.
    /// </summary>
    public abstract class ParameterOption : Option
    {
        /// <summary>
        /// Determines whether a option requires parameter.
        /// </summary>
        public bool IsParameterMandatory { get; init; }

        /// <summary>
        /// Parses the parameter.
        /// </summary>
        /// <param name="param">Parameter which followed the option.</param>
        /// <returns>True if parsing was successful, otherwise false.</returns>
        public abstract bool TryParse(string param);
    }

    
    public abstract class MultipleParameterOption : ParameterOption
    {
        /// <summary>
        /// Delimits multiple parameters entries. 
        /// </summary>
        public char Delimiter { get; init; }
    }


    /// <summary>
    /// This class represents option, which takes 0 to 1 int parameters based on is mandatory field.
    /// </summary>
    public class IntOption : ParameterOption
    {
        Action<int?> saveAction;

        /// <summary>
        /// Constructs an instance of <see cref="IntOption"/>.
        /// </summary>
        /// <param name="action">Specifices action, wihich should be executed with int option parameter or null if 
        /// mandatory is set to false.
        /// </param>
        /// <param name="mandatory"> Specifices wether the option requires at least one parameter present on
        /// command line.
        /// </param>
        public IntOption(Action<int?> action, bool mandatory)
        {
            this.saveAction = action;
            this.IsMandatory = mandatory;
            
        }
       
       
        public override bool TryParse(string param)
        {
            throw new NotImplementedException();
        }

    }

    
    /// <summary>
    /// This class represents option, which takes 0-1(based on mandatory field) to unlimited int parameters.
    /// </summary>
    public class MultipleIntOption : MultipleParameterOption
    {
        Action<int[]?> saveAction;
        /// <summary>
        /// Creates an instance of <see cref="MultipleIntOption"/>.
        /// </summary>
        /// <param name="action">Specifices action, which should be executed with int parameters or null if
        /// there were no parameters present on command line.
        /// </param>
        /// <param name="mandatory"> Specifices wether the option requires at least one parameter present on
        /// command line.
        /// </param>
        public MultipleIntOption(Action<int[]?> action, bool mandatory)
        {
            this.saveAction = action;
            this.IsMandatory = mandatory;

        }
        public override bool TryParse(string param)
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// This class represents option, which takes 0 to 1 string arguments based on mandatory field.
    /// </summary>
    public class StringOption : ParameterOption
    {
        Action<string?> saveAction;

        /// <summary>
        /// Creates an instance of <see cref="StringOption"/>.
        /// </summary>
        /// <param name="action">Specifices action, which should be exectued with string parameter
        /// or null if there was no parameter present on command line.
        /// </param>
        /// <param name="mandatory"> Specifices wether the option requires at least one parameter present on
        /// command line.
        /// </param>
        public StringOption(Action<string?> action, bool mandatory)
        {
            this.saveAction = action;
            this.IsMandatory = mandatory;

        }

        public override bool TryParse(string param)
        {
            throw new NotImplementedException();
        }

    }
    /// <summary>
    /// This class represents option, which takes 0-1(based on is mandatory field) to unlimited string options.
    /// </summary>
    public class MultipleStringOption : MultipleParameterOption
    {
        Action<string[]?> saveAction;
        /// <summary>
        /// Creates an instance of <see cref="MultipleStringOption"/>.
        /// </summary>
        /// <param name="action">Specifices action, which should be executed with string parameters or null if
        /// there were no parameters present on command line.
        /// </param>
        /// <param name="mandatory"> Specifices wether the option requires at least one parameter present on
        /// command line.
        /// </param>
        public MultipleStringOption(Action<string[]?> action, bool mandatory)
        {
            this.saveAction = action;
            this.IsMandatory = mandatory;

        }
        public override bool TryParse(string param)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// This class represents option, which takes 0 to 1 bool arguments based on mandatory field.
    /// </summary>
    class BoolOption : ParameterOption
    {
        private Action<bool> saveAction;
        /// <summary>
        /// Creates an instance of <see cref="BoolOption"/>.
        /// </summary>
        /// <param name="action">Specifices action, which should be exectued with bool parameter
        /// or null if there was no parameter present on command line.
        /// </param>
        /// <param name="mandatory"> Specifices wether the option requires at least one parameter present on
        /// command line.
        /// </param>
        public BoolOption(Action<bool> action, bool mandatory)
        {
            this.saveAction = action;
            this.IsMandatory = mandatory;
        }
        public override bool TryParse(string param)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// This class represents option, which takes 0-1(based on is mandatory field) to unlimited bool options.
    /// </summary>
    class MultipleBoolOption : ParameterOption
    {
        private Action<bool[]?> saveAction;
        /// <summary>
        /// Creates an instance of <see cref="MultipleBoolOption"/>.
        /// </summary>
        /// <param name="action">Specifices action, which should be executed with bool parameters or null if
        /// there were no parameters present on command line.
        /// </param>
        /// <param name="mandatory"> Specifices wether the option requires at least one parameter present on
        /// command line.
        /// </param>
        public MultipleBoolOption(Action<bool[]?> action, bool mandatory)
        {
            this.saveAction = action;
            this.IsMandatory = mandatory;
        }
        public override bool TryParse(string param)
        {
            throw new NotImplementedException();
        }
    }

    enum OptionType
    {
        NoParameter,
        Int,
        MultipleInt,
        String,
        MultipleString,
        Bool,
        MultipleBool
    }    
    
    public class EnumOption <T> : ParameterOption where T : Enum
    {
        Action<T> saveAction;

        // TODO: Consider using default value instead of nullable type.
        public EnumOption(Action<T> action, bool mandatory)
        {
            this.saveAction = action;
            this.IsMandatory = mandatory;
            Enum.GetNames(typeof(T));
        }

        public override bool TryParse(string param)
        {
            throw new NotImplementedException();
        }
    }

    public class MultipleEnumOption<T> : ParameterOption where T : Enum
    {
        Action<T[]> saveAction;

        // TODO: Consider using default value instead of nullable type.
        public MultipleEnumOption(Action<T[]> action, bool mandatory)
        {
            this.saveAction = action;
            this.IsMandatory = mandatory;
        }

        public override bool TryParse(string param)
        {
            throw new NotImplementedException();
        }
    }

    public static class OptionFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// // TODO: 
        /// <param name="line"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Option CreateOption(string line)
        {
            throw new NotImplementedException();
        }

        
    }

    // TODO: consider array options.
}
