using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace D3HeroesTool.Validators
{
    /// <summary>
    /// Validates a battle tag according to following syntax:
    /// NickName#1234
    /// </summary>
    public class BattleTagValidator : ValidationRule
    {
        public BattleTagValidator() { ValidatesOnTargetUpdated = true; }

        public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
        {
            if (value == null)
                return new ValidationResult(false, "A valid BattleTag is of the form MyNickname#1234");

            string[] elems = ((string)value).Split('#');
            if (elems.Length != 2)
                return new ValidationResult(false, "A valid BattleTag includes one, and only one, '#' (ex: MyNickname#1234)");

            try
            {
                int integral_part = int.Parse(elems[1]);
            }
            catch // Expect FormatException
            {
                return new ValidationResult(false,
                        "A valid BattleTag contains a number after '#' (ex: MyNickname#1234) instead of " + elems[1]);
            }

            return new ValidationResult(true, null);
        }
    }
}
