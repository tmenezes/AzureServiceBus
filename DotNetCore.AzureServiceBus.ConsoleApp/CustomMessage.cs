using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetCore.AzureServiceBus.ConsoleApp
{
    public class CustomMessage
    {
        public string Content { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedOn { get; set; }

        public CustomMessage(string content, string userName, DateTime createdOn)
        {
            Content = content;
            UserName = userName;
            CreatedOn = createdOn;
        }

        public override string ToString()
        {
            return $"{nameof(Content)}: {Content}, {nameof(UserName)}: {UserName}, {nameof(CreatedOn)}: {CreatedOn}";
        }
    }
}
