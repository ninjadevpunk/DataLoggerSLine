
namespace Data_Logger_1._3.Models.App_Models
{
    public class TypeClass
    {
        public int TypeID { get; set; } = 1;

        public ACCOUNT User { get; set; }

        public string Name { get; set; } = "NONE";

        public ApplicationClass Application { get; set; }

        public LOG.CATEGORY Category { get; set; } = LOG.CATEGORY.CODING;

        public TypeClass()
        {
        }

        public TypeClass(string typeName)
        {
            Name = typeName;
        }

        public TypeClass(int typeID, ACCOUNT user, string name, ApplicationClass application, LOG.CATEGORY category)
        {
            TypeID = typeID;
            User = user;
            Name = name;
            Application = application;
            Category = category;
        }

        public override bool Equals(object? obj)
        {
            return obj is TypeClass @class &&
                   TypeID == @class.TypeID &&
                   User.Equals(@class.User) &&
                   Name == @class.Name &&
                   Application.Equals(@class.Application) &&
                   Category == @class.Category;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TypeID, User, Name, Application, Category);
        }

        public static bool operator ==(TypeClass left, TypeClass right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TypeClass left, TypeClass right)
        {
            return !left.Equals(right);
        }
    }
}
