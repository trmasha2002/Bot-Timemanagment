using System;
using System.Collections.Generic;
using Microsoft.Bot.Connector;
using System.Linq;
using System.Web;
using Bot.Dialogs;

namespace Bot.Dialogs
{
    [Serializable]
    public class Problem
    {
        public Datatime Start_time { get; set; }
        public Datatime Finish_time { get; set; }
        public Datatime Add_time { get; set; }
        public string Name { get; set; }
        public Problem(IMessageActivity activity)
        {
            Add_time = new Datatime();
            Name = activity.Text;
        }
        public void add_time_start(IMessageActivity activity)
        {

            string time = activity.Text;
            Start_time = new Datatime(time);
        }
        public void add_time_finish(IMessageActivity activity)
        {
            string time = activity.Text;
            Finish_time = new Datatime(time);
        }
    }
}