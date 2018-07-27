using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Bot.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Timers;
namespace Bot_Application2
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary> 
        /// POST: api/Messages 
        /// Receive a message from a user and reply to it 
        /// </summary> 
        private static Activity act;
        private static Timer aTimer = new Timer();
        //public int seed;

        public static async Task Starttimer(int seed)
        {
            aTimer.Interval = seed;

            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;


            // Start the timer 
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
            aTimer.Start();
        }
        private static async void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            ConnectorClient connector = new ConnectorClient(new Uri(act.ServiceUrl));
            int number = 1;

            string tasks = string.Empty;
            foreach (Problem task in RootDialog.todo)
            {
                tasks += String.Format("{0}) {1}", number.ToString(), task.Name);
                tasks += "\n";
                number++;

            }
            Activity reply = act.CreateReply(tasks);
            await connector.Conversations.ReplyToActivityAsync(reply);

        }
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            act = activity;
            if (activity.Text == "Стоп" || activity.Text == "стоп")
            {

                aTimer.Stop();
                aTimer.Interval = 0;
                aTimer.Elapsed -= OnTimedEvent;

                //seed = RootDialog.timeseed;
                //await Starttimer(seed);
            }

            if (activity.Type == ActivityTypes.Message)
            {
                await Conversation.SendAsync(activity, () => new Bot.Dialogs.RootDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);


            //string resume = "lf"; 


            return response;
        }

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here 
                // If we handle user deletion, return a real message 
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed 
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info 
                // Not available in all channels 
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists 
                // Activity.From + Activity.Action represent what happened 
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing 
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}