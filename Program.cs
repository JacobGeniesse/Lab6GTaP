using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace LabAssignment6_Zork
{
    internal class Program
    {
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

        private static readonly Room[,] rooms = { 
            { new Room("Rocky Trail"), new Room("South of House"), new Room("Canyon View") },
            { new Room("Forest"), new Room("West of House"), new Room("Behind House") },
            { new Room("Dense Woods"), new Room("North of House"), new Room("Clearing") } };

        private static void InitRoomDesc()
        {
            var roomMap = new Dictionary<string, Room>();

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



        private static (int Row, int Column) location = (1, 1);

        private static Room currRoom
        {
            get
            {
                return rooms[location.Row,location.Column];
            }
        }

        private static readonly List<Commands> directions = new List<Commands>
        {
            Commands.NORTH,
            Commands.SOUTH,
            Commands.EAST,
            Commands.WEST
        };

        static void Main(string[] args)
        {
            InitRoomDesc();
            Console.WriteLine("Welcome to Zork!");

            Room prevRoom = null;

            Commands commands = Commands.UNKNOWN;

            while(commands != Commands.QUIT)
            {
                if(prevRoom != currRoom)
                {
                    Console.WriteLine(currRoom.description);
                    prevRoom = currRoom;
                }

                Console.Write("> ");
                commands = ToCommand(Console.ReadLine().Trim());

                string outputString;
                switch (commands)
                {
                    case Commands.QUIT:
                        outputString = "Thank you for playing!\nPress any key to continue...";
                        break;
                    case Commands.LOOK:
                        outputString = currRoom.description;
                        break;
                    case Commands.NORTH:
                    case Commands.SOUTH:
                    case Commands.EAST:
                    case Commands.WEST:
                        if (Move(commands))
                        {
                            outputString = $"You moved {commands}.\n{currRoom.ToString()}.";
                        }
                        else
                        {
                            outputString = $"The way is shut!";
                        }
                        break;
                    default:
                        outputString = "Unknown command.";
                        break;
                }

                Console.WriteLine(outputString);
            }
        }

        private static Commands ToCommand(string commandString)
        {
            if(Enum.TryParse<Commands>(commandString, true, out Commands result))
            {
                return result;
            }
            else
            {
                return Commands.UNKNOWN;
            }
        }

        private static bool Move(Commands command)
        {
            Assert.IsTrue(IsDirection(command), "Invalid direction.");

            bool isValidMove = true;

            switch (command)
            {
                case Commands.EAST when location.Column < rooms.GetLength(0) - 1:
                    location.Column++;
                    break;
                case Commands.WEST when location.Column > 0:
                    location.Column--;
                    break;
                case Commands.NORTH when location.Row < rooms.GetLength(1) - 1:
                    location.Row++;
                    break;
                case Commands.SOUTH when location.Row > 0:
                    location.Row--;
                    break;
                default:
                    isValidMove = false;
                    break;
            }

            return isValidMove;
        }
        private static bool IsDirection(Commands command)
        {
            if (directions.Contains(command))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static class Assert
        {
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

    public class Room
    {
        public string name { get; }
        public string description { get; set; }

        public Room(string legalName, string desc = "")
        {
            name = legalName;
            description = desc;
        }

        public override string ToString()
        {
            return name;
        }
    }
}
