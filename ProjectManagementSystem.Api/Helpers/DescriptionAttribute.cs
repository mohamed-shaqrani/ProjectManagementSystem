using AutoMapper.Execution;
using System.Reflection;

namespace ProjectManagementSystem.Api.Helpers
{
    public class DescriptionAttribute : Attribute
    {
       private string Word;

        public DescriptionAttribute(string word) 
        {
            this.Word = word;
        }

        public static string GetDescription(object obj) 
        {
            string description = string.Empty;
            if (obj == null)
            {
                return description;
            }

            Type type = obj.GetType();

            MemberInfo[] members = type.GetMember(obj.ToString());

            if (members is null || !members.Any())
                return description;
            object[] attributes = members[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            if(attributes is null || !attributes.Any()) 
            {
                return description;
            }

            DescriptionAttribute descriptionAttribute = (DescriptionAttribute)attributes[0];

            return descriptionAttribute.Word;
        }
    }
}
