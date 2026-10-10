using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace LabAssignment6_Zork
{
    internal class Program
    {
        //Enum for handling input events
        enum Commands
        {
            QUIT,
            LOOK,
            NORTH,
            SOUTH,
            EAST,
            WEST,
            UNKNOWN
        }

        //2D array of rooms the player can go to
        private static readonly Room[,] rooms = { 
            { new Room("Rocky Trail"), new Room("South of House"), new Room("Canyon View") },
            { new Room("Forest"), new Room("West of House"), new Room("Behind House") },
            { new Room("Dense Woods"), new Room("North of House"), new Room("Clearing") } };

        //Func for initalizing the descriptions of rooms
        private static void InitRoomDesc()
        {
            var roomMap = new Dictionary<string, Room>();

            //For each Room in the rooms array add a slot in the dictionary with a name attached
            foreach(Room room in rooms)
            {
                roomMap[room.name] = room;
            }

            roomMap["Rocky Trail"].description = "You are on a rock-strewn trail.";
            roomMap["South of House"].description = "You are facing the south side of a white house. There is no door here, and all the windows are barred.";
            roomMap["Canyon View"].description = "You are at the top of the Great Canyon on its south wall.";

            roomMap["Forest"].description = "This is a forest, with trees in all directions around you.";
            roomMap["West of House"].description = "This is an open field west of a white house, with a boarded front door.";
            roomMap["Behind House"].description = "You are behind the white house. In one corner of the house there is a small window which is slightly ajar.";

            roomMap["Dense Woods"].description = "This is a dimly lit forest, with large trees all around. To the east, there appears to be sunlight.";
            roomMap["North of House"].description = "You are facing the north side of a white house. There is no door here, and all the windows are barred.";
            roomMap["Clearing"].description = "You are in a clearing, with a forest surrounding you on the west and south.";
        }

        //variable referencing the location in the rooms 2D array.
        private static (int Row, int Column) location = (1, 1);

        //A variable referencing the current room the player is in.
        private static Room currRoom
        {
            get
            {
                return rooms[location.Row,location.Column];
            }
        }

        //A list of commands that count as directions
        private static readonly List<Commands> directions = new List<Commands>
        {
            Commands.NORTH,
            Commands.SOUTH,
            Commands.EAST,
            Commands.WEST
        };

        static void Main(string[] args)
        {
            InitRoomDesc(); //Initialize room list
            Console.WriteLine("Welcome to Zork!"); //Print a welcome message

            Room prevRoom = null; //Set the previous room to null

            Commands commands = Commands.UNKNOWN; //Set the current command to unknown

            //Run this until the player quits.
            while(commands != Commands.QUIT)
            {
                //If the rooms change print a message with the description of the new room
                if(prevRoom != currRoom)
                {
                    Console.WriteLine(currRoom.description);
                    prevRoom = currRoom; //Update the previous room for future use
                }

                Console.Write("> "); //Indicate to the player they can input something
                commands = ToCommand(Console.ReadLine().Trim()); //set the commands var to whatever the player puts in

                string outputString; //string var for what to print after the switch statement
                switch (commands)
                {
                    //If the player quits print a thanks for playing message
                    case Commands.QUIT:
                        outputString = "Thank you for playing!\nPress any key to continue...";
                        break;
                    //If the player looks, tell them a description of the room around them
                    case Commands.LOOK:
                        outputString = currRoom.description;
                        break;
                    //If a direction is inputted move the player
                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        //If the player can move in that direction tell them they moved that way and what room they're moving to
                        if (Move(commands))
                        {
                            outputString = $"You moved {commands}.\n{currRoom.ToString()}.";
                        }
                        else
                        {
                            //If the player can't move that way tell them the way is shut
                            outputString = $"The way is shut!";
                        }
                        break;
                    default:
                        //If an unknown command is entered warn the player that is the case.
                        outputString = "Unknown command.";
                        break;
                }
                //Print the result of the switch case statement.
                Console.WriteLine(outputString);
            }
        }

        /*Func to get the player's input and translate it into a command
         * Originally written as a more 'proper' function with brackets due to personal preference
         * however to align with the tutorial we were meant to follow, it was reformatted to match.
         */
        private static Commands ToCommand(string commandString) => Enum.TryParse(commandString, true, out Commands result) ? result : Commands.UNKNOWN;

        //Func for moving between rooms
        private static bool Move(Commands command)
        {
            //Check if the entered command is a direction
            Assert.IsTrue(IsDirection(command), "Invalid direction.");

            bool isValidMove = true; //Set the valid move to true

            switch (command)
            {
                //Execute this case for the player validly moving east
                case Commands.EAST when location.Column < rooms.GetLength(0) - 1:
                    location.Column++;
                    break;
                //Execute this case for the player validly moving west
                case Commands.WEST when location.Column > 0:
                    location.Column--;
                    break;
                //Execute this case for the player validly moving north
                case Commands.NORTH when location.Row < rooms.GetLength(1) - 1:
                    location.Row++;
                    break;
                //Execute this case for the player validly going south
                case Commands.SOUTH when location.Row > 0:
                    location.Row--;
                    break;
                //If the move is invalid use this case
                default:
                    isValidMove = false;
                    break;
            }

            return isValidMove; //Return the validity of the move
        }

        /*Func to check if the directions list contains the entered command
         * Similarly to the ToCommand function this was originally a more bulky
         * function with an if statement inside due to personal preference for code readability.
         * However it was re-written to match tutorial code. The two longer comments are here to communicate
         * that I was paying attention during the tutorials rather than just following them verbatim
         */
        private static bool IsDirection(Commands command) => directions.Contains(command);

        //Assert func needed as a debug func in the move func.
        public static class Assert
        {
            //Checks if a bool statement is true, with an error message if the expression is false.
            [Conditional("DEBUG")]
            public static void IsTrue(bool expression, string message = null)
            {
                if(expression == false)
                {
                    throw new Exception(message);
                }
            }
        }
    }

    //Class for a room the player can travel to
    public class Room
    {
        public string name { get; } //Name of the room
        public string description { get; set; } //Description of the room

        //Initializer for the room
        public Room(string legalName, string desc = "")
        {
            name = legalName;
            description = desc;
        }

        //Override func for printing the name of a room as the name var within the room
        public override string ToString()
        {
            return name;
        }
    }
}
