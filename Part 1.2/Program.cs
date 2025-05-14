using System;
using System.IO;
using System.Threading;
using NAudio.Wave;
using System.Collections.Generic;

namespace CyberSecurityBot
{
    internal class Program
    {
        static Dictionary<string, string> keywordResponses = new Dictionary<string, string>
        {
            { "password", "Make sure to use strong, unique passwords for each account. Avoid personal details." },
            { "scam", "Always verify unknown links or calls. Scammers often impersonate trusted entities." },
            { "privacy", "Review your app permissions and privacy settings regularly to protect your data." },
            { "phishing", "Be cautious of emails asking for personal information. Scammers often disguise themselves as trusted organisations." }
        };

        static List<string> passwordTips = new List<string>
        {
            "Use a mix of letters, numbers, and symbols in your password.",
            "Avoid using personal info like birthdays or pet names in passwords.",
            "Use a password manager to generate and store strong passwords.",
            "Change your passwords regularly and don’t reuse old ones.",
            "Enable two-factor authentication wherever possible."
        };

        static List<string> scamTips = new List<string>
        {
            "Don’t click on links from unknown senders.",
            "If it sounds too good to be true, it probably is — be suspicious of online offers.",
            "Always verify the sender's email address before taking action.",
            "Watch out for urgent language asking you to act immediately.",
            "Never share personal or financial information via email or SMS."
        };

        static List<string> privacyTips = new List<string>
        {
            "Review app permissions on your phone and remove access you don’t need.",
            "Use privacy-focused browsers or search engines like DuckDuckGo.",
            "Avoid posting personal information publicly on social media.",
            "Use strong passwords and avoid saving login info in public browsers.",
            "Turn off location tracking when it’s not necessary."
        };

        static List<string> phishingTips = new List<string>
        {
            "Avoid clicking suspicious links in emails.",
            "Verify sender addresses before replying to emails.",
            "Use multi-factor authentication for extra security."
        };

        static Dictionary<string, string> sentimentResponses = new Dictionary<string, string>
        {
            { "worried", "It's okay to feel worried. Let's go over some tips to ease your concerns." },
            { "curious", "Curiosity is great! Learning about cybersecurity keeps you one step ahead." },
            { "frustrated", "I understand it's frustrating. Cybersecurity can be tricky, but I'm here to help." }
        };

        static Dictionary<string, string> userMemory = new Dictionary<string, string>();
        static string currentTopic = "";

        static void Main(string[] args)
        {
            Console.Title = "Cybersecurity Awareness Bot";
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Clear();

            DisplayAsciiArt();
            Console.ResetColor();

            PlayVoiceGreeting();

            DrawDivider();
            PrintMessage("Chat: Enter your name: ", ConsoleColor.Yellow);
            string userName = Console.ReadLine()?.Trim();

            while (string.IsNullOrEmpty(userName))
            {
                PrintMessage("Chat: Name cannot be empty. Please enter your name:", ConsoleColor.Red);
                userName = Console.ReadLine()?.Trim();
            }

            Console.Clear();
            DrawBox($"Welcome, {userName}! I'm here to help you stay safe online.", ConsoleColor.Green);
            ChatbotLoop(userName);
        }

        static void ChatbotLoop(string userName)
        {
            DrawDivider();
            PrintMessage("Chat: Ask me anything about cybersecurity or type 'exit' to quit.", ConsoleColor.Yellow);
            DrawDivider();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("You: ");
                string userInput = Console.ReadLine()?.Trim().ToLower();
                Console.ResetColor();

                if (string.IsNullOrEmpty(userInput))
                {
                    PrintSlowly($"Chat: {userName}, please enter a valid question.", ConsoleColor.Red);
                    continue;
                }

                if (userInput == "exit")
                {
                    DrawBox($"Goodbye, {userName}! Stay safe online.", ConsoleColor.Red);
                    break;
                }

                string response = GetResponse(userInput, userName);
                PrintSlowly($"Chat: {response}", ConsoleColor.Cyan);
            }
        }

