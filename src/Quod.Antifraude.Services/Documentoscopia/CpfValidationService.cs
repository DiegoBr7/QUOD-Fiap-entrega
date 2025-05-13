using System.Linq;

namespace Quod.Antifraude.Services.Documentoscopia
{
    public class CpfValidationService
    {
        /// <summary>
        /// Verifica dígitos verificadores do CPF.
        /// </summary>
        public bool IsValid(string cpf)
        {
            var digits = new string(cpf.Where(char.IsDigit).ToArray());
            if (digits.Length != 11 || digits.All(c => c == digits[0]))
                return false;

            int[] m1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] m2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            // Primeiro dígito
            var temp = digits[..9];
            int soma = temp.Select((c, i) => (c - '0') * m1[i]).Sum();
            int resto = (soma % 11) < 2 ? 0 : 11 - (soma % 11);
            if (resto != digits[9] - '0')
                return false;

            // Segundo dígito
            temp += resto;
            soma = temp.Select((c, i) => (c - '0') * m2[i]).Sum();
            resto = (soma % 11) < 2 ? 0 : 11 - (soma % 11);
            return resto == digits[10] - '0';
        }
    }
}
