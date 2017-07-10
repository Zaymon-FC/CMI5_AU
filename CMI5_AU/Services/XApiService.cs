using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TinCan.LRSResponses;
using TinCan;
using System.Diagnostics;

namespace CMI5_AU.Services
{
    public class XApiService
    {
        private string username = "zaymon.fc@conceptsafety.com.au";
        private string password = "!P@ssw0rd";

        private Dictionary<string, Uri> verbMap = new Dictionary<string, Uri>
        {
            { "launched", new Uri("http://adlnet.gov/expapi/verbs/launched") },
            { "initialized", new Uri("http://adlnet.gov/expapi/verbs/initialized") },
            { "progressed", new Uri("http://adlnet.gov/expapi/verbs/progressed") },
            { "completed", new Uri("http://adlnet.gov/expapi/verbs/completed") },
            { "passed", new Uri("http://adlnet.gov/expapi/verbs/passed") },
            { "terminated", new Uri("http://adlnet.gov/expapi/verbs/terminated") }
        };

        private RemoteLRS lrs;

        private RemoteLRS ConnectLrs(string username, string password, string endpoint= "https://cloud.scorm.com/tc/YP5P8MYB6K/sandbox/")
        {

            try
            {
                return new RemoteLRS(endpoint, username, password);
            }
            catch (Exception)
            {
                Debug.WriteLine("Error connecting to the LRS");
                throw;
            }
        }

        private bool ValidateVerb(string verb)
        {
            return verbMap.ContainsKey(verb) ? true : false;
        }

        private Verb CreateVerb(string shortVerb)
        {
            Uri verbUri;
            verbMap.TryGetValue(shortVerb, out verbUri);

            var verb = new Verb();
            verb.id = verbUri;
            verb.display = new LanguageMap();
            verb.display.Add("en-US", shortVerb);
            return verb;
        }

        public void SendStatement(string shortVerb)
        {

            if (!ValidateVerb(shortVerb))
            {
                throw new Exception("Supplied Verb is not in the map of valid verbs (Check spelling or usage?)");
            }

            // Connect to the LRS
            lrs = ConnectLrs(username, password);


            // Define the actor
            var actor = new Agent();
            actor.mbox = "mailto:example@example.com";
            actor.name = "Zaymon Foulds-Cook";

            Verb verb = CreateVerb(shortVerb);

            // Define the activity
            var activity = new Activity();
            activity.id = "http://www.example.com/SomeActivity";

            // Define the statement
            var statement = new Statement();
            statement.actor = actor;
            statement.verb = verb;
            statement.target = activity;

            // Send the statement to the LRS
            StatementLRSResponse lrsResponse = lrs.SaveStatement(statement);
            if (lrsResponse.success)
            {
                // Updated 'statement' here, now with id
                Console.WriteLine("Save statement: " + lrsResponse.content.id);
            }
            else
            {
                Debug.WriteLine("Something went wrong here captain!" + lrsResponse.errMsg);
            }
        }
    }

}
