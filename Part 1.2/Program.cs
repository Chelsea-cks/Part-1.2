using System;
using System.Media;
using System.Threading;
using System.Xml.Serialization;
using NAudio.Wave;

class CyberSecurityBot
{

    static void Main()
    {
        Console.Title = "Cybersecurity Awareness Bot";
        Console.ForegroundColor = ConsoleColor.Cyan;

        Console.Clear();
        DisplayAsciiArt();
        Console.ResetColor();

        PlayVoiceGreeting();

        Console.Write("Chat: Enter your name: ");
        string userName = Console.ReadLine()?.Trim();

        while (string.IsNullOrEmpty(userName))// ensures that user enters a name
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Chat: Name cannot be empty. Please enter your name:");
            Console.ResetColor();
            userName = Console.ReadLine()?.Trim();
        }

        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(new string('=', 50));
        Console.WriteLine($"Chat: Welcome, {userName}! I'm here to help you stay safe online.");
        Console.WriteLine(new string('=', 50));
        Console.ResetColor();

        ChatbotLoop(userName);
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
                   //.         ./%%###########%#*  ");
        Console.ResetColor();

    }

    static void PlayVoiceGreeting()
    {
        string audioFilePath = "C:\\Users\\Dell 3550\\source\\repos\\Part1.1\\greetings.wav";

        try
        {
            using (var audioFile = new AudioFileReader(audioFilePath))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(audioFile);
                outputDevice.Play();
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100);
                }
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(" Error playing voice greeting: " + ex.Message);
            Console.ResetColor();
        }

    }

    static void ChatbotLoop(string userName)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Chat: Ask me anything about cybersecurity or type 'exit' to quit.");
        Console.ResetColor();

        while (true)
        {
            Console.Write(" You: ");
            string userInput = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrEmpty(userInput))
            {
                PrintSlowly($"Chat: {userName}, please enter a valid question.", ConsoleColor.Red);
                continue;
            }

            if (userInput == "exit")
            {
                PrintSlowly($"Chat: Goodbye, {userName}! Stay safe online. ", ConsoleColor.Red);
                break;
            }

            string response = GetResponse(userInput, userName);
            PrintSlowly($" {userName}, {response}", ConsoleColor.Cyan);
        }

    }

    static string GetResponse(string input, string userName)
    {
        if (input.Contains("password"))
            return "a strong password should have at least 12 characters, including uppercase, lowercase, numbers, and special symbols.";

        if (input.Contains("phishing"))
            return "be cautious of emails or messages asking for personal details. Always verify the sender before clicking on links.";

        if (input.Contains("safe browsing") || input.Contains("browse safely"))
            return "always check for 'https://' in URLs, avoid suspicious websites, and use a secure browser.";

        switch (input)
        {
            case "how are you?":
            case "how are you doing?":
                return "I'm just a bot, but I'm always ready to help you stay safe online! 😊";

            case "what’s your purpose?":
            case "what do you do?":
                return "I’m here to provide cybersecurity awareness and help you protect yourself online! 🔒";

            case "what can i ask you about?":
                return "You can ask me about password safety, phishing, safe browsing, and other cybersecurity tips.";

            default:
                return "I didn’t quite understand that. Could you rephrase?";
        }
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

    static void PrintBox(string message)
    {
        int boxWidth = message.Length + 6;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(new string('═', boxWidth));
        Console.WriteLine($"  {message}  ");
        Console.WriteLine(new string('═', boxWidth));
        Console.ResetColor();
    }

    static void DrawBox(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine("\n" + new string('═', message.Length + 4));
        Console.WriteLine($"  {message}  ");
        Console.WriteLine(new string('═', message.Length + 4));
        Console.ResetColor();
    }

    static void DrawDivider()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine(new string('─', 50));
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

};

