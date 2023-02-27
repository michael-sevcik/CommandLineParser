using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArgumentParsing
{
    
    public abstract class Option
    {
       
        public bool IsMandatory { get; init; }

        public bool IsParametrized { get; init; }
    }
    public class NoParameterOption : Option
    {
        Action action;
        public NoParameterOption(bool isMandatory, Action action)
        {
            this.IsMandatory = isMandatory;
            this.action = action;
        }

        public void TakeAction()
        {
            action();
        }
    }


    public abstract class ParameterOption : Option
    {
        public bool IsParameterMandatory { get; init; }
        public abstract bool TryParse(string param);
    }

    
    public abstract class MultipleParameterOption : ParameterOption
    {
        public char Delimiter { get; init; }
    }



    public class IntOption : ParameterOption
    {
        Action<int?> saveAction;

        // TODO: Consider using default value instead of nullable type.
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

    

    public class MultipleIntOption : MultipleParameterOption
    {
        Action<int[]?> saveAction;
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



    public class StringOption : ParameterOption
    {
        Action<string?> saveAction;

        // TODO: Consider using default value instead of nullable type.
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

    public class MultipleStringOption : MultipleParameterOption
    {
        Action<string[]?> saveAction;
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

    class BoolOption : ParameterOption
    {
        private Action<bool> saveAction;
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

    class MultipleBoolOption : ParameterOption
    {
        private Action<bool[]?> saveAction;
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
