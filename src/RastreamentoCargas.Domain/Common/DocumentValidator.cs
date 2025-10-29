using RastreamentoCargas.Domain.Enums;

namespace RastreamentoCargas.Domain.Common
{
    /// <summary>
    /// Classe estática para validação de documentos fiscais (CPF e CNPJ).
    /// </summary>
    public static class DocumentValidator
    {
        /// <summary>
        /// Normaliza a string de entrada, removendo caracteres não-numéricos.
        /// </summary>
        private static string Normalize(string document)
        {
            return new string(document.Where(char.IsDigit).ToArray());
        }

        /// <summary>
        /// Verifica se o número do documento é válido (tamanho e dígito verificador).
        /// </summary>
        public static bool IsValid(string document, ClientDocumentType type)
        {
            var normalizedDoc = Normalize(document);

            if(normalizedDoc != document) return false;

            return type switch
            {
                ClientDocumentType.CPF => IsValidCpf(normalizedDoc),
                ClientDocumentType.CNPJ => IsValidCnpj(normalizedDoc),
                _ => normalizedDoc.Length > 0
            };
        }

        private static bool IsValidCnpj(string cnpj)
        {
            int[] multiplier1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplier2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            cnpj = Normalize(cnpj);
            if (cnpj.Length != 14) return false;

            if (new string(cnpj[0], 14) == cnpj) return false;

            string tempCnpj = cnpj.Substring(0, 12);
            int sum = 0;

            for (int i = 0; i < 12; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier1[i];

            int remainder = sum % 11;
            int checkDigit = remainder < 2 ? 0 : 11 - remainder;
            tempCnpj += checkDigit;

            sum = 0;
            for (int i = 0; i < 13; i++)
                sum += int.Parse(tempCnpj[i].ToString()) * multiplier2[i];

            remainder = sum % 11;
            checkDigit = remainder < 2 ? 0 : 11 - remainder;

            tempCnpj += checkDigit;

            return cnpj.EndsWith(tempCnpj.Substring(12, 2));
        }

        private static bool IsValidCpf(string cpf)
        {
            int[] multiplier1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplier2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            cpf = Normalize(cpf);
            if (cpf.Length != 11) return false;

            if (new string(cpf[0], 11) == cpf) return false;

            string tempCpf = cpf.Substring(0, 9);
            int sum = 0;

            for (int i = 0; i < 9; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];

            int remainder = sum % 11;
            remainder = remainder < 2 ? 0 : 11 - remainder;
            string checkDigit = remainder.ToString();
            tempCpf += checkDigit;

            sum = 0;
            for (int i = 0; i < 10; i++)
                sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];

            remainder = sum % 11;
            remainder = remainder < 2 ? 0 : 11 - remainder;
            checkDigit += remainder.ToString();

            return cpf.EndsWith(checkDigit);
        }
    }
}