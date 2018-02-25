using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Data;
using System.Security.Permissions;
using System.Threading;
/*
 * goodbye0 
 * I do not call myself a god at coding and this should not be used for practical use, only for learning.
 * Offsets retrieved using Cheat Engine and subtracting the dynamic address from the local player
 * ReadProcessMemory / WriteProcessMemory library was not written by me, I just modified it
 * This is used for hacking on AssaultCube as an educational source, Please support the game by donating as it is actually good and the developers are friendly and kind!
 * I forgot to exclude FASM
 * Vector2, Vector3, Vector4 and the viewmatrix and other things included in my source are there because I was intending on making an ESP and aimbot but I didn't have time.
*/

namespace assault_
{
    class Program
    {
        private const string processName = "ac_client"; // declare process name
        private  static Process process; // declare Process
        private static Player self; // declare Player.cs as SELF
        private  static void AttachTo()
        {
            bool success = false; // declare bool
            do
            {
                if (Memory.GetProcessesByName(processName, out process))
                {
                    Console.WriteLine("Attaching.."); // tell user we attachin!
                    Thread.Sleep(3000); // wait 3 seconds
                    try
                    {
                        IntPtr handle = Memory.OpenProcess(process.Id); // open handle to the game
                        Console.WriteLine("Attached: 0x" + handle); // tell user we are in!
                        if (handle != IntPtr.Zero) // handle is not = zero
                            success = true; // set bool to true if we attached!
                    }
                    catch (Exception excp) // bool is not true, lets see why
                    {
                        Console.WriteLine("Attach failed: " + excp.Message); // it failed for a reason
                        Console.ReadLine(); // dont close!
                    }
                }
                else // the game isn't running idiote
                {
                    Console.WriteLine("Is the game running?"); // you goofy!
                    Console.ReadLine(); // dont close!
                }
            } while (!success);
        }
        private static void ReadGameMemory()
        {
            int ptrPlayerSelf = Memory.Read<int>(Offsets.baseGame + Offsets.ptrPlayerEntity); //declare player offset
            self = new Player(ptrPlayerSelf); // set "self" to pointer Player
        }

        static void Main(string[] args)
        {
                Process[] ProcessList = System.Diagnostics.Process.GetProcessesByName("ac_client"); // get process by name "ac_client" from process list
                if (ProcessList.Length > 0) // check that process list is not fucked (empty)
            {
                Process assaultcube = ProcessList[0]; // self-explanatory
                IntPtr BaseAddress = IntPtr.Zero; // self-explanatory

                foreach (System.Diagnostics.ProcessModule Module in assaultcube.Modules) // check modules in our assaultcube process
                {
                    if (Module.ModuleName.Contains("ac_client.exe")) // check for module that contains "ac_client.exe"
                    BaseAddress = Module.BaseAddress; // declare BaseAddress as the base address of the module we just searched for above
                }

                try
                {
                    SecurityPermission sp = new SecurityPermission(SecurityPermissionFlag.UnmanagedCode); // Declare which flag we want
                    sp.Demand(); // Demand it
                } catch (Exception ex) // if failed, type into console that it failed
                {
                    Console.WriteLine("Could not set SecurityPermissionFlag.UnmanagedCode:" + ex.Message); // tells the user the demand failed and displays exception
                    Console.ReadLine(); // makes console not shut down instantly
                }

                if (BaseAddress != IntPtr.Zero)
                {
                    Console.WriteLine("Found base address at: 0x" + BaseAddress); // tells user we found the base address and displays it in the console
                    Thread.Sleep(5000); // wait 5 seconds

                }
                else
                {
                    Console.WriteLine("Base address not found!"); // game is not running or program failed
                    Console.ReadLine(); // makes console not shut down instantly
                }


                AttachTo(); // Initialize the attach!
                ReadGameMemory(); // Initialize self. and the player structs!
                self.Health = 99999; // Set health to 99999, this isn't done through magic though!, check the defiintions!
                self.weapon.Ammo = 99999; // Set Ammo to 99999, this isn't done through magic though!, check the defiintions!
                self.weapon.AmmoClip = 99999; // Set Magazine to 99999, this isn't done through magic though!, check the defiintions!
                self.weapon.DelayTime = 0; // No Reload, this isn't done through magic though!, check the defiintions!
                self.vest = 99999; // Set Armor to 99999, this isn't done through magic though!, check the defiintions!
                Memory.Write<int>(Offsets.recoil, 0); // No Recoil

                Console.WriteLine("Ammo set to 99999"); // tell user we are gold!
                Thread.Sleep(700); //wait 700ms between messages
                Console.WriteLine("Clip set to 99999"); // tell user we are gold!
                Thread.Sleep(700); //wait 700ms between messages
                Console.WriteLine("Health set to 99999"); // tell user we are gold!
                Thread.Sleep(700); //wait 700ms between messages
                Console.WriteLine("Armor set to 99999"); // tell user we are gold!
                Thread.Sleep(700); //wait 700ms between messages
                Console.WriteLine("Recoil removed"); // tell user we are gold!
                Thread.Sleep(700); //wait 700ms between messages
                Console.ReadLine(); // Console doesn't close instantly!

            }
        }
    }
}
