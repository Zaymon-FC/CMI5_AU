using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TinCan.LRSResponses;
using TinCan;
using System.Diagnostics;
using Newtonsoft.Json;



/// <summary>
///  Notes, Need to add an authority statement and implement a working context checker to remove the plethora
///  of isnotnull statements throughout the services
/// </summary>



namespace CMI5_AU.Services
{
    public class XApiService
    {
        private string _username = "zaymon.fc@conceptsafety.com.au";
        private string _password = "!P@ssw0rd";

        private Dictionary<string, Uri> verbMap = new Dictionary<string, Uri>
        {
            { "launched", new Uri("http://adlnet.gov/expapi/verbs/launched") },
            { "initialized", new Uri("http://adlnet.gov/expapi/verbs/initialized") },
            { "progressed", new Uri("http://adlnet.gov/expapi/verbs/progressed") },
            { "completed", new Uri("http://adlnet.gov/expapi/verbs/completed") },
            { "passed", new Uri("http://adlnet.gov/expapi/verbs/passed") },
            { "terminated", new Uri("http://adlnet.gov/expapi/verbs/terminated") }
        };

        private RemoteLRS _lrs;
        private HttpRequest _request;

        public void CreateStatement(string shortVerb, HttpRequest Request)
        {
            _request = Request;
            

            Verb verb;
            if (!ValidateVerb(shortVerb))
            {
                throw new Exception("Supplied Verb is not in the map of valid verbs (Check spelling or usage?)");
            }
            else
            {
                // Create the verb using the supplied shortverb
                verb = CreateVerb(shortVerb);
            }



            string fetched_username = "";
            string fetched_password = "";
            // Connect to the LRS
            var endpoint = _request.QueryString["endpoint"];

            var actorJSON = string.IsNullOrEmpty(_request.QueryString["actor"]) ? null : JsonConvert.DeserializeObject<ActorJSON>(_request.QueryString["actor"]);

            if (actorJSON != null)
            {
                //Get the username
                fetched_username = actorJSON.account.name.Split('|')[1];
                // Get the password
                fetched_password = actorJSON.account.name.Split('|')[0];
            }

            // Lets try and mash the enpoint out of the Query String
            _lrs = string.IsNullOrEmpty(endpoint) ? ConnectLrs(_username, _password) : ConnectLrs(_username, _password, endpoint);



            // Create the statement context
            var context = new Context();

            //Create the actor (of type agent)
            var actor = CreateActor(context);

            // Define the activity
            var activity = CreateActivity();

            var state1 = _lrs.auth;
            var state = _lrs.RetrieveState("", activity, actor, context.registration);
            // Debug.WriteLine("\n \n \n State Doc?: \n" + state.content.ToString() + "\n\nAuth:" + state1 + "\n\n");

            // Define the statement
            var statement = new Statement()
            {
                actor = actor,
                verb = verb,
                target = activity,
                context = context,
            };

            SendStatement(statement);
        }

        private bool ValidateVerb(string verb)
        {
            return verbMap.ContainsKey(verb);
        }


        private RemoteLRS ConnectLrs(string username, string password, string endpoint= "https://cloud.scorm.com/tc/YP5P8MYB6K/sandbox/")
        {
            Debug.Write("\n ENDPOINT USED: " + endpoint + "\n");
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

        private Verb CreateVerb(string shortVerb)
        {
            Uri verbUri;
            // Check if the verb is in the verb map
            verbMap.TryGetValue(shortVerb, out verbUri);

            // Define a new verb and return
            var verb = new Verb()
            {
                id = verbUri,
                display = new LanguageMap()
            };

            verb.display.Add("en-US", shortVerb);
            return verb;
        }

        private Agent CreateActor(Context context)
        {
            var actorJSON = string.IsNullOrEmpty(_request.QueryString["actor"]) ? null : JsonConvert.DeserializeObject<ActorJSON>(_request.QueryString["actor"]);

            var actor = new Agent();

            if (actorJSON == null)
            {
                // TODO: Create context creation function once I understand the requirements
                actor.mbox = "mailto:example@example.com";
                actor.name = "Zaymon Foulds-Cook";
            }
            else
            {
                context.registration = new Guid(_request.QueryString["registration"]);
                var name = actorJSON.name;
                var mbox =  "mailto:" + actorJSON.account.name.Split('|')[1]; // ? Depending on if we need that identifier on the front?
                actor.mbox = mbox;
                actor.name = name;
            }
            return actor;
        }

        private Activity CreateActivity()
        {
            // Get the activityId from the query
            var activity = new Activity();
            var activityId = _request.QueryString["activityId"];

            // Setup the activity definition
            var activityDefinition = new ActivityDefinition();
            activityDefinition.name = new LanguageMap();
            activityDefinition.name.Add("en-US", "Course 3");


            if (activityId != null)
            {

                activity.id = activityId;
                activity.definition = activityDefinition;
            }
            else
            {
                activity.id = "http://localhost:50136";
            }
            return activity;
        }

        private void SendStatement(Statement statement)
        {
            // Send the statement to the LRS
            StatementLRSResponse lrsResponse = _lrs.SaveStatement(statement);
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

// Define a class as a helper when deserialising information provided by the LRS
class ActorJSON
{
    public string objectType { get; set; }
    public Account account { get; set; }
    public string name { get; set; }
}

class Account
{
    public string homePage { get; set; }
    public string name { get; set; }
}