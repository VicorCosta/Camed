using System;
using System.Collections.Generic;
using System.Text;

namespace Camed.SSC.Application.Util
{
    public class MessageSending
    {
        public MessageSending(string from, string to, string text)
        {
            this.from = from;
            this.to = to;
            this.text = text;
        }

        public string from { get; set; }
        public string to { get; set; }
        public string text { get; set; }
    }

    public class Status
    {
        public int groupId { get; set; }
        public string groupName { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class ReturnMessage
    {
        public string to { get; set; }
        public Status status { get; set; }
        public int smsCount { get; set; }
        public string messageId { get; set; }
    }

    public class RootObject
    {
        public List<ReturnMessage> messages { get; set; }
    }
}
