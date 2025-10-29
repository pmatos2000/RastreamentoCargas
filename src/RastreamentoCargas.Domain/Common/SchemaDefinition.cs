namespace RastreamentoCargas.Domain.Common
{
    public static class SchemaDefinition
    {
        public const int NameDefaultLength = 100;
        public const int DoumentDefaultLength = 20;
        public const int PhoneDefaultLength = 20;
        public const int EmailDefaultLength = 255;
        public const int LocationDefaultLength = 255;
        public const int ObservationDefaultLength = 500;

        public static class User
        {
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