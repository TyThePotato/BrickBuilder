using System.Collections;
using System.Collections.Generic;
using BrickBuilder.World;
using UnityEngine;

namespace BrickBuilder.Commands
{
    public class CommandManager
    {
        public static List<Command> CommandHistory = new List<Command>();
        public static int HistoryPosition = -1;
        
        // Adds a command to the command history
        public static void RegisterCommand(Command command)
        {
            if (HistoryPosition < CommandHistory.Count - 1)
            {
                // there are some commands forward in the history, discard them
                CommandHistory.RemoveRange(HistoryPosition + 1, CommandHistory.Count - 1 - HistoryPosition);
            }

            CommandHistory.Add(command);
            HistoryPosition = CommandHistory.Count - 1;
            
            Debug.Log("Successfully registered command of type " + command.Type);
        }

        // Creates and adds a CreateBricks / DeleteBricks command to the command history
        public static Command RegisterCommand(List<Brick> bricks, bool delete = false)
        {
            Command command = new Command();
            command.Type = delete ? Command.CommandType.DeleteBricks : Command.CommandType.CreateBricks;

            command.TargetBricks = new List<Brick>();
            for (int i = 0; i < bricks.Count; i++)
            {
                command.TargetBricks.Add(bricks[i].Clone());
            }
            
            RegisterCommand(command);

            return command;
        }

        // Creates and adds an UpdateBricks command to the command history
        public static Command RegisterCommand(List<Brick> newBricks, List<Brick> previousBricks)
        {
            Command command = new Command();
            command.Type = Command.CommandType.UpdateBricks;

            command.TargetBricks = new List<Brick>();
            for (int i = 0; i < newBricks.Count; i++)
            {
                command.TargetBricks.Add(newBricks[i].Clone());
            }

            command.PreviousBrickData = previousBricks;
            
            RegisterCommand(command);

            return command;
        }

        // Rolls back the latest command
        public static void HistoryBack()
        {
            // ensure we can go backwards
            if (HistoryPosition < 0) return;
            
            UndoCommand(CommandHistory[HistoryPosition]); // undo last command
            HistoryPosition--; // decrement history position
        }

        // Execute next command in history
        public static void HistoryForward()
        {
            // ensure we can go forward
            if (HistoryPosition >= CommandHistory.Count - 1) return;

            HistoryPosition++; // increment history position
            DoCommand(CommandHistory[HistoryPosition]); // do next command
        }

        // Executes specified command
        public static void DoCommand(Command command)
        {
            // why am i not using a switch statement? because.
            if (command.Type == Command.CommandType.CreateBricks)
            {
                ImportBricks(command.TargetBricks);
                
            } else if (command.Type == Command.CommandType.DeleteBricks)
            {
                DeleteBricks(command.TargetBricks);
                
            } else if (command.Type == Command.CommandType.UpdateBricks)
            {
                ModifyBricks(command.TargetBricks);
            } else if (command.Type == Command.CommandType.UpdateEnvironment)
            {
                ModifyEnvironment(command.NewEnvironmentData);
            }
        }

        // Rolls back changes from specified command
        public static void UndoCommand(Command command)
        {
            if (command.Type == Command.CommandType.CreateBricks)
            {
                DeleteBricks(command.TargetBricks);
                
            } else if (command.Type == Command.CommandType.DeleteBricks)
            {
                ImportBricks(command.TargetBricks);

            } else if (command.Type == Command.CommandType.UpdateBricks)
            {
                ModifyBricks(command.PreviousBrickData);
            } else if (command.Type == Command.CommandType.UpdateEnvironment)
            {
                ModifyEnvironment(command.OldEnvironmentData);
            }
        }
        
        // Command Functions

        private static void ImportBricks(List<Brick> bricks)
        {
            // Creates bricks using values specified in 'bricks', including ID
            MapEditor.ImportBricks(bricks, false);
        }

        private static void DeleteBricks(List<Brick> bricks)
        {
            // Removes all bricks from the world referenced in 'bricks' (via ID)
            MapEditor.RemoveBricks(bricks, false);
        }

        private static void ModifyBricks(List<Brick> bricks)
        {
            // Modifies all bricks referenced in 'bricks' (via ID) to have the specified values
            MapEditor.UpdateBricks(bricks);
        }

        private static void ModifyEnvironment((Color, Color, Color, int, int) properties)
        {
            // Updates environment settings using values from 'properties'
            MapEditor.UpdateEnvironment(properties.Item1, properties.Item2, properties.Item3, properties.Item4, properties.Item5);
        }
    }

    public class Command
    {
        public CommandType Type;
        
        // Command properties
        public List<Brick> TargetBricks;
        public List<Brick> PreviousBrickData; // Used when updating brick properties

        public (Color, Color, Color, int, int) NewEnvironmentData;
        public (Color, Color, Color, int, int) OldEnvironmentData;
        
        public enum CommandType
        {
            CreateBricks,
            DeleteBricks,
            UpdateBricks,
            UpdateEnvironment
        }
    }

    public interface ICommand
    {
        void Perform();
        void Unperform();
    }
}