        static string GetResponse(string input, string userName)
        {
            if (IsNumeric(input))
                return "Invalid input. Please ask a question using words, not numbers.";
             
            foreach (var mood in sentimentResponses) //this will detect moods/sentiment
            {
                if (input.Contains(mood.Key))
                    return sentimentResponses[mood.Key];
            }
            //this handles topic interest input
            if (input.Contains("interested in"))
            {
                string[] parts = input.Split(new[] { "interested in" }, StringSplitOptions.None);
                if (parts.Length > 1)
                {
                    string topic = parts[1].Trim();
                    userMemory["interest"] = topic;
                    return $"Great! I'll remember that you're interested in {topic}.";
                }
            }

            if (input.Contains("remind me") && userMemory.ContainsKey("interest"))
            {
                return $"You mentioned you're interested in {userMemory["interest"]}. Here's a tip: Review security settings for your {userMemory["interest"]} frequently.";
            }

            foreach (var keyword in keywordResponses.Keys)
            {
                if (input.Contains(keyword))
                {
                    currentTopic = keyword;

                    // Personalize if user previously mentioned interest
                    if (userMemory.ContainsKey("interest") && keyword == userMemory["interest"])
                    {
                        return $"Since you're interested in {keyword}, here's a tip: {keywordResponses[keyword]}";
                    }

                    // Random tips per topic
                    Random rand = new Random();
                    if (keyword == "phishing")
                    {
                        return phishingTips[rand.Next(phishingTips.Count)] + " Also: " + keywordResponses[keyword];
                    }
                    else if (keyword == "password")
                    {
                        return passwordTips[rand.Next(passwordTips.Count)];
                    }
                    else if (keyword == "scam")
                    {
                        return scamTips[rand.Next(scamTips.Count)];
                    }
                    else if (keyword == "privacy")
                    {
                        return privacyTips[rand.Next(privacyTips.Count)];
                    }

                    return keywordResponses[keyword];
                }
            }

            if ((input.Contains("more") || input.Contains("explain")) && !string.IsNullOrEmpty(currentTopic))
            {
                return $"Here's more about {currentTopic}: {keywordResponses[currentTopic]}";
            }

            switch (input)
            {
                case "how are you?":
                case "how are you doing?":
                    return "I'm just a bot, but I'm always ready to help you stay safe online!";

                case "what’s your purpose?":
                case "what do you do?":
                    return "I’m here to provide cybersecurity awareness and help you protect yourself online!";

                case "what can i ask you about?":
                    return "You can ask me about password safety, phishing, safe browsing, and other cybersecurity tips.";
            }

            return "I didn’t quite understand that. Can you try rephrasing or ask about another cybersecurity topic?";
        }

        static void DisplayAsciiArt()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine(@"
                              .(%##############%%*                              
                         (############################%*                        
        (#           *####################################%.           %,       
        ##        .###########################################        %##       
        ##       #######.                               *#######      %##       
        ##     %######                                     ######,    %##       
          %#( %#####.                                       (#####* %## ,       
        %##% (#####.        .**.                 ,/,         (#####..###/       
       ####* %#####       %#######.           (#######%       %##### %###,      
       ####. #####/                                           %####% %###/      
       (###/ %####%                  /(   %                   #####( %###.      
        #### (#####,                                         ###### .###*       
          ##% ######*                                       ######, #%*         
               /######                                    ,######               
                 #######(                              .#######(                
                   ##########################################*                  
                     &####################################(                     
                    .#####%###########################(                         
                   //.         ./%%###########%#*  
            ");
            Console.ResetColor();
        }

        static void PlayVoiceGreeting()
        {
            string audioFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "greetings.wav");

            try
            {
                using (var audioFile = new AudioFileReader(audioFilePath))
                using (var outputDevice = new WaveOutEvent())
                {
                    outputDevice.Init(audioFile);
                    outputDevice.Play();
                    while (outputDevice.PlaybackState == PlaybackState.Playing)
                    {
                        Thread.Sleep(50);
                    }
                }
            }
            catch (Exception ex)
            {
                PrintMessage("Error playing voice greeting: " + ex.Message, ConsoleColor.Red);
            }
        }

        static bool IsNumeric(string input)
        {
            return double.TryParse(input, out _);
        }

        static void PrintSlowly(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(20);
            }
            Console.WriteLine();
            Console.ResetColor();
        }

        static void DrawBox(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            string border = new string('═', message.Length + 4);
            Console.WriteLine("\n" + border);
            Console.WriteLine($"  {message}  ");
            Console.WriteLine(border);
            Console.ResetColor();
        }

        static void DrawDivider()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(new string('─', 60));
            Console.ResetColor();
        }

        static void PrintMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            foreach (char c in message)
            {
                Console.Write(c);
                Thread.Sleep(20);
            }
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}
