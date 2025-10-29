using System.ComponentModel;
using System.Reflection;

namespace RastreamentoCargas.Domain.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Retorna a descrição (DescriptionAttribute) associada ao valor do Enum.
        /// Se o atributo não existir, retorna o nome do Enum.
        /// </summary>
        public static string GetFriendlyName(this Enum value)
        {

            var fieldInfo = value.GetType().GetField(value.ToString());

            if (fieldInfo is null) return value.ToString();
            
            var attribute = (DescriptionAttribute?)Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute));

            return attribute?.Description ?? value.ToString();
        }
    }
}