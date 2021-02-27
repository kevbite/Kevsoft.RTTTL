using System;
using System.Device.Pwm;
using System.Threading;
using Kevsoft.RTTTL;
using System.Linq; 

namespace DeviceGpio
{
    class Program
    {
        static void Main(string[] args)
        {
            // https://github.com/dotnet/iot/blob/main/Documentation/raspi-pwm.md
            Console.WriteLine("Hello PWM!");
            using var pwm = PwmChannel.Create(0, 0, 700);
          
            var rtttls = new []{
                "Simpsons:d=4,o=5,b=160:c.6,e6,f#6,8a6,g.6,e6,c6,8a,8f#,8f#,8f#,2g,8p,8p,8f#,8f#,8f#,8g,a#.,8c6,8c6,8c6,c6",
                "Super Mario - Main Theme:d=4,o=5,b=125:a,8f.,16c,16d,16f,16p,f,16d,16c,16p,16f,16p,16f,16p,8c6,8a.,g,16c,a,8f.,16c,16d,16f,16p,f,16d,16c,16p,16f,16p,16a#,16a,16g,2f,16p,8a.,8f.,8c,8a.,f,16g#,16f,16c,16p,8g#.,2g,8a.,8f.,8c,8a.,f,16g#,16f,8c,2c6",
                "Muppets:d=4,o=5,b=250:c6,c6,a,b,8a,b,g,p,c6,c6,a,8b,8a,8p,g.,p,e,e,g,f,8e,f,8c6,8c,8d,e,8e,8e,8p,8e,g,2p,c6,c6,a,b,8a,b,g,p,c6,c6,a,8b,a,g.,p,e,e,g,f,8e,f,8c6,8c,8d,e,8e,d,8d,c",
                "Looney:d=4,o=5,b=140:32p,c6,8f6,8e6,8d6,8c6,a.,8c6,8f6,8e6,8d6,8d#6,e.6,8e6,8e6,8c6,8d6,8c6,8e6,8c6,8d6,8a,8c6,8g,8a#,8a,8f"
            }
            .Select(song => (prased: Rtttl.TryParse(song, out var rtttl), rtttl: rtttl) switch
                {
                    (true, _) x => x.rtttl!,
                    (false, _) => throw new InvalidOperationException()
                })
            .ToArray();

            Console.WriteLine("Select a song to play:");
            for(var i =0; i < rtttls.Length; i++)
            {
                Console.WriteLine($"{i}: {rtttls[i].Name}");
            }

            Console.Write("> ");

            if(!int.TryParse(Console.ReadLine(), out var index) || index >= rtttls.Length || index < 0)
            {
                Console.WriteLine($"Invalid Input");
                return;
            }
            
            var selectedRtttl = rtttls[index];
            Console.WriteLine($"Playing: {selectedRtttl.Name}");
            
            selectedRtttl.PlayWithPwmChannel(pwm);
        }
    }


}
