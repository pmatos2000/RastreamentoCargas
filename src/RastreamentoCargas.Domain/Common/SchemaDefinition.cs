namespace RastreamentoCargas.Domain.Common
{
    public static class SchemaDefinition
    {
        public static class User
        {
            public const int UserNameLength = 100;
            public const int PasswordMinLength = 8;
            public const int PasswordMaxLength = 16;
        }

        public static class Operator
        {
            public const int EmployeeIdLength = 50;
            public const int DepartmentNameLength = 100;
        }
    }
